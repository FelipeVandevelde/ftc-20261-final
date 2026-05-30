using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parte2;

public class  APD
{
    #region 7-tupla do APD

    // Conjunto de estados Q
    public List<Estado> Estados { get; set; } = new List<Estado>();


    // Alfabeto de entrada Σ
    public List<char> Alfabeto_Entrada { get; set; } = new List<char> {'a', 'b'};


    // Alfabeto de pilha Γ
    public List<char> Alfabeto_Pilha { get; set; } = new List<char>();


    // Função de transição δ
    public Dictionary<
        (Estado estado, char entrada, char topo),
        List<(Estado proximo_estado,
                char? simbolo_para_empilhar,
                bool desempilhar)>
    > Funcao_Transicao
    { get; set; } = new();


    // Estado inicial q0 ∈ Q
    public Estado Estado_Inicial { get; set; } = new Estado("q0", true, false);


    // Símbolo inicial de pilha Z0 ∈ Γ
    public char Simbolo_Inicial_Pilha { get; set; } = 'Z';


    // Estado final = null implica aceitação por pilha vazia
    public Estado? Estado_Final { get; set; } = null;

    #endregion

    #region Pilha do APD

    public Stack<char> Pilha = new();

    #endregion

    #region Funções e Procedimentos

    public string MenuAPD()
    {
        // Linguagem-alvo L2
        Console.WriteLine("Digite 1  ->  L₂ = { aⁿbⁿ | n ≥ 1 }");

        // Linguagem-alvo L3
        Console.WriteLine("\nDigite 2  ->  L₃ = { w ∈ {a,b}* | w = wᴿ, |w| >= 1 }");

        Console.Write("\nEscolha uma linguagem: ");
        string opcao = Console.ReadLine() ?? "";

        return opcao;
    }

