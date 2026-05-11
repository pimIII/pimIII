# estoque_farmacia_winforms

Interface **desktop** (Windows Forms) do sistema de gestão de estoque/farmácia do PIM III. Este projeto é o **cliente gráfico**: janelas, formulários e fluxo de uso (login → menu → cadastros e vendas).

## Relação com `estoque_farmacia`

Ao lado desta pasta existe o projeto **`estoque_farmacia`** (aplicação **console**). Os dois compartilham a mesma base de código de domínio:

- **Modelos** (`Models`)
- **Acesso a dados** (`Data` / `AppDbContext`)
- **Regras de negócio** (`Services`)

Para **não duplicar** ficheiros, o `estoque_farmacia_winforms.csproj` inclui esses `.cs` como [linked files](https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items#compile) (`Compile Include="..\estoque_farmacia\..." Link="..."`). Alterações na regra de negócio ou no modelo refletem **automaticamente** na console e no WinForms.

O ficheiro **`appsettings.json`** (connection string do PostgreSQL) também é reutilizado a partir de `estoque_farmacia` e copiado para a pasta de saída na compilação.

Em resumo:

| Pasta / projeto        | Papel |
|------------------------|--------|
| `estoque_farmacia`     | Núcleo (EF Core, serviços, migrations) + UI em texto (console) |
| `estoque_farmacia_winforms` | UI gráfica (WinForms) que consome o mesmo núcleo |

## O que *não* fica só “na telinha”

A persistência e a maior parte da lógica ficam nos **serviços** e no **Entity Framework**, no projeto partilhado. O WinForms **apresenta** dados, captura input e chama serviços; não substitui o modelo de dados central.

## Stack

- .NET 10 (`net10.0-windows`)
- Windows Forms (UI criada em código, sem Designer obrigatório)
- Entity Framework Core + Npgsql (PostgreSQL)
- `Microsoft.Extensions.DependencyInjection` (registo de `AppDbContext`, serviços e forms em `Program.cs`)

## Como executar

1. Instalar [.NET SDK](https://dotnet.microsoft.com/download) compatível com o target do projeto.
2. Ter **PostgreSQL** acessível e a base configurada conforme as migrations do projeto `estoque_farmacia` (ou o esquema acordado pela equipa).
3. Ajustar a connection string em `..\estoque_farmacia\appsettings.json` (é esse ficheiro que o WinForms usa na build).
4. Na pasta deste projeto:

   ```bash
   dotnet run
   ```

## Estrutura desta pasta

- `Program.cs` — arranque, DI, EF, primeira janela (`LoginForm`).
- `Forms/` — ecrãs (`LoginForm`, `MenuForm`, cadastros, vendas).
- `UIHelper.cs` — paleta de cores e estilos comuns aos formulários.

Quem procura migrations, `AppDbContext` completo ou a variante **console** do mesmo sistema deve abrir o repositório/pasta **`estoque_farmacia`**.
