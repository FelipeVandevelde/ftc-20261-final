using Spectre.Console;
using Parte0.Utils;
using Parte2;

namespace Parte0.Screens;

public class APDScreen
{
   public static void Show()
    {
        APD apd = new APD();
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("Automato De Pilha Não Determinístico")
                    .Centered()
                    .Color(Color.Cyan1));

            AnsiConsole.WriteLine();

            AnsiConsole.Write(
                new Panel(
                    "[green]Ramonys Santos - 72301104[/]")
                .Header("[white]RESPONSAVEL[/]")
                .Border(BoxBorder.Double)
                .Expand());

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Escolha uma opção:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Validar Palavra",
                        "Selecionar um arquivo para validação",
                        "Voltar"
                    }));

            switch (opcao)
            {
                case "Validar Palavra":
                    validarPalavra(apd);
                    break;

                case "Selecionar um arquivo para validação":
                    selecionarArquivo(apd);
                    break;

                case "Voltar":
                    AnsiConsole.MarkupLine(
                        "[grey]Encerrando...[/]");
                    return;
            }
        }
    }

    private static void validarPalavra(APD apd)
    {
        string linguagem = SelecionarLinguagem();
        if (linguagem == "0")
        {
            return;
        }

        string palavra = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Digite a palavra para validação:[/]")
            .AllowEmpty());

        
        bool resultado = apd.Executar(palavra, linguagem);

        if (resultado)
        {
            AnsiConsole.MarkupLine(
                $"[green]A palavra '{palavra}' é aceita pelo APND![/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[red]A palavra '{palavra}' é rejeitada pelo APND.[/]");
        }

        end();
    }

    private static void selecionarArquivo(APD apd)
    {
        string linguagem = SelecionarLinguagem();
        if (linguagem == "0")
        {
            return;
        }
        string? arquivo = FilePicker.Open(Path.Combine(Directory.GetCurrentDirectory(), "exemplos"), "*.txt");

        if (arquivo == null)
        {
            AnsiConsole.MarkupLine("[red]Nenhum arquivo selecionado[/]");
            return;
        }

        AnsiConsole.MarkupLine(
            $"[green]Arquivo:[/] {arquivo}");            

        string conteudo = File.ReadAllText(arquivo);
        ApresentarTabelaDeAceitacao(conteudo, apd, linguagem);
        end();
    }

    public static void ApresentarTabelaDeAceitacao(string conteudo, APD apd, string linguagem)
    {
        AnsiConsole.Clear();
        string[] linhas = conteudo
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);


        var tabela = new Table()
            .Border(TableBorder.Rounded)
            .Title("[yellow]Tabela de Aceitação[/]");

        tabela.AddColumn("[yellow]Linha[/]");
        tabela.AddColumn("[cyan]Resultado[/]");

        for (int i = 0; i < linhas.Length; i++)
        {
            AnsiConsole.MarkupLine(
                $"[green]'{linhas[i]}':[/]");
            tabela.AddRow(
                linhas[i],
                apd.Executar(linhas[i], linguagem) ? "[green]Aceita[/]" : "[red]Rejeita[/]"
            );
            AnsiConsole.MarkupLine(
                "\n\n");
        }

        AnsiConsole.Write(tabela);
    }

    private static string SelecionarLinguagem()
    {
        string linguagem = "";
        var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Escolha uma linguagem:\nL₂ = { aⁿbⁿ | n ≥ 1 }\nL₃ = { w ∈ {a,b}* | w = wᴿ, |w| >= 1 }[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "L₂",
                        "L₃",
                        "Voltar"
                    }));

        switch (opcao)
        {
            case "L₂":
                linguagem = "1";
                break;

            case "L₃":
                linguagem = "2";
                break;

            case "Voltar":
                linguagem = "0";
                break;
        }
        return linguagem;
    }

    private static void end()
    {
        AnsiConsole.MarkupLine("[grey]Pressione qualquer tecla...[/]");
        Console.ReadKey();
    }
}