    public bool Executar(string entrada, string opcao)
    {
        Estado Estado_Morto = new Estado("qM", false, false);

        if (opcao == "1")
            MontarTransicoesL2();
        else if (opcao == "2")
            MontarTransicoesL3();
        else
        {
            Console.Write("Encerrando Programa...");
            Thread.Sleep(2000);
            return false;
        }

        ConfiguracaoInstantanea(
            this.Estado_Inicial,
            entrada,
            Pilha
        );

        return ExecutarAPND(
            this.Estado_Inicial,
            entrada,
            0,
            Pilha
        );

        bool ExecutarAPND(
            Estado Estado_Atual,
            string entrada,
            int posicao,
            Stack<char> Pilha)
        {
            if (Pilha.Count == 0)
                return posicao == entrada.Length;

            char topo = Pilha.Peek();

            // Lista contendo transições normais e λ
            List<(
                Estado proximo_estado,
                char? simbolo_para_empilhar,
                bool desempilhar,
                bool consomeEntrada
            )> transicoes = new();

            // Busca transições normais somente se ainda existe entrada
            if (posicao < entrada.Length)
            {
                char simbolo = entrada[posicao];

                if (Funcao_Transicao.TryGetValue(
                    (Estado_Atual, simbolo, topo),
                    out var transicoesNormais))
                {
                    foreach (var t in transicoesNormais)
                    {
                        transicoes.Add((
                            t.proximo_estado,
                            t.simbolo_para_empilhar,
                            t.desempilhar,
                            true
                        ));
                    }
                }
            }

            // Busca λ-transições
            if (Funcao_Transicao.TryGetValue(
                (Estado_Atual, '\0', topo),
                out var transicoesLambda))
            {
                foreach (var t in transicoesLambda)
                {
                    transicoes.Add((
                        t.proximo_estado,
                        t.simbolo_para_empilhar,
                        t.desempilhar,
                        false
                    ));
                }
            }

            // Aceita somente quando toda entrada foi consumida e pilha ficou vazia
            if (posicao == entrada.Length
                && transicoes.Count == 0)
            {
                return Pilha.Count == 0;
            }

            // Se não existe nenhuma transição possível
            if (transicoes.Count == 0)
            {
                Estado_Atual = Estado_Morto;

                ConfiguracaoInstantanea(
                    Estado_Atual,
                    entrada.Substring(posicao),
                    Pilha
                );

                return false;
            }

            // Testa todas as possibilidades
            foreach (var t in transicoes)
            {
                // Clonar a pilha para que cada caminho possível tenha sua própria pilha independente.
                Stack<char> Nova_Pilha =
                    new Stack<char>(Pilha.Reverse());

                // Se a transição pede para desempilhar, faça isso
                if (t.desempilhar)
                    Nova_Pilha.Pop();

                // Se a transição pede para empilhar, faça isso
                if (t.simbolo_para_empilhar.HasValue)
                    Nova_Pilha.Push(
                        t.simbolo_para_empilhar.Value
                    );

                int proximaPosicao =
                    t.consomeEntrada
                        ? posicao + 1
                        : posicao;

                string entradaRestante =
                    proximaPosicao < entrada.Length
                        ? entrada.Substring(proximaPosicao)
                        : "";

                ConfiguracaoInstantanea(
                    t.proximo_estado,
                    entradaRestante,
                    Nova_Pilha
                );

                // Recursividade para que o APND verifique vários caminhos possíveis ao mesmo tempo.
                if (ExecutarAPND(
                    t.proximo_estado,
                    entrada,
                    proximaPosicao,
                    Nova_Pilha))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void MontarTransicoesL2()
    {
        Pilha.Clear();
        Pilha.Push(this.Simbolo_Inicial_Pilha);
        Funcao_Transicao.Clear();
        Estado q0 = this.Estado_Inicial;
        Estado q1 = new Estado("q1", false, true);

        this.Funcao_Transicao = new()
        {
            // Para q0 -> q0 lendo a
            { (q0, 'a', 'Z'),
                new() { (q0, 'X', false) }
            },

            { (q0, 'a', 'X'),
                new() {(q0, 'X', false) }
            },

            // Para q0 -> q1 lendo b
            { (q0, 'b', 'X'),
                new() { (q1, null, true) }
            },

            // Para q1 -> q1 lendo b
            { (q1, 'b', 'X'),
                new() {(q1, null, true) }
            },

            // Para remover Z no final do processamento
            { (q1, '\0', 'Z'),
                new() { (q1, null, true) }
            }
        };
    }

    public void MontarTransicoesL3()
    {
        Pilha.Clear();
        Pilha.Push(this.Simbolo_Inicial_Pilha);
        Funcao_Transicao.Clear();

        Estado q0 = this.Estado_Inicial;
        Estado q1 = new Estado("q1", false, false);

        this.Funcao_Transicao = new()
        {
            // q0 -> q0 (empilha)
            {
                (q0, 'a', 'Z'),
                new()
                {
                    (q0, 'A', false),
                    (q1, null, false)   // palavra "a"
                }
            },

            {
                (q0, 'a', 'A'),
                new()
                {
                    (q0, 'A', false),
                    (q1, null, false)   // centro ímpar
                }
            },

            {
                (q0, 'a', 'B'),
                new()
                {
                    (q0, 'A', false),
                    (q1, null, false)   // centro ímpar
                }
            },

            {
                (q0, 'b', 'Z'),
                new()
                {
                    (q0, 'B', false),
                    (q1, null, false)   // palavra "b"
                }
            },

            {
                (q0, 'b', 'A'),
                new()
                {
                    (q0, 'B', false),
                    (q1, null, false)   // centro ímpar
                }
            },

            {
                (q0, 'b', 'B'),
                new()
                {
                    (q0, 'B', false),
                    (q1, null, false)   // centro ímpar
                }
            },

            // λ-transições caso par
            {
                (q0, '\0', 'A'),
                new()
                {
                    (q1, null, false)
                }
            },

            {
                (q0, '\0', 'B'),
                new()
                {
                    (q1, null, false)
                }
            },

            // q1 -> q1 (compara)
            {
                (q1, 'a', 'A'),
                new()
                {
                    (q1, null, true)
                }
            },

            {
                (q1, 'b', 'B'),
                new()
                {
                    (q1, null, true)
                }
            },

            // Remove Z no final
            {
                (q1, '\0', 'Z'),
                new()
                {
                    (q1, null, true)
                }
            }
        };
    }

    public void ConfiguracaoInstantanea(
        Estado estado,
        string entradaRestante,
        Stack<char> Pilha)
    {
        if (entradaRestante.Length == 0)
            entradaRestante = "\\0";

        string pilha = Pilha.Count == 0
            ? "\\0"
            : string.Join("", Pilha.Reverse());

        Console.WriteLine(
            $"\n({estado.Nome}, {entradaRestante}, {pilha})"
        );
    }

    #endregion
}