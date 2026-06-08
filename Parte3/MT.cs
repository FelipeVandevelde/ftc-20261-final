using System;
using System.Collections.Generic;
using System.Linq;

namespace Parte3;

public class MT
{
    public record Estado(string Nome); // Classe para representar os estados da máquina
    public record Simbolo(char Valor); // Classe para representar os símbolos da fita e de entrada
    public HashSet<Estado> Estados { get; set; } = new(); // E = Conjunto de estados da máquina
    public HashSet<Simbolo> AlfabetoEntrada { get; set; } = new(); // Σ = Alfabeto de entrada
    public HashSet<Simbolo> AlfabetoFita { get; set; } = new(); // Γ = Alfabeto da fita (inclui símbolos de marcação e branco)
    private Dictionary<(Estado estado, Simbolo simbolo), (Estado novoEstado, Simbolo novoSimbolo, char direcao)> transicoes = new(); // δ = Função de transição
    public Estado? EstadoInicial { get; set; } // q0 = Estado inicial
    public Estado? EstadoAceitacao { get; set; } // qaccept = Conjunto de estados de aceitação
    public Estado? EstadoRejeicao { get; set; } // qreject = Conjunto de estados de rejeição

    public bool MTReconhecedora { get; set; } = true; // Propriedade para definir se a máquina é reconhecedora ou transdutora
    public int LimitePassos { get; set; } = 1000; // Propriedade para definir o limite de passos da execução, usada para evitar loops infinitos
    
    public void AdicionarEstados(params string[] nomes) // Método para adicionar estados à máquina
    {
        foreach (var nome in nomes)
            Estados.Add(new Estado(nome));
    }

    public void AdicionarAlfabetoEntrada(params char[] nomes) // Método para adicionar símbolos ao alfabeto de entrada
    {
        foreach (var nome in nomes)
            AlfabetoEntrada.Add(new Simbolo(nome));
    }

    public void AdicionarAlfabetoFita(params char[] nomes) // Método para adicionar símbolos ao alfabeto da fita
    {
        foreach (var nome in nomes)
            AlfabetoFita.Add(new Simbolo(nome));
    }
    
    public void AdicionarTransicao(string estado, char simbolo, string novoEstado, char novoSimbolo, char direcao) // Método para adicionar transições à máquina
    {
        var estadoObj = Estados.FirstOrDefault(e => e.Nome == estado) ?? throw new Exception($"Estado '{estado}' não encontrado.");
        var novoEstadoObj = Estados.FirstOrDefault(e => e.Nome == novoEstado) ?? throw new Exception($"Estado '{novoEstado}' não encontrado.");
        var simboloObj = AlfabetoFita.FirstOrDefault(s => s.Valor == simbolo) ?? throw new Exception($"Símbolo '{simbolo}' não encontrado no alfabeto da fita.");
        var novoSimboloObj = AlfabetoFita.FirstOrDefault(s => s.Valor == novoSimbolo) ?? throw new Exception($"Símbolo '{novoSimbolo}' não encontrado no alfabeto da fita.");
        transicoes[(estadoObj, simboloObj)] = (novoEstadoObj, novoSimboloObj, char.ToUpper(direcao));
    }
    public Estado ObterEstado(string estado) // Método para obter um estado pelo nome
    {
        var estadoObj = Estados.FirstOrDefault(e => e.Nome == estado) ?? throw new Exception($"Estado '{estado}' não encontrado.");
        return estadoObj;
    }

    public class PassoMT // Classe para representar cada passo da execução da máquina, usada para registrar o histórico de execução
    {
        public int Numero { get; set; }
        public string Estado { get; set; } = "";
        public string Fita { get; set; } = "";
        public int PosicaoCabecote { get; set; }
    }

    public (bool aceito, string motivo, List<PassoMT> historico) Executar(string entrada) // Método para executar a máquina com uma palavra de entrada
    {
        var fita = new Dictionary<int, Simbolo>(); // Representação da fita usando um dicionário para permitir posições negativas e infinitas em ambas as direções
        for (int i = 0; i < entrada.Length; i++) // Inicializa a fita com os símbolos da entrada, usando o índice como posição na fita
            fita[i] = new Simbolo(entrada[i]);

        int cabecote = 0; // O cabeçote começa na posição 0 da fita
        if (EstadoInicial == null) throw new Exception("Estado inicial não foi definido na máquina."); // Verificação para garantir que o estado inicial foi definido
            Estado estadoAtual = EstadoInicial;
        int passos = 0; // Contador de passos para evitar loops infinitos, comparado com o LimitePassos definido pelo usuário
        var historico = new List<PassoMT>(); // Lista para registrar o histórico de execução da máquina, usada para exibir o passo a passo no console

        while (passos < LimitePassos) // Loop principal de execução da máquina, que continua até atingir o limite de passos para evitar loops infinitos
        {
            if (estadoAtual == EstadoAceitacao) // Verificação de aceitação
            {
                RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);
                
                // Se for transdutora, retorna a fita processada
                if (!MTReconhecedora)
                    return (true, ExtrairResultadoTransdutor(fita), historico);
                
                return (true, "Palavra aceita!", historico);
            }

            if (estadoAtual == EstadoRejeicao) // Verificação de rejeição
            {
                RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);
                return (false, "Palavra rejeitada!", historico);
            }

            Simbolo simboloLido = fita.ContainsKey(cabecote) ? fita[cabecote] : new Simbolo('_'); // Lê o símbolo na posição atual do cabeçote, ou assume branco (_) se não houver símbolo definido nessa posição

            RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);

            if (!transicoes.TryGetValue((estadoAtual, simboloLido), out var acao)) // Verifica se existe uma transição definida para o estado atual e o símbolo lido, se não existir, a máquina leva a palavra para o estado de rejeição
            {
                estadoAtual = EstadoRejeicao;
                continue;
            }

            // Aplica a transição
            fita[cabecote] = acao.novoSimbolo;
            estadoAtual = acao.novoEstado;
            
            // Movimenta o cabeçote usando R e L
            cabecote += (acao.direcao == 'R') ? 1 : -1;
            
            passos++;
        }

        return (false, $"Rejeitado (Limite de {LimitePassos} passos atingido - Loop infinito).", historico);
    }

    // Métodos auxiliares para registrar o histórico e formatar a fita para visualização
    private void RegistrarHistorico(List<PassoMT> historico, int passos, Estado estado, Dictionary<int, Simbolo> fita, int cabecote) // Método para registrar o estado atual, a fita e a posição do cabeçote a cada passo da execução, usado para exibir o passo a passo no console
    {
        historico.Add(new PassoMT
        {
            Numero = passos,
            Estado = estado.Nome, // Extrai o nome da classe Estado para o visual
            Fita = FormatarFita(fita, cabecote),
            PosicaoCabecote = cabecote
        });
    }

    private string ExtrairResultadoTransdutor(Dictionary<int, Simbolo> fita) // Método para extrair o resultado final da fita processada, usado para máquinas transdutoras que geram uma saída na fita
    {
        if (fita.Count == 0) return "";
        int min = fita.Keys.DefaultIfEmpty(0).Min();
        int max = fita.Keys.DefaultIfEmpty(0).Max();
        string resultado = "";
        
        for (int i = min; i <= max; i++)
        {
            // Pega o caractere primitivo (.Valor) de dentro do objeto Simbolo
            resultado += fita.ContainsKey(i) ? fita[i].Valor : '_';
        }
        return resultado.Trim('_');
    }

    private string FormatarFita(Dictionary<int, Simbolo> fita, int cabecote) // Método para formatar a fita em uma string visual, mostrando o símbolo atual do cabeçote entre colchetes, usado para exibir o passo a passo no console
    {
        if (fita.Count == 0) return "[_]";
        
        int min = Math.Min(fita.Keys.DefaultIfEmpty(0).Min(), cabecote);
        int max = Math.Max(fita.Keys.DefaultIfEmpty(0).Max(), cabecote);
        
        var fitaFormatada = "";
        for (int i = min; i <= max; i++)
        {
            // Pega o caractere primitivo (.Valor) de dentro do objeto Simbolo
            char c = fita.ContainsKey(i) ? fita[i].Valor : '_';
            if (i == cabecote) fitaFormatada += $"[{c}]";
            else fitaFormatada += c;
        }
        return fitaFormatada;
    }
}