using Spectre.Console;
using Parte0.Screens;
using Parte1;
using Parte2;
using Parte3;


while (true)
{
    AnsiConsole.Clear();

    AnsiConsole.Write(
        new FigletText("FTC 2026/1")
            .Centered()
            .Color(Color.Cyan1));

    AnsiConsole.WriteLine();

    AnsiConsole.Write(
        new Panel(
            "[green]Felipe Vandevelde[/]\n" +
            "[green]Ramonys Santos[/]\n" +
            "[green]Artur Piumbini[/]")
        .Header("[white]ALUNOS[/]")
        .Border(BoxBorder.Double)
        .Expand());

    var opcao = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[yellow]Escolha uma opção:[/]")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Teste",
                "AFD",
                "APD",
                "MT",
                "Sair"
            }));

    switch (opcao)
    {
        case "Teste":
            TesteScreen.Show();
            break;

        case "AFD":
            AFD.Executar();
            AFDScreen.Show();
            break;

        case "APD":
            APD.Executar();
            APDScreen.Show();
            break;

        case "MT":
            MT.Executar();
            MTScreen.Show();
            break;

        case "Sair":
            AnsiConsole.MarkupLine(
                "[grey]Encerrando...[/]");
            return;
    }
}