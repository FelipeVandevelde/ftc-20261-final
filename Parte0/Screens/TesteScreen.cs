using Spectre.Console;

namespace Parte0.Screens;

public class TesteScreen
{
    public static void Show()
    {
        AnsiConsole.Clear();

        //
        // TÍTULO
        //
        AnsiConsole.Write(
            new FigletText("TESTE")
                .Centered()
                .Color(Color.Cyan1));

        AnsiConsole.WriteLine();

        //
        // PAINEL
        //
        AnsiConsole.Write(
            new Panel(
                "[green]Tela de teste funcionando![/]\n\n" +
                "[yellow]Spectre.Console ativo com sucesso.[/]")
            .Header("[white]STATUS[/]")
            .Border(BoxBorder.Double)
            .Expand());

        AnsiConsole.WriteLine();

        //
        // TABELA
        //
        var tabela = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green);

        tabela.AddColumn("[yellow]Item[/]");
        tabela.AddColumn("[yellow]Valor[/]");

        tabela.AddRow("Projeto", "FTC 2026/1");
        tabela.AddRow("UI", "Spectre.Console");
        tabela.AddRow("Status", "[green]OK[/]");

        AnsiConsole.Write(tabela);

        AnsiConsole.WriteLine();

        //
        // PROGRESSO
        //
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var tarefa = ctx.AddTask("[green]Executando teste[/]");

                while (!ctx.IsFinished)
                {
                    tarefa.Increment(2);
                    Thread.Sleep(40);
                }
            });

        AnsiConsole.WriteLine();

        //
        // FINAL
        //
        AnsiConsole.MarkupLine(
            "[grey]Pressione qualquer tecla para voltar...[/]");

        Console.ReadKey(true);
    }
}