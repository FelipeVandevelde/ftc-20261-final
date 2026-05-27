// ================================================================
// FilePicker.cs
// ================================================================
//
// OBJETIVO
// ---------------------------------------------------------------
// Este arquivo cria um seletor de arquivos em modo terminal (TUI)
// utilizando Spectre.Console.
//
// O usuário consegue:
//
// - Navegar entre pastas
// - Voltar diretórios
// - Selecionar arquivos
// - Filtrar extensões
//
// Tudo usando teclado no terminal.
//
// FUNCIONA EM:
// ---------------------------------------------------------------
// ✅ Windows
// ✅ Linux
// ✅ macOS
// ✅ WSL
// ✅ SSH
// ✅ Docker com terminal
//
// DEPENDÊNCIAS:
// ---------------------------------------------------------------
// dotnet add package Spectre.Console
//
// ================================================================

using Spectre.Console;

namespace Parte0.Utils;

/// <summary>
/// Classe responsável por exibir um explorador de arquivos
/// diretamente no terminal utilizando Spectre.Console.
///
/// A ideia é simular um pequeno "Windows Explorer"
/// porém totalmente dentro do terminal.
///
/// Exemplo visual:
///
/// 📁 ..
/// 📁 Downloads
/// 📁 Projetos
/// 📄 arquivo.txt
/// 📄 config.json
///
/// O usuário navega usando:
/// - Setas do teclado
/// - Enter
/// </summary>
public static class FilePicker
{
    /// <summary>
    /// Abre o explorador de arquivos TUI.
    ///
    /// Retorna:
    /// - caminho completo do arquivo escolhido
    /// - null caso o usuário cancele
    ///
    /// EXEMPLO:
    ///
    /// string? arquivo = FilePicker.Open();
    ///
    /// if (arquivo != null)
    /// {
    ///     string texto = File.ReadAllText(arquivo);
    /// }
    /// </summary>
    /// <param name="initialDirectory">
    /// Diretório inicial.
    ///
    /// Se não for informado:
    /// usa o diretório atual da aplicação.
    /// </param>
    /// <param name="searchPattern">
    /// Filtro de arquivos.
    ///
    /// Exemplos:
    /// "*.txt"
    /// "*.json"
    /// "*.*"
    ///
    /// Padrão:
    /// todos os arquivos.
    /// </param>
    /// <returns>
    /// Caminho completo do arquivo
    /// ou null.
    /// </returns>
    public static string? Open(
        string? initialDirectory = null,
        string searchPattern = "*.*")
    {
        // ========================================================
        // DEFINE O DIRETÓRIO INICIAL
        // ========================================================
        //
        // Se o usuário não passar uma pasta:
        // usa a pasta atual da aplicação.
        //
        // Exemplo:
        // bin/Debug/net9.0/
        //
        string currentDirectory =
            initialDirectory
            ?? Directory.GetCurrentDirectory();

        // ========================================================
        // LOOP PRINCIPAL
        // ========================================================
        //
        // O explorador funciona em loop.
        //
        // A cada iteração:
        //
        // 1. Lista pastas
        // 2. Lista arquivos
        // 3. Exibe menu
        // 4. Usuário escolhe algo
        // 5. Navega ou retorna arquivo
        //
        while (true)
        {
            try
            {
                // =================================================
                // LISTA QUE VAI CONTER TODOS OS ITENS
                // EXIBIDOS NO MENU
                // =================================================
                List<string> items = [];

                // =================================================
                // OPÇÃO DE VOLTAR DIRETÓRIO
                // =================================================
                //
                // Só adiciona se existir pasta pai.
                //
                // Exemplo:
                //
                // C:/Users/Felipe/Documents
                //
                // Pasta pai:
                //
                // C:/Users/Felipe
                //
                if (Directory.GetParent(currentDirectory) != null)
                {
                    items.Add("📁 ..");
                }

                // =================================================
                // OBTÉM TODAS AS PASTAS
                // =================================================
                //
                // Directory.GetDirectories retorna:
                //
                // C:/Teste/Pasta1
                // C:/Teste/Pasta2
                //
                // Depois usamos:
                //
                // Path.GetFileName
                //
                // Para pegar apenas:
                //
                // Pasta1
                // Pasta2
                //
                var directories =
                    Directory.GetDirectories(currentDirectory)
                        .OrderBy(d => d)
                        .Select(d =>
                            "📁 " + Path.GetFileName(d));

                items.AddRange(directories);

                // =================================================
                // OBTÉM TODOS OS ARQUIVOS
                // =================================================
                //
                // O searchPattern permite filtrar:
                //
                // "*.txt"
                // "*.json"
                // "*.xml"
                //
                var files =
                    Directory.GetFiles(
                            currentDirectory,
                            searchPattern)
                        .OrderBy(f => f)
                        .Select(f =>
                            "📄 " + Path.GetFileName(f));

                items.AddRange(files);

                // =================================================
                // OPÇÃO DE CANCELAR
                // =================================================
                items.Add("❌ Cancelar");

                // =================================================
                // EXIBE O MENU NO TERMINAL
                // =================================================
                //
                // SelectionPrompt:
                //
                // Cria um menu navegável usando:
                //
                // ↑ ↓ Enter
                //
                string selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(
                            $"[yellow]Diretório atual:[/]\n{currentDirectory}")
                        .PageSize(20)
                        .MoreChoicesText(
                            "[grey](Use as setas para navegar)[/]")
                        .AddChoices(items));

                // =================================================
                // CANCELAR
                // =================================================
                if (selected == "❌ Cancelar")
                {
                    return null;
                }

                // =================================================
                // VOLTAR PASTA
                // =================================================
                //
                // Se usuário escolher:
                //
                // 📁 ..
                //
                // Voltamos para a pasta pai.
                //
                if (selected == "📁 ..")
                {
                    currentDirectory =
                        Directory
                            .GetParent(currentDirectory)!
                            .FullName;

                    continue;
                }

                // =================================================
                // REMOVE O ÍCONE DO ITEM
                // =================================================
                //
                // Exemplo:
                //
                // "📁 Downloads"
                //
                // Vira:
                //
                // "Downloads"
                //
                string name = selected.Substring(3);

                // =================================================
                // MONTA CAMINHO COMPLETO
                // =================================================
                //
                // Exemplo:
                //
                // currentDirectory:
                // C:/Users/Felipe
                //
                // name:
                // Downloads
                //
                // Resultado:
                // C:/Users/Felipe/Downloads
                //
                string fullPath =
                    Path.Combine(currentDirectory, name);

                // =================================================
                // SE FOR DIRETÓRIO:
                // ENTRA NA PASTA
                // =================================================
                if (Directory.Exists(fullPath))
                {
                    currentDirectory = fullPath;
                    continue;
                }

                // =================================================
                // SE FOR ARQUIVO:
                // RETORNA O CAMINHO
                // =================================================
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            catch (Exception ex)
            {
                // =================================================
                // TRATAMENTO DE ERRO
                // =================================================
                //
                // Pode acontecer:
                //
                // - pasta sem permissão
                // - caminho inválido
                // - erro de IO
                //
                AnsiConsole.MarkupLine(
                    $"[red]Erro:[/] {ex.Message}");

                return null;
            }
        }
    }
}