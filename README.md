# Sistema de Estoque Farmácia

Sistema de gerenciamento de estoque para farmácia, desenvolvido em C# com .NET 10.0 e Entity Framework Core, utilizando PostgreSQL como banco de dados.

O projeto possui duas interfaces:

- **Aplicação Console** (`estoque_farmacia`) — versão original em terminal.
- **Aplicação Windows Forms** (`estoque_farmacia_winforms`) — interface gráfica desktop com login, menu e cadastros.

Ambas compartilham os mesmos Models, Services e DbContext.

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 10.0 SDK** ou superior
  - Download: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

- **PostgreSQL 17** (ou versão superior)
  - Download: [https://www.postgresql.org/download/](https://www.postgresql.org/download/)
  - Durante a instalação, anote a senha do usuário `postgres` e a porta (padrão `5432`).

- **pgAdmin** (opcional) — interface gráfica para administrar o PostgreSQL.

## Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/pimIII/pimIII.git
cd pimIII/backend/c_sharp
```

### 2. Criar o banco de dados

Pelo `psql` ou pelo pgAdmin, execute:

```sql
CREATE DATABASE estoque_farmacia;
```

### 3. Ajustar a connection string

Edite `estoque_farmacia/appsettings.json` com o usuário, senha e porta usados na instalação:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_farmacia;Username=postgres;Password=SUA_SENHA"
  }
}
```

### 4. Restaurar dependências e aplicar migrations

```bash
cd estoque_farmacia
dotnet restore
dotnet ef database update
```

Isso criará as tabelas `Funcionarios`, `Produtos`, `Fornecedores` e `__EFMigrationsHistory` dentro do banco `estoque_farmacia`.

## Executando a aplicação

### Aplicação Console

```bash
cd estoque_farmacia
dotnet run
```

### Aplicação Windows Forms

```bash
cd estoque_farmacia_winforms
dotnet run
```

### Credenciais padrão (login)

- **Usuário:** `admin`
- **Senha:** `123`

## Compilando um executável

```bash
# Console
cd estoque_farmacia
dotnet publish -c Release -o ./publish

# Windows Forms
cd estoque_farmacia_winforms
dotnet publish -c Release -o ./publish
```

## Estrutura do projeto

```
backend/c_sharp/
├── estoque_farmacia/                    # Projeto Console (.NET 10)
│   ├── Models/                          # Classes de domínio
│   ├── Services/                        # Regras de negócio + EF Core
│   ├── UI/                              # Telas em texto (Console)
│   ├── Data/                            # AppDbContext + Factory
│   ├── Migrations/                      # Migrations do EF Core
│   ├── Program.cs                       # Ponto de entrada
│   ├── appsettings.json                 # Connection string
│   └── estoque_farmacia.csproj
└── estoque_farmacia_winforms/           # Projeto Windows Forms (.NET 10)
    ├── Forms/                           # Telas (Login, Menu, CRUDs, Venda)
    ├── UIHelper.cs                      # Paleta de cores
    ├── Program.cs                       # Ponto de entrada
    └── estoque_farmacia_winforms.csproj # Reaproveita Models/Services via Link
```

## Funcionalidades

- **Login** com usuário e senha.
- **Cadastro de Produtos** com validação de preço e indicador de receita.
- **Cadastro de Funcionários** com hash de senha (SHA-256) e inativação.
- **Cadastro de Fornecedores** (nome, CNPJ, telefone).
- **Registro de Vendas** com carrinho, desconto e total automático.

## Tecnologias utilizadas

- **.NET 10.0** — Plataforma
- **C#** — Linguagem
- **Windows Forms** — Interface desktop
- **Entity Framework Core 10.0.4** — ORM
- **Npgsql.EntityFrameworkCore.PostgreSQL 10.0.1** — Provedor PostgreSQL
- **Microsoft.Extensions.DependencyInjection** — Injeção de dependência

## Solução de problemas

### Erro: "57P03 - the database system is starting up" ou conexão recusada

**Causa:** o serviço do PostgreSQL não está rodando.

**Solução:** abra o gerenciador de serviços do Windows (`services.msc`) e inicie o serviço `postgresql-x64-17` (ou a versão equivalente).

### Erro: "28P01 - password authentication failed"

**Causa:** usuário ou senha incorretos na connection string.

**Solução:** atualizar `appsettings.json` com a senha definida na instalação do PostgreSQL.

### Erro: "Database does not exist"

**Causa:** o banco `estoque_farmacia` ainda não foi criado.

**Solução:**

```sql
CREATE DATABASE estoque_farmacia;
```

E em seguida:

```bash
dotnet ef database update
```

## Autores

Desenvolvido como projeto acadêmico (PIM III) pelos seguintes integrantes:

- Anderson da Silva Araújo
- Daniel Reis Oliveira
- Eduardo Goulart Pessini
- Geovanna Xavier Alves
- Geovanni da Silva de Andrade
- Patrick Hugo Costa Freitas
- Roberto Matheus da Costa Domingues Martins
- Vinicius Umeda de Araujo

## Licença

MIT License
