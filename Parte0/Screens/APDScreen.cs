using Spectre.Console;
using Parte1;

namespace Parte0.Screens;

public class APDScreen
{
    public static void Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("APD")
                .Color(Color.Green));

        var resultado = 0; //APD.Executar();

        AnsiConsole.Write(
            new Panel($"[yellow]{resultado}[/]")
                .Header("Resultado")
                .Border(BoxBorder.Double));

        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla...[/]");

        Console.ReadKey();
    }
}