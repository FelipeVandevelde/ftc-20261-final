using System;
using System.Collections.Generic;
using System.Linq;

namespace Parte3;

public class MT
{
    public record Estado(string Nome);
    public record Simbolo(char Valor);
    public HashSet<Estado> Estados { get; set; } = new();
    public HashSet<Simbolo> AlfabetoEntrada { get; set; } = new();
    public HashSet<Simbolo> AlfabetoFita { get; set; } = new();
    private Dictionary<(Estado estado, Simbolo simbolo), (Estado novoEstado, Simbolo novoSimbolo, char direcao)> transicoes = new();
    public Estado? EstadoInicial { get; set; }
    public Estado? EstadoAceitacao { get; set; }
    public Estado? EstadoRejeicao { get; set; }
    
    public bool MTReconhecedora { get; set; } = true; 
    public int LimitePassos { get; set; } = 1000;
    
    public void AdicionarEstados(params string[] nomes)
    {
        foreach (var nome in nomes)
            Estados.Add(new Estado(nome));
    }
    public void AdicionarAlfabetoFita(params char[] nomes)
    {
        foreach (var nome in nomes)
            AlfabetoFita.Add(new Simbolo(nome));
    }
    public void AdicionarAlfabetoEntrada(params char[] nomes)
    {
        foreach (var nome in nomes)
            AlfabetoEntrada.Add(new Simbolo(nome));
    }
    public void AdicionarTransicao(string estado, char simbolo, string novoEstado, char novoSimbolo, char direcao)
    {
        var estadoObj = Estados.FirstOrDefault(e => e.Nome == estado) ?? throw new Exception($"Estado '{estado}' não encontrado.");
        var novoEstadoObj = Estados.FirstOrDefault(e => e.Nome == novoEstado) ?? throw new Exception($"Estado '{novoEstado}' não encontrado.");
        var simboloObj = AlfabetoFita.FirstOrDefault(s => s.Valor == simbolo) ?? throw new Exception($"Símbolo '{simbolo}' não encontrado no alfabeto da fita.");
        var novoSimboloObj = AlfabetoFita.FirstOrDefault(s => s.Valor == novoSimbolo) ?? throw new Exception($"Símbolo '{novoSimbolo}' não encontrado no alfabeto da fita.");
        transicoes[(estadoObj, simboloObj)] = (novoEstadoObj, novoSimboloObj, char.ToUpper(direcao));
    }
    public Estado ObterEstado(string estado)
    {
        var estadoObj = Estados.FirstOrDefault(e => e.Nome == estado) ?? throw new Exception($"Estado '{estado}' não encontrado.");
        return estadoObj;
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
        var fita = new Dictionary<int, Simbolo>();
        for (int i = 0; i < entrada.Length; i++)
            fita[i] = new Simbolo(entrada[i]);

        int cabecote = 0; 
        if (EstadoInicial == null) throw new Exception("Estado inicial não foi definido na máquina.");
            Estado estadoAtual = EstadoInicial;
        int passos = 0;
        var historico = new List<PassoMT>();

        while (passos < LimitePassos)
        {
            if (estadoAtual == EstadoAceitacao)
            {
                RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);
                
                // Se for transdutora, retorna a fita processada
                if (!MTReconhecedora)
                    return (true, ExtrairResultadoTransdutor(fita), historico);
                
                return (true, "Palavra aceita!", historico);
            }

            if (estadoAtual == EstadoRejeicao)
            {
                RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);
                return (false, "Palavra rejeitada!", historico);
            }

            Simbolo simboloLido = fita.ContainsKey(cabecote) ? fita[cabecote] : new Simbolo('_');

            RegistrarHistorico(historico, passos, estadoAtual, fita, cabecote);

            // 4. Transição implícita: sem regra? Vai pro estado de rejeição
            if (!transicoes.TryGetValue((estadoAtual, simboloLido), out var acao))
            {
                estadoAtual = EstadoRejeicao;
                continue; // Volta pro topo do laço para registrar a parada no próximo passo
            }

            // Aplica a transição
            fita[cabecote] = acao.novoSimbolo;
            estadoAtual = acao.novoEstado;
            
            // 5. Movimenta o cabeçote usando R e L
            cabecote += (acao.direcao == 'R') ? 1 : -1;
            
            // Regra da 7-Tupla: Se tentar cair da fita pela esquerda, fica parado na posição 0
            /*if (cabecote < 0) 
                cabecote = 0;*/
            
            passos++;
        }

        return (false, $"Rejeitado (Limite de {LimitePassos} passos atingido - Loop infinito).", historico);
    }

    // Métodos auxiliares para manter o Executar() limpo
    private void RegistrarHistorico(List<PassoMT> historico, int passos, Estado estado, Dictionary<int, Simbolo> fita, int cabecote)
    {
        historico.Add(new PassoMT
        {
            Numero = passos,
            Estado = estado.Nome, // Extrai o nome da classe Estado para o visual
            Fita = FormatarFita(fita, cabecote),
            PosicaoCabecote = cabecote
        });
    }

    private string ExtrairResultadoTransdutor(Dictionary<int, Simbolo> fita)
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

    private string FormatarFita(Dictionary<int, Simbolo> fita, int cabecote)
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