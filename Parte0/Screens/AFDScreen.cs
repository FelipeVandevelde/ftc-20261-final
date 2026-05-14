using Spectre.Console;
using Parte1;

namespace Parte0.Screens;

public class AFDScreen
{
    public static void Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("AFD")
                .Color(Color.Green));

        var resultado = 0; //AFD.Executar();

        AnsiConsole.Write(
            new Panel($"[yellow]{resultado}[/]")
                .Header("Resultado")
                .Border(BoxBorder.Double));

        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla...[/]");

        Console.ReadKey();
    }
}