# Sistema de Estoque Farmácia

Sistema de gerenciamento de estoque para farmácia, desenvolvido em C# com .NET 10.0 e Entity Framework Core, utilizando SQL Server como banco de dados.

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 10.0 SDK** ou superior
  - Download: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
  
- **SQL Server Express** (ou versão completa do SQL Server)
  - Download: [https://www.microsoft.com/pt-br/sql-server/sql-server-downloads](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
  - Durante a instalação, use a instância padrão: `SQLEXPRESS`
  - Ative "Autenticação do Windows (Trusted Connection)"

- **SQL Server Management Studio (SSMS)** - Opcional, mas recomendado
  - Para gerenciar e visualizar o banco de dados
  - Download: [https://docs.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms](https://docs.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms)

## Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/pimIII/pimIII.git
cd pimIII/backend/c_sharp/estoque_farmacia
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Configurar o banco de dados

O projeto utiliza Entity Framework Core com migrações automáticas. Para criar o banco de dados:

```bash
dotnet ef database update
```

Isso criará automaticamente:
- Banco de dados: `EstoqueFarmacia`
- Tabelas: `Funcionarios`, `Produtos`, `Fornecedores`

**Verificação (opcional):**
Se tiver SQL Server Management Studio instalado, conecte-se a `.\SQLEXPRESS` e verifique se a base de dados `EstoqueFarmacia` foi criada.

## Executando a Aplicação

### Via linha de comando

```bash
dotnet run
```

### Credenciais padrão

- **Login:** `admin`
- **Senha:** `123`

## Compilando um Executável

Para criar um arquivo executável standalone que pode ser distribuído:

```bash
dotnet publish -c Release -o ./publish
```

O executável estará em: `./publish/estoque_farmacia.exe`

## Estrutura do Projeto

```
estoque_farmacia/
├── Models/              # Classes de domínio
│   ├── Funcionario.cs
│   ├── Produto.cs
│   └── Fornecedor.cs
├── Services/            # Lógica de negócio
│   ├── FuncionarioService.cs
│   ├── ProdutoService.cs
│   └── FornecedorService.cs
├── UI/                  # Interface do usuário (console)
│   ├── Menu.cs
│   ├── FuncionarioUI.cs
│   ├── ProdutoUI.cs
│   └── FornecedorUI.cs
├── Data/                # Camada de dados (EF Core)
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── Migrations/          # Histórico de migrações do banco
├── Program.cs           # Ponto de entrada
├── appsettings.json     # Configurações (connection string)
└── estoque_farmacia.csproj
```

## Funcionalidades

- **Controle de Funcionários**
  - Cadastro, listagem, atualização e inativação de funcionários
  - Persistência no banco de dados via EF Core

- **Controle de Produtos**
  - Gerenciamento de produtos do estoque
  - Associação com fornecedores

- **Controle de Fornecedores**
  - Cadastro e gerenciamento de fornecedores

- **Controle de Vendas**
  - Módulo em desenvolvimento

## Tecnologias Utilizadas

- **.NET 10.0** - Framework
- **C#** - Linguagem de programação
- **Entity Framework Core 10.0.0** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **Dependency Injection** - Padrão de injeção de dependências

## Solução de Problemas

### Erro: "Erro ao Localizar Servidor/Instância Especificado"

**Causa:** SQL Server não está instalado ou não está rodando.

**Solução:**
1. Instale SQL Server Express
2. Inicie o serviço SQL Server:
   - Windows: Abra "Serviços" (services.msc) e procure por "SQL Server (SQLEXPRESS)"
   - Clique com botão direito → Iniciar

### Erro: "The process cannot access the file"

**Causa:** A aplicação está aberta e bloqueando a recompilação.

**Solução:** Feche a aplicação antes de compilar novamente.

### Erro: "Database does not exist"

**Causa:** Migrations não foram aplicadas.

**Solução:**
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
