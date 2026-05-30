using System.Collections.Generic;
namespace Parte1;

public class AFD
{
    private string[] Q;
    private char[] Σ;
    private Dictionary<(string, char), string> δ = new Dictionary<(string, char), string>();
    private string I;
    private string F;

    public AFD()
    {
        Q = new string[] { "q-1", "q0", "q1", "q2" };
        Σ = new char[] { 'a', 'b' };
        δ[("q0", 'a')] = "q1";
        δ[("q0", 'b')] = "q0";
        δ[("q1", 'a')] = "q1";
        δ[("q1", 'b')] = "q2";
        δ[("q2", 'a')] = "q1";
        δ[("q2", 'b')] = "q0";
        δ[("*", '*')] = "q-1";
        I = "q0";
        F = "q2";
    }

    /*public AFD(string[] q, char[] σ, string[] t, string i, string f)
    {
        Q = q;
        Σ = σ;
        δ = t;
        I = i;
        F = f;
    }*/

    public record AFDInfo(
        string[] Q,
        char[] Σ,
        string I,
        string F
    );

    public AFDInfo ObterInformacoes()
    {
        return new AFDInfo(Q, Σ, I, F);
    }

    public List<(string EstadoAtual, char Simbolo, string ProximoEstado)> ObterTransicoes()
    {
        var lista = new List<(string, char, string)>();

        foreach (var transicao in δ)
        {
            lista.Add((
                transicao.Key.Item1,
                transicao.Key.Item2,
                transicao.Value
            ));
        }

        return lista;
    }

    public bool Aceitar(string cadeia)
    {
        string estadoAceitacao = F;

        string[] transicoes = ObterTransicoes(cadeia);
        return transicoes[^1] == estadoAceitacao;
    }

    public bool Aceitar(string[] transicoes )
    {
        string estadoAceitacao = F;
        return transicoes[^1] == estadoAceitacao;
    }

    public string[] ObterTransicoes(string cadeia)
    {
        string estadoAtual = I;
        string[] transicoes = [estadoAtual];

        foreach (char simbolo in cadeia)
        {
            if (!δ.ContainsKey((estadoAtual, simbolo)))
                return transicoes.Append(δ[("", 'ε')]).ToArray();
            transicoes = transicoes.Append(δ[(estadoAtual, simbolo)]).ToArray();
            estadoAtual = δ[(estadoAtual, simbolo)];
        }
        return transicoes;
    }
}