using System;
using System.Collections.Generic;
using System.Linq;

namespace Parte3;

public class MT
{
    public string EstadoInicial { get; set; } = "";
    public bool MTReconhecedor { get; set; } = true; // Define se a MT é reconhecedora (aceita por estado de aceitação) ou geradora (aceita por resultado na fita)
    public HashSet<string> EstadosAceitacao { get; set; } = new();
    public int LimitePassos { get; set; } = 100;

    private Dictionary<(string estado, char simbolo), (string novoEstado, char novoSimbolo, char direcao)> transicoes = new();

    public void AdicionarTransicao(string estado, char simbolo, string novoEstado, char novoSimbolo, char direcao)
    {
        transicoes[(estado, simbolo)] = (novoEstado, novoSimbolo, char.ToUpper(direcao));
    }

    public class PassoMT
    {
        public int Numero { get; set; }
        public string Estado { get; set; } = "";
        public string Fita { get; set; } = "";
        public int PosicaoCabecote { get; set; }
    }

    public (bool aceito, string motivo, List<PassoMT> historico) Executar(string entrada)
    {
        // Implemente a fita como estrutura dinâmica
        var fita = new Dictionary<int, char>();
        for (int i = 0; i < entrada.Length; i++)
            fita[i] = entrada[i];

        int cabecote = 1;
        string estadoAtual = EstadoInicial;
        int passos = 0;
        var historico = new List<PassoMT>();

        while (passos < LimitePassos)
        {
            // O espaço em branco é representado por '_'
            char simboloLido = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

            // Salva o estado atual para exibir a cada passo
            historico.Add(new PassoMT
            {
                Numero = passos,
                Estado = estadoAtual,
                Fita = FormatarFita(fita, cabecote),
                PosicaoCabecote = cabecote
            });

            // Verifica se existe transição definida e define a próxima ação, podendos finalizar ou continuar a execução
            if (!transicoes.TryGetValue((estadoAtual, simboloLido), out var acao)){
                if (EstadosAceitacao.Contains(estadoAtual))
                    return (true, "Palavra aceita!", historico);
                if (MTReconhecedor==false)
                {
                    int min = fita.Keys.DefaultIfEmpty(0).Min();
                    int max = fita.Keys.DefaultIfEmpty(0).Max();
                    string resultadoFita = "";
                    for (int i = min; i <= max; i++)
                    {
                        if (i == 0) continue;
                        resultadoFita += fita.ContainsKey(i) ? fita[i] : '_';
                    }
                    resultadoFita = resultadoFita.Trim('_');
                    return (true, resultadoFita, historico);
                }
                return (false, $"Rejeitado, sem transição para δ({estadoAtual}, '{simboloLido}') e está fora de um estado de aceitação.", historico);
            }

            // Aplica a transição na fita
            fita[cabecote] = acao.novoSimbolo;
            estadoAtual = acao.novoEstado;
            
            // Movimenta o cabeçote
            cabecote += (acao.direcao == 'D') ? 1 : -1;
            
            passos++;
        }

        return (false, $"Rejeitado (Limite de {LimitePassos} passos atingido - Loop infinito).", historico);
    }

    // Método que gera a fita visual com os delimitadores [ ]
    private string FormatarFita(Dictionary<int, char> fita, int cabecote)
    {
        if (fita.Count == 0) return "[_]";
        
        // Descobre os limites da fita para imprimir continuamente
        int min = Math.Min(fita.Keys.DefaultIfEmpty(0).Min(), cabecote);
        int max = Math.Max(fita.Keys.DefaultIfEmpty(0).Max(), cabecote);
        
        var fitaFormatada = "";
        for (int i = min; i <= max; i++)
        {
            char c = fita.ContainsKey(i) ? fita[i] : '_';
            
            // Delimitadores ao redor do símbolo sob o cabeçote
            if (i == cabecote) fitaFormatada += $"[{c}]";
            else fitaFormatada += c;
        }
        return fitaFormatada;
    }
}