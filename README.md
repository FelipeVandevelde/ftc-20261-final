# FTC 2026/1 - Projeto Final
Repositorio academico da disciplina FTC, organizado como solucao .NET modular com interface de terminal e componentes separados para AFD, APD e MT.

## 1. Integrantes
- Felipe Vandevelde - 72301201
- Ramonys Santos - 72301104
- Artur Piumbini - 72400609

## 2. Estrutura do Projeto
```text
ftc-20261-final/
|-- Sistema.slnx                   # Solucao principal (.slnx)
|-- LICENSE
|-- README.md
|-- docs/
|   `-- relatorio.md
|-- exemplos/                      # Casos de teste do pdf
|   |-- entradas_af.txt            # Entradas para o AFD
|   |-- entradas_ap1.txt           # Entradas para o APD (L2 = a^n b^n)
|   |-- entradas_ap2.txt           # Entradas para o APD (L3 = palindromos)
|-- Parte0/                        # Ponto de entrada (UI em console)
|   |-- Parte0.csproj
|   |-- Parte0.sln                 # Solucao individual do modulo
|   |-- Program.cs
|   |-- Screens/                   # Screens de UI para o console
|   |   |-- AFDScreen.cs
|   |   |-- APDScreen.cs
|   |   `-- MTScreen.cs
|   `-- Utils/                     # Utilitarios de UI
|       `-- FilePicker.cs          # Seletor de arquivos TUI (Spectre.Console)
|-- Parte1/                        # Modulo AFD
|   |-- Parte1.csproj
|   |-- Parte1.sln
|   |-- reflexiva.md
|   `-- AFD.cs
|-- Parte2/                        # Modulo APD
|   |-- Parte2.csproj
|   |-- Parte2.sln
|   |-- Estado.cs
|   |-- reflexiva.md
|   `-- APD.cs
`-- Parte3/                        # Modulo MT
    |-- Parte3.csproj
    |-- Parte3.sln
|   |-- reflexiva.md
    `-- MT.cs
```

## 3. Tecnologias
- **Linguagem:** C#
- **SDK:** .NET 8 (`net8`)
- **Interface de terminal:** Spectre.Console
- **Build e execucao:** `dotnet` CLI com projetos no formato SDK (`.csproj`)
- **Solucoes:** `.sln` (por modulo, para build individual) e `.slnx` (solucao principal que agrega todos os projetos)

## 4. Como Executar
### Pre-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0?utm_source=chatgpt.com) (ou superior) instalado

### Instalacao
```bash
git clone <URL_DO_REPOSITORIO>
cd ftc-20261-final
dotnet restore Sistema.slnx
```

### Execucao
```bash
dotnet run --project Parte0
```

## 5. Organizacao da Arquitetura
O projeto segue uma abordagem de **monolito modular**, com separacao clara entre interface e modulos de dominio:

- `Parte0`: camada de apresentacao e orquestracao do fluxo.
- `Parte1`, `Parte2`, `Parte3`: modulos de dominio (AFD, APD, MT), cada um com ponto de entrada proprio.
- `Parte0` referencia os modulos por `ProjectReference`, centralizando a integracao.

Fluxo principal:
1. O `Program.cs` exibe o menu utilizando o `Spectre.Console`.
2. O usuário escolhe uma opção (`AFD`, `APD`, `MT` ou `Sair`).
3. O módulo correspondente é executado (`AFDScreen.Show()`, `APDScreen.Show()`, `MTScreen.Show()`).
4. As telas dentro da pasta `Screens` servem como uma forma organizada de separar cada layout e são utilizadas para que o usuário possa interagir com a lógica de cada módulo.
5. A pasta `Utils` contém o `FilePicker`, um seletor de arquivos TUI que permite ao usuário navegar no sistema de arquivos e selecionar arquivos de teste diretamente pelo terminal.

Essa organizacao facilita evolucao incremental por parte (cada modulo pode crescer sem acoplamento excessivo com os demais).

## 6. Conceitos Aplicados
- **Separacao de responsabilidades (SoC):** interface concentrada em `Parte0`, com execucao dos modulos em projetos dedicados.
- **Modularidade:** cada tema (AFD/APD/MT) esta isolado em seu proprio projeto.
- **Baixo acoplamento entre modulos:** integracao feita apenas pela aplicacao principal (`Parte0`).
- **Ponto de acesso simplificado (estilo Facade):** classes de dominio expoem metodos publicos como entrada: `AFD.Aceitar()` / `AFD.ObterTransicoes()`, `APD.Executar()` e `MT.Executar()`.
- **Fluxo legivel e direto (Clean Code):** menu central com `switch` explicito e classes de tela separadas.

Estado atual observado no codigo:
- Boa base de organizacao para expansao.
- Os tres modulos (AFD, APD, MT) estao totalmente implementados com simuladores funcionais.
- As telas (`AFDScreen`, `APDScreen`, `MTScreen`) compartilham um padrao comum de header e menu, mas cada uma possui logica de interacao especifica ao seu modelo (ex.: `MTScreen` configura duas MTs distintas; `APDScreen` permite selecionar entre linguagens L2 e L3).

## 7. Dependencias
- `Spectre.Console` `0.55.2`
- `Microsoft.NET.Sdk` (infraestrutura de build dos projetos)
- `net8.0` (framework alvo)

## 8. Licenca
Este projeto esta licenciado sob a **MIT License**. Consulte o arquivo [LICENSE](LICENSE) para detalhes completos.
