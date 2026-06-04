using Spectre.Console;
using Parte0.Utils;
using Parte1;

namespace Parte0.Screens;

public class AFDScreen
{
    public static void Show()
    {
        AFD afd = new AFD();
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("Automato Finito Determinístico")
                    .Centered()
                    .Color(Color.Cyan1));

            AnsiConsole.WriteLine();

            AnsiConsole.Write(
                new Panel(
                    "[green]Felipe Vandevelde - 72301201[/]")
                .Header("[white]RESPONSAVEL[/]")
                .Border(BoxBorder.Double)
                .Expand());

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Escolha uma opção:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Ver Diagrama",
                        "Validar Palavra",
                        "Selecionar um arquivo para validação",
                        "Voltar"
                    }));

            switch (opcao)
            {
                case "Ver Diagrama":
                    ExibirDiagrama(afd);
                    break;
                    
                case "Validar Palavra":
                    validarPalavra(afd);
                    break;

                case "Selecionar um arquivo para validação":
                    selecionarArquivo(afd);
                    break;

                case "Voltar":
                    AnsiConsole.MarkupLine(
                        "[grey]Encerrando...[/]");
                    return;
            }
        }
    }

    public static void ExibirDiagrama(AFD afd)
    {
        var afdInfo = afd.ObterInformacoes();
        var info = new Table()
            .Border(TableBorder.Rounded)
            .Title("[yellow]Informações do AFD[/]");

        info.AddColumn("Propriedade");
        info.AddColumn("Valor");

        info.AddRow("Estados (Q)", string.Join(", ", afdInfo.Q));
        info.AddRow("Alfabeto (Σ)", string.Join(", ", afdInfo.Σ));
        info.AddRow("Estado Inicial (I)", afdInfo.I);
        info.AddRow("Estado Final (F)", afdInfo.F);

        var tabelaTransicoes = new Table()
            .Border(TableBorder.Heavy)
            .Title("[green]Função de Transição (δ)[/]");

        tabelaTransicoes.AddColumn("[blue]Estado Atual[/]");
        tabelaTransicoes.AddColumn("[blue]Símbolo[/]");
        tabelaTransicoes.AddColumn("[blue]Próximo Estado[/]");

        foreach (var t in afd.ObterTransicoes())
        {
            tabelaTransicoes.AddRow(
                $"[yellow]{t.EstadoAtual}[/]",
                $"[white]{t.Simbolo}[/]",
                $"[cyan]{t.ProximoEstado}[/]"
            );
        }

        var colunas = new Columns(
            info,
            tabelaTransicoes
        );

        AnsiConsole.Write(colunas);
        end();
    }

    private static void validarPalavra(AFD afd)
    {
        string palavra = AnsiConsole.Ask<string>(
            "[yellow]Digite a palavra para validação:[/]");

        string[] transicoes = afd.ObterTransicoes(palavra);
        bool resultado = afd.Aceitar(transicoes);

        if (resultado)
        {
            AnsiConsole.MarkupLine(
                $"[green]A palavra '{palavra}' é aceita pelo AFD![/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[red]A palavra '{palavra}' é rejeitada pelo AFD.[/]");
        }

        end();
    }
    
    private static void selecionarArquivo(AFD afd)
    {
        string? arquivo = FilePicker.Open(Path.Combine(Directory.GetCurrentDirectory(), "exemplos"), "*.txt");

        if (arquivo == null)
        {
            AnsiConsole.MarkupLine("[red]Nenhum arquivo selecionado[/]");
            return;
        }

        AnsiConsole.MarkupLine(
            $"[green]Arquivo:[/] {arquivo}");            

        string conteudo = File.ReadAllText(arquivo);
        ApresentarTabelaDeAceitacao(conteudo, afd);
        end();
    }

    public static void ApresentarTabelaDeAceitacao(string conteudo, AFD afd)
    {
        string[] linhas = conteudo
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        var tabela = new Table()
            .Border(TableBorder.Rounded)
            .Title("[yellow]Tabela de Aceitação[/]");

        tabela.AddColumn("[yellow]Linha[/]");
        tabela.AddColumn("[cyan]Resultado[/]");

        for (int i = 0; i < linhas.Length; i++)
        {
            tabela.AddRow(
                linhas[i],
                afd.Aceitar(linhas[i]) ? "[green]Aceita[/]" : "[red]Rejeita[/]"
            );
        }

        AnsiConsole.Write(tabela);
    }

    private static void end()
    {
        AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla...[/]");
        Console.ReadKey();
    }
}