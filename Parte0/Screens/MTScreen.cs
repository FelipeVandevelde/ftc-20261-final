using Spectre.Console;
using Parte3;

namespace Parte0.Screens;

public class MTScreen
{
    public static void Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
                new FigletText("Máquina de Turing")
                    .Centered()
                    .Color(Color.Cyan1));

        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new Panel(
                "[green]Artur Ribeiro Piumbini - 72400609[/]")
            .Header("[white]RESPONSAVEL[/]")
            .Border(BoxBorder.Double)
            .Expand());

        string entrada = "";

        // Menu de opções para escolher qual MT executar
        var opcao = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Escolha uma opção:[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Notação formal das MT",
                    "MT aⁿbⁿcⁿ",
                    "MT n + 1",
                    "Voltar"
                }));

        // Executa a opção escolhida
        switch (opcao)
        {
            // Opção 1: Exibe a notação formal das MT
            case "Notação formal das MT":
                AnsiConsole.MarkupLine("[yellow]Notação formal das Máquinas de Turing[/]");
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("[blue]EstadoAtual[/]");
                table.AddColumn("[yellow]SimboloLido[/]");
                table.AddColumn("[blue]NovoEstado[/]");
                table.AddColumn("[yellow]NovoSimbolo[/]");
                table.AddColumn("[grey]Direcao[/]");

                AnsiConsole.MarkupLine("[grey]Uma Máquina de Turing consiste em uma 7-tupla (Q, Σ, Γ, δ, q0, qaccept, qreject):[/]");
                AnsiConsole.MarkupLine("[grey] Maquina reconhecedora da linguagem L4 = {aⁿbⁿcⁿ | n ≥ 1}[/]");
                AnsiConsole.MarkupLine("[grey] Q = {1,2,3,4,5,6,7,8,rej}[/]");
                AnsiConsole.MarkupLine("[grey] Σ = {a,b,c}[/]");
                AnsiConsole.MarkupLine("[grey] Γ = {a,b,c,X,Y,_,<}[/]");
                AnsiConsole.MarkupLine("[grey] q0 = 1[/]");
                AnsiConsole.MarkupLine("[grey] qaccept = 8[/]");
                AnsiConsole.MarkupLine("[grey] qreject = rej[/]");
                AnsiConsole.MarkupLine("[grey] δ:[/]");
                
                table.AddRow("1", "a", "2", "Y", "R");
                table.AddRow("2", "a", "2", "a", "R");
                table.AddRow("2", "X", "2", "X", "R");
                table.AddRow("2", "b", "3", "X", "R");
                table.AddRow("3", "b", "3", "b", "R");
                table.AddRow("3", "X", "3", "X", "R");
                table.AddRow("3", "c", "4", "X", "R");
                table.AddRow("4", "c", "5", "c", "L");
                table.AddRow("4", "_", "7", "_", "L");
                table.AddRow("5", "a", "5", "a", "L");
                table.AddRow("5", "b", "5", "b", "L");
                table.AddRow("5", "X", "5", "X", "L");
                table.AddRow("5", "Y", "6", "Y", "R");
                table.AddRow("6", "X", "6", "X", "R");
                table.AddRow("6", "a", "2", "X", "R");
                table.AddRow("7", "X", "7", "X", "L");
                table.AddRow("7", "Y", "8", "Y", "R");
                AnsiConsole.Write(table);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey] Maquina transdutora da função f(n) = n + 1[/]");
                AnsiConsole.MarkupLine("[grey] Q = {0,1,rej}[/]");
                AnsiConsole.MarkupLine("[grey] Σ = {0,1}[/]");
                AnsiConsole.MarkupLine("[grey] Γ = {0,1,_,<}[/]");
                AnsiConsole.MarkupLine("[grey] q0 = 0[/]");
                AnsiConsole.MarkupLine("[grey] qaccept = 1[/]");
                AnsiConsole.MarkupLine("[grey] qreject = rej[/]");
                AnsiConsole.MarkupLine("[grey] δ:[/]");
                
                table.Rows.Clear();
                table.AddRow("0", "1", "0", "1", "R");
                table.AddRow("0", "_", "1", "1", "L");
                table.AddRow("0", "0", "1", "1", "L");
                AnsiConsole.Write(table);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;
            
            // Opção 2: Executa a MT que reconhece a linguagem aⁿbⁿcⁿ
            case "MT aⁿbⁿcⁿ":
                // Configura a MT com os estados, alfabeto, transições e outros parâmetros necessários
                var mtr = new MT();
                mtr.LimitePassos = int.Parse(AnsiConsole.Ask<string>("Digite o [yellow]limite de passos[/]: ")); // Limite de passos configuravelpara evitar loops infinitos
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): "); // Entrada da palavra teste do usuário

                mtr.MTReconhecedora = true; // Define que é uma máquina reconhecedora ou transdutora (Definido para facilitar a exibição do resultado final formatado, sem precisar criar uma classe separada para MT transdutora)
                mtr.AdicionarEstados("1", "2", "3", "4", "5", "6", "7", "8", "rej"); // Estados da MT
                mtr.AdicionarAlfabetoEntrada('a', 'b', 'c'); // Alfabeto de entrada
                mtr.AdicionarAlfabetoFita('a', 'b', 'c', 'X', 'Y', '_'); // Alfabeto da fita (inclui símbolos de marcação e branco)
                mtr.EstadoInicial = mtr.ObterEstado("1"); // Estado inicial
                mtr.EstadoAceitacao = mtr.ObterEstado("8"); // Estado de aceitação
                mtr.EstadoRejeicao = mtr.ObterEstado("rej"); // Estado de rejeição
                // Transições da MT no formato (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mtr.AdicionarTransicao("1", 'a', "2", 'Y', 'R');
                mtr.AdicionarTransicao("2", 'a', "2", 'a', 'R');
                mtr.AdicionarTransicao("2", 'X', "2", 'X', 'R');
                mtr.AdicionarTransicao("2", 'b', "3", 'X', 'R');
                mtr.AdicionarTransicao("3", 'b', "3", 'b', 'R');
                mtr.AdicionarTransicao("3", 'X', "3", 'X', 'R');
                mtr.AdicionarTransicao("3", 'c', "4", 'X', 'R');
                mtr.AdicionarTransicao("4", 'c', "5", 'c', 'L');
                mtr.AdicionarTransicao("4", '_', "7", '_', 'L');
                mtr.AdicionarTransicao("5", 'a', "5", 'a', 'L');
                mtr.AdicionarTransicao("5", 'b', "5", 'b', 'L');
                mtr.AdicionarTransicao("5", 'X', "5", 'X', 'L');
                mtr.AdicionarTransicao("5", 'Y', "6", 'Y', 'R');
                mtr.AdicionarTransicao("6", 'X', "6", 'X', 'R');
                mtr.AdicionarTransicao("6", 'a', "2", 'X', 'R');
                mtr.AdicionarTransicao("7", 'X', "7", 'X', 'L');
                mtr.AdicionarTransicao("7", 'Y', "8", 'Y', 'R');

                Execute(entrada, mtr); // Chamada do método para executar a MT e exibir o resultado formatado

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            // Opção 3: Executa a MT que gera a função f(n) = n + 1
            case "MT n + 1":
                // Configura a MT com os estados, alfabeto, transições e outros parâmetros necessários
                var mtt = new MT();
                mtt.LimitePassos = int.Parse(AnsiConsole.Ask<string>("Digite o [yellow]limite de passos[/]: ")); // Limite de passos configuravel para evitar loops infinitos
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): "); // Entrada da palavra teste do usuário

                mtt.MTReconhecedora = false; // Define que é uma máquina reconhecedora ou transdutora (Definido para facilitar a exibição do resultado final formatado, sem precisar criar uma classe
                mtt.AdicionarEstados("0", "1", "rej"); // Estados da MT
                mtt.AdicionarAlfabetoEntrada('0', '1'); // Alfabeto de entrada
                mtt.AdicionarAlfabetoFita('0', '1', '_'); // Alfabeto da fita (inclui branco)
                mtt.EstadoInicial = mtt.ObterEstado("0"); // Estado inicial
                mtt.EstadoAceitacao = mtt.ObterEstado("1"); // Estado de aceitação
                mtt.EstadoRejeicao = mtt.ObterEstado("rej") ; // Estado de rejeição
                // Transições da MT no formato (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mtt.AdicionarTransicao("0", '1', "0", '1', 'R');
                mtt.AdicionarTransicao("0", '_', "1", '1', 'R');
                mtt.AdicionarTransicao("0", '0', "1", '1', 'R');

                Execute(entrada, mtt); // Chamada do método para executar a MT e exibir o resultado formatado

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "Voltar":
                return;
        }
    }

    // Método para executar a MT e exibir o resultado formatado
    public static void Execute(string entrada, MT mt)
    {
        var resultado = mt.Executar(entrada);

        string strEstados = string.Join(", ", mt.Estados.Select(e => e.Nome));
        string strAlfabetoEntrada = string.Join(", ", mt.AlfabetoEntrada.Select(s => s.Valor));
        string strAlfabetoFita = string.Join(", ", mt.AlfabetoFita.Select(s => s.Valor));
        string strInicial = mt.EstadoInicial?.Nome ?? "Indefinido";
        string strAceitacao = mt.EstadoAceitacao?.Nome ?? "Indefinido";
        string strRejeicao = mt.EstadoRejeicao?.Nome ?? "Indefinido";

        AnsiConsole.MarkupLine($"[grey] E = {{{strEstados}}}[/]");
        AnsiConsole.MarkupLine($"[grey] Σ = {{{strAlfabetoEntrada}}}[/]");
        AnsiConsole.MarkupLine($"[grey] Γ = {{{strAlfabetoFita}}}[/]");
        AnsiConsole.MarkupLine($"[grey] Estado Inicial = {strInicial}[/]");
        AnsiConsole.MarkupLine($"[grey] Estado de Aceitação = {strAceitacao}[/]");
        AnsiConsole.MarkupLine($"[grey] Estado de Rejeição = {strRejeicao}[/]");
        AnsiConsole.Write("");

        var table = new Table(); // Tabela para exibir o histórico de execução da MT
        table.Border(TableBorder.Rounded);
        table.AddColumn("[blue]Passo[/]");
        table.AddColumn("[yellow]Estado[/]");
        table.AddColumn("Fita");
        table.AddColumn("[grey]Pos. Cabeçote[/]");

        foreach (var passo in resultado.historico) // Preenche a tabela com o histórico de execução da MT
        {
            table.AddRow(
                passo.Numero.ToString(),
                $"[yellow]{passo.Estado}[/]",
                Markup.Escape(passo.Fita),
                $"[grey]{passo.PosicaoCabecote}[/]"
            );
        }

        AnsiConsole.Write(table); // Exibe a tabela com o histórico de execução da MT

        // Exibe o resultado final formatado
        var tipo = mt.MTReconhecedora ? "Reconhecedora" : "Geradora";
        var corPainel = resultado.aceito ? Color.Green : Color.Red;
        var textoPainel = resultado.aceito ? "[green]ACEITO[/]" : "[red]REJEITADO[/]";

        if (tipo == "Geradora")
            textoPainel = resultado.aceito ? "[green]RESULTADO GERADO[/]" : "[red]REJEITADO[/]";

        AnsiConsole.Write(
            new Panel($"{textoPainel}\n{resultado.motivo}")
                .Header("Resultado Final")
                .BorderColor(corPainel)
                .Border(BoxBorder.Double));
    }
}