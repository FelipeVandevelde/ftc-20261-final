using Spectre.Console;
using Parte3;

namespace Parte0.Screens;

public class MTScreen
{
    public static void Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("MT")
                .Color(Color.Green));

        string entrada = "";

        var opcao = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Escolha uma opção:[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "MT a^nb^nc^n",
                    "MT n+1",
                    "Sair"
                }));

        switch (opcao)
        {
            case "MT a^nb^nc^n":
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): ");
                var mt = new MT();
                mt.EstadoInicial = "1";
                mt.EstadosAceitacao.Add("8");
                // (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mt.AdicionarTransicao("1", 'a', "2", 'X', 'D');
                mt.AdicionarTransicao("1", '_', "8", '_', 'E');
                mt.AdicionarTransicao("2", 'a', "2", 'a', 'D');
                mt.AdicionarTransicao("2", 'X', "2", 'X', 'D');
                mt.AdicionarTransicao("2", 'b', "3", 'X', 'D');
                mt.AdicionarTransicao("3", 'b', "3", 'b', 'D');
                mt.AdicionarTransicao("3", 'X', "3", 'X', 'D');
                mt.AdicionarTransicao("3", 'c', "4", 'X', 'D');
                mt.AdicionarTransicao("4", 'c', "5", 'c', 'E');
                mt.AdicionarTransicao("4", '_', "7", '_', 'E');
                mt.AdicionarTransicao("5", 'a', "5", 'a', 'E');
                mt.AdicionarTransicao("5", 'b', "5", 'b', 'E');
                mt.AdicionarTransicao("5", 'X', "5", 'X', 'E');
                mt.AdicionarTransicao("5", '<', "6", '<', 'D');
                mt.AdicionarTransicao("6", 'X', "6", 'X', 'D');
                mt.AdicionarTransicao("6", 'a', "2", 'X', 'D');
                mt.AdicionarTransicao("7", 'X', "7", 'X', 'E');
                mt.AdicionarTransicao("7", '<', "8", '<', 'D');
                mt.AdicionarTransicao("8", '<', "8", '<', 'D');

                Execute(entrada, mt);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "MT n+1":
                entrada = AnsiConsole.Ask<string>("Digite a [yellow]palavra[/] de entrada (use _ para branco): ");
                var mti = new MT();
                mti.EstadoInicial = "0";
                mti.MTReconhecedor = false;
                // (EstadoAtual, SimboloLido, NovoEstado, NovoSimbolo, Direcao)
                mti.AdicionarTransicao("0", '_', "1", '1', 'E');
                mti.AdicionarTransicao("0", '0', "1", '1', 'E');
                mti.AdicionarTransicao("0", '1', "0", '1', 'D');
                mti.AdicionarTransicao("1", '1', "1", '1', 'E');
                mti.AdicionarTransicao("1", '<', "2", '<', 'D');

                Execute(entrada, mti);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla para voltar ao menu...[/]");
                Console.ReadKey();
                MTScreen.Show();
                break;

            case "Sair":
                AnsiConsole.MarkupLine(
                    "[grey]Encerrando...[/]");
                return;
        }
    }

    public static void Execute(string entrada, MT mt)
        {
            AnsiConsole.MarkupLine("\n[grey]Iniciando simulação...[/]\n");

                // Adiciona o símbolo de início '<' no começo da entrada para demarcar o simbolo inicial da fita
                var resultado = mt.Executar('<' + entrada);

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
