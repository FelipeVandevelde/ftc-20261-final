using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato_De_Pilha
{
    internal class APD
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
            (Estado estado, char? entrada, char topo),
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
            bool impar = false, par = false;

            if (ValidaSimbolos(entrada, out par, out impar))
            {
                if (opcao == "1")
                {
                    MontarTransicoesL2();

                    Estado Estado_Atual = this.Estado_Inicial;

                    foreach (var simbolo in entrada)
                    {
                        char topo = Pilha.Peek();

                        if (!Funcao_Transicao.TryGetValue(
                            (Estado_Atual, simbolo, topo),
                            out var transicoes))
                        {
                            return false;
                        }

                        var t = transicoes[0];

                        if (t.desempilhar)
                            Pilha.Pop();

                        if (t.simbolo_para_empilhar.HasValue)
                            Pilha.Push(t.simbolo_para_empilhar.Value);

                        Estado_Atual = t.proximo_estado;
                    }

                    return Pilha.Count == 1 && Pilha.Peek() == Simbolo_Inicial_Pilha;
                }
                else if (opcao == "2")
                {
                    if(par)
                        MontarTransicoesL3_Par();
                    if (impar)
                        MontarTransicoesL3_Impar();

                    return ExecutarL3(this.Estado_Inicial, entrada, 0, Pilha);

                    bool ExecutarL3(Estado Estado_Atual, string entrada, int posicao, Stack<char> Pilha)
                    {
                        // Aceita quando consumiu toda entrada e a pilha está logicamente vazia
                        if (posicao == entrada.Length)
                            return Pilha.Count == 1 && Pilha.Peek() == Simbolo_Inicial_Pilha;

                        char simbolo = entrada[posicao];
                        char topo = Pilha.Peek();

                        // Se não existe transição, rejeita este caminho
                        if (!Funcao_Transicao.TryGetValue(
                            (Estado_Atual, simbolo, topo),
                            out var transicoes))
                        {
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

                            // Recursividade para que o APND verifique vários caminhos possíveis ao mesmo tempo.
                            if (ExecutarL3(t.proximo_estado, entrada, posicao+1, Nova_Pilha))
                                return true;
                        }

                        return false;
                    }
                }
                else
                {
                    Console.Write("Encerrando Programa...");
                    Thread.Sleep(2000);
                    return false;
                }
            }
            else
            {
                Console.Clear();
                Console.Write("Alguns simbolos não pertencem ao Alfabeto de entrada.\n\n");
                Thread.Sleep(3000);
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
                }
            };
        }

        public void MontarTransicoesL3_Impar()
        {
            Pilha.Clear();
            Pilha.Push(this.Simbolo_Inicial_Pilha);
            Funcao_Transicao.Clear();
            Estado q0 = this.Estado_Inicial;
            Estado q1 = new Estado("q1", false, false);

            this.Funcao_Transicao = new()
            {
                { (q0, 'a', 'Z'),
                    new()
                    {
                        (q0, 'A', false),
                        (q1, null, false)
                    }
                },

                { (q0, 'a', 'A'),
                    new()
                    {
                        (q0, 'A', false),
                        (q1, null, false)
                    }
                },

                { (q0, 'a', 'B'),
                    new()
                    {
                        (q0, 'A', false),
                        (q1, null, false)
                    }
                },

                { (q0, 'b', 'Z'),
                    new() { (q0, 'B', false) }
                },

                { (q0, 'b', 'A'),
                    new()
                    {
                        (q0, 'B', false),
                        (q1, null, false)
                    }
                },

                { (q0, 'b', 'B'),
                    new()
                    {
                        (q0, 'B', false),
                        (q1, null, false)
                    }
                },

                { (q0, null, 'A'),
                    new() { (q1, null, false) }
                },

                { (q0, null, 'B'),
                    new() { (q1, null, false) }
                },

                { (q1, 'a', 'A'),
                    new() { (q1, null, true) }
                },

                { (q1, 'b', 'B'),
                    new() { (q1, null, true) } 
                }
            };
        }

        public void MontarTransicoesL3_Par()
        {
            Pilha.Clear();
            Pilha.Push(this.Simbolo_Inicial_Pilha);
            Funcao_Transicao.Clear();
            Estado q0 = this.Estado_Inicial;
            Estado q1 = new Estado("q1", false, false);

            this.Funcao_Transicao = new()
    {
        // empilha
        {
            (q0, 'a', 'Z'),
            new() { (q0, 'A', false) }
        },

        {
            (q0, 'b', 'Z'),
            new() { (q0, 'B', false) }
        },

        {
            (q0, 'a', 'A'),
            new()
            {
                (q0, 'A', false),
                (q1, null, true)
            }
        },

        {
            (q0, 'a', 'B'),
            new()
            {
                (q0, 'A', false)
            }
        },

        {
            (q0, 'b', 'B'),
            new()
            {
                (q0, 'B', false),
                (q1, null, true)
            }
        },

        {
            (q0, 'b', 'A'),
            new()
            {
                (q0, 'B', false)
            }
        },

        // comparação
        {
            (q1, 'a', 'A'),
            new() { (q1, null, true) }
        },

        {
            (q1, 'b', 'B'),
            new() { (q1, null, true) }
        }
    };
        }

        public bool ValidaSimbolos(string entrada, out bool tamanhoPar, out bool tamanhoImpar)
        {
            if(entrada.Length % 2 == 0)
            {
                tamanhoPar = true; tamanhoImpar = false;
            }
            else
            {
                tamanhoImpar = true; tamanhoPar = false;
            }

            foreach (var simbolo in entrada)
                if (!Alfabeto_Entrada.Contains(simbolo))
                    return false;

            return true;
        }

        #endregion
    }
}
