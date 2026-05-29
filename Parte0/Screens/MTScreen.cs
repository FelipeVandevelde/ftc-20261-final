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
                "[green]Artur Ribeiro Piumbini[/]")
            .Header("[white]RESPONSAVEL[/]")
            .Border(BoxBorder.Double)
            .Expand());

        string entrada = "";

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

        switch (opcao)
        {
            case "Notação formal das MT":
                AnsiConsole.MarkupLine("[yellow]Notação formal das Máquinas de Turing[/]");
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("[blue]EstadoAtual[/]");
                table.AddColumn("[yellow]SimboloLido[/]");
                table.AddColumn("[blue]NovoEstado[/]");
                table.AddColumn("[yellow]NovoSimbolo[/]");
                table.AddColumn("[grey]Direcao[/]");

                AnsiConsole.MarkupLine("[grey]Uma Máquina de Turing consiste em uma sêxtupla (E,Σ,Γ,δ,i,F):[/]");
                AnsiConsole.MarkupLine("[grey] Maquina reconhecedora da linguagem L4 = {aⁿbⁿcⁿ | n ≥ 1}[/]");
                AnsiConsole.MarkupLine("[grey] E = {1,2,3,4,5,6,7,8}[/]");
                AnsiConsole.MarkupLine("[grey] Σ = {a,b,c}[/]");
                AnsiConsole.MarkupLine("[grey] Γ = {a,b,c,X,_,<}[/]");
                AnsiConsole.MarkupLine("[grey] i = 1[/]");
                AnsiConsole.MarkupLine("[grey] F = {8}[/]");
                AnsiConsole.MarkupLine("[grey] δ:[/]");
                table.AddRow("1", "a", "2", "X", "D");
                table.AddRow("1", "_", "8", "_", "E");
                table.AddRow("2", "a", "2", "a", "D");
                table.AddRow("2", "X", "2", "X", "D");
                table.AddRow("2", "b", "3", "X", "D");
                table.AddRow("3", "b", "3", "b", "D");
                table.AddRow("3", "X", "3", "X", "D");
                table.AddRow("3", "c", "4", "X", "D");
                table.AddRow("4", "c", "5", "c", "E");
                table.AddRow("4", "_", "7", "_", "E");
                table.AddRow("5", "a", "5", "a", "E");
                table.AddRow("5", "b", "5", "b", "E");
                table.AddRow("5", "X", "5", "X", "E");
                table.AddRow("5", "<", "6", "<", "D");
                table.AddRow("6", "X", "6", "X", "D");
                table.AddRow("6", "a", "2", "X", "D");
                table.AddRow("7", "X", "7", "X", "E");
                table.AddRow("7", "<", "8", "<", "D");
                table.AddRow("8", "<", "8", "<", "D");
                AnsiConsole.Write(table);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey] Maquina transdutora da função f(n) = n + 1[/]");
                AnsiConsole.MarkupLine("[grey] E = {0,1,2}[/]");
                AnsiConsole.MarkupLine("[grey] Σ = {0,1}[/]");
                AnsiConsole.MarkupLine("[grey] Γ = {0,1,_,<}[/]");
                AnsiConsole.MarkupLine("[grey] i = 0[/]");
                AnsiConsole.MarkupLine("[grey] F = {}[/]");
                AnsiConsole.MarkupLine("[grey] δ:[/]");
                table.Rows.Clear();
                table.AddRow("0", "_", "1", "1", "E");
                table.AddRow("0", "0", "1", "1", "E");
                table.AddRow("0", "1", "0", "1", "D");
                table.AddRow("1", "1", "1", "1", "E");
                table.AddRow("1", "<", "2", "<", "D");
                AnsiConsole.Write(table);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "MT aⁿbⁿcⁿ":
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): ");
                var mtr = new MT();
                mtr.EstadoInicial = "1";
                mtr.EstadosAceitacao.Add("8");
                // (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mtr.AdicionarTransicao("1", 'a', "2", 'X', 'D');
                mtr.AdicionarTransicao("1", '_', "8", '_', 'E');
                mtr.AdicionarTransicao("2", 'a', "2", 'a', 'D');
                mtr.AdicionarTransicao("2", 'X', "2", 'X', 'D');
                mtr.AdicionarTransicao("2", 'b', "3", 'X', 'D');
                mtr.AdicionarTransicao("3", 'b', "3", 'b', 'D');
                mtr.AdicionarTransicao("3", 'X', "3", 'X', 'D');
                mtr.AdicionarTransicao("3", 'c', "4", 'X', 'D');
                mtr.AdicionarTransicao("4", 'c', "5", 'c', 'E');
                mtr.AdicionarTransicao("4", '_', "7", '_', 'E');
                mtr.AdicionarTransicao("5", 'a', "5", 'a', 'E');
                mtr.AdicionarTransicao("5", 'b', "5", 'b', 'E');
                mtr.AdicionarTransicao("5", 'X', "5", 'X', 'E');
                mtr.AdicionarTransicao("5", '<', "6", '<', 'D');
                mtr.AdicionarTransicao("6", 'X', "6", 'X', 'D');
                mtr.AdicionarTransicao("6", 'a', "2", 'X', 'D');
                mtr.AdicionarTransicao("7", 'X', "7", 'X', 'E');
                mtr.AdicionarTransicao("7", '<', "8", '<', 'D');
                mtr.AdicionarTransicao("8", '<', "8", '<', 'D');

                // Adicionar o simbolo de início da fita
                Execute('<' + entrada, mtr);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "MT n + 1":
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): ");
                var mtt = new MT();
                mtt.EstadoInicial = "0";
                mtt.MTReconhecedor = false;
                // (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mtt.AdicionarTransicao("0", '_', "1", '1', 'E');
                mtt.AdicionarTransicao("0", '0', "1", '1', 'E');
                mtt.AdicionarTransicao("0", '1', "0", '1', 'D');
                mtt.AdicionarTransicao("1", '1', "1", '1', 'E');
                mtt.AdicionarTransicao("1", '<', "2", '<', 'D');

                // Adicionar o simbolo de início da fita
                Execute('<' + entrada, mtt);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "Voltar":
                return;
        }
    }

    public static void Execute(string entrada, MT mt)
        {
            var resultado = mt.Executar(entrada);

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[blue]Passo[/]");
            table.AddColumn("[yellow]Estado[/]");
            table.AddColumn("Fita");
            table.AddColumn("[grey]Pos. Cabeçote[/]");

            foreach (var passo in resultado.historico)
            {
                table.AddRow(
                    passo.Numero.ToString(),
                    $"[yellow]{passo.Estado}[/]",
                    Markup.Escape(passo.Fita),
                    $"[grey]{passo.PosicaoCabecote}[/]"
                );
            }

            AnsiConsole.Write(table);

            // Exibe o resultado final formatado
            var reconhecedor = mt.MTReconhecedor ? "Reconhecedora" : "Geradora";
            var corPainel = resultado.aceito ? Color.Green : Color.Red;
            var textoPainel = resultado.aceito ? "[green]ACEITO[/]" : "[red]REJEITADO[/]";

            if(reconhecedor == "Geradora")
                textoPainel = "[green]RESULTADO GERADO[/]";

            AnsiConsole.Write(
                new Panel($"{textoPainel}\n{resultado.motivo}")
                    .Header("Resultado Final")
                    .BorderColor(corPainel)
                    .Border(BoxBorder.Double));
        }
}
