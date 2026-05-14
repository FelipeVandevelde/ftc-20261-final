# FTC 2026/1 - Projeto Final
Repositorio academico da disciplina FTC, organizado como solucao .NET modular com interface de terminal e componentes separados para AFD, APD e MT.

## 1. Integrantes
- Felipe Vandevelde
- Ramonys Santos
- Artur Piumbini

## 2. Estrutura do Projeto
```text
ftc-20261-final/
|-- Sistema.slnx
|-- LICENSE
|-- README.md
|-- docs/
|   `-- relatorio.md
|-- Parte0/                        # Ponto de entrada (UI em console)
|   |-- Parte0.csproj
|   |-- Program.cs
|   `-- Screens/                   # Screens de UI para o console
|       |-- AFDScreen.cs
|       |-- APDScreen.cs
|       `-- MTScreen.cs
|-- Parte1/                        # Modulo AFD
|   |-- Parte1.csproj
|   `-- AFD.cs
|-- Parte2/                        # Modulo APD
|   |-- Parte2.csproj
|   `-- APD.cs
`-- Parte3/                        # Modulo MT
    |-- Parte3.csproj
    `-- MT.cs
```

## 3. Tecnologias
- **Linguagem:** C#
- **SDK:** .NET 10 (`net10.0`)
- **Interface de terminal:** Spectre.Console
- **Build e execucao:** `dotnet` CLI com projetos no formato SDK (`.csproj`)
- **Solucoes:** `.sln` (por modulo) e `.slnx` (solucao principal)

## 4. Como Executar
### Pre-requisitos
- .NET SDK 10.0 (ou superior) instalado

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
1. `Program.cs` exibe o menu com Spectre.Console.
2. O usuario escolhe uma opcao (`Teste`, `AFD`, `APD`, `MT` ou `Sair`).
3. O modulo correspondente executa (`AFD.Executar()`, `APD.Executar()`, `MT.Executar()`).
4. A tela especifica (`Screen`) mostra o retorno e devolve o controle ao menu principal.

Essa organizacao facilita evolucao incremental por parte (cada modulo pode crescer sem acoplamento excessivo com os demais).

## 6. Conceitos Aplicados
- **Separacao de responsabilidades (SoC):** interface concentrada em `Parte0`, com execucao dos modulos em projetos dedicados.
- **Modularidade:** cada tema (AFD/APD/MT) esta isolado em seu proprio projeto.
- **Baixo acoplamento entre modulos:** integracao feita apenas pela aplicacao principal (`Parte0`).
- **Ponto de acesso simplificado (estilo Facade):** classes `AFD`, `APD` e `MT` expoem `Executar()` como entrada publica.
- **Fluxo legivel e direto (Clean Code):** menu central com `switch` explicito e classes de tela separadas.

Estado atual observado no codigo:
- Boa base de organizacao para expansao.
- Metodos de dominio ainda estao em estagio inicial, com saidas de placeholder.
- Ha repeticao entre telas (`AFDScreen`, `APDScreen`, `MTScreen`), com potencial de extracao para componente reutilizavel.

## 7. Dependencias
- `Spectre.Console` `0.55.2`
- `Microsoft.NET.Sdk` (infraestrutura de build dos projetos)
- `net10.0` (framework alvo)

## 8. Licenca
Este projeto esta licenciado sob a **MIT License**. Consulte o arquivo [LICENSE](LICENSE) para detalhes completos.
