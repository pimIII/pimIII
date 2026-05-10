# Documentação do ORM utilizado no projeto

## 1. O que é um ORM

ORM (Object-Relational Mapping, ou Mapeamento Objeto-Relacional) é uma camada
de software que traduz dados entre o mundo da programação orientada a objetos
(classes, propriedades, objetos em memória) e o mundo dos bancos de dados
relacionais (tabelas, colunas, linhas, SQL).

Em vez de o desenvolvedor escrever consultas SQL manualmente, ele trabalha
com objetos da linguagem e o ORM se encarrega de gerar o SQL correspondente
e executá-lo no banco.

## 2. ORM escolhido: Entity Framework Core (EF Core)

O projeto utiliza **Entity Framework Core**, o ORM oficial da Microsoft para
a plataforma .NET. A versão utilizada é a `10.0.4`, em conjunto com o
provedor `Npgsql.EntityFrameworkCore.PostgreSQL 10.0.1`, que permite ao
EF Core se comunicar com o banco PostgreSQL.

## 3. Justificativa da adoção

A escolha pelo EF Core foi feita pelos seguintes motivos:

1. **Integração nativa com C# e .NET 10**: o EF Core é o ORM padrão da Microsoft,
   recebe atualizações junto com o framework e tem suporte oficial de longo prazo.
2. **Migrations**: a evolução do esquema do banco é versionada em arquivos C#,
   o que permite que toda a equipe (e o avaliador) reproduza a mesma estrutura
   do banco com um único comando (`dotnet ef database update`).
3. **Tipagem forte**: erros de digitação em nomes de colunas ou tipos
   incompatíveis são detectados em tempo de compilação, não em produção.
4. **LINQ**: as consultas são escritas em C# usando LINQ, o que reduz erros
   de sintaxe SQL e melhora a legibilidade do código.
5. **Independência de SGBD**: a troca do SQL Server para o PostgreSQL exigiu
   apenas alterar o pacote do provedor e a string de conexão. O código de
   consultas e modelos não foi afetado.
6. **Familiaridade da equipe**: todos os integrantes do grupo estudaram EF Core
   ao longo do semestre e conseguem responder questões sobre o seu funcionamento.

## 4. Configuração no projeto

A configuração do EF Core fica em três arquivos principais:

- `estoque_farmacia.csproj` — declara os pacotes NuGet utilizados.
- `Data/AppDbContext.cs` — define o contexto do banco (DbSets e configurações
  do modelo via Fluent API).
- `Data/AppDbContextFactory.cs` — fornece o contexto para os comandos de
  migração em tempo de design.
- `appsettings.json` — guarda a string de conexão com o PostgreSQL.

Trecho do `Program.cs` que registra o contexto:

```csharp
services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});
```

## 5. Comandos SQL gerados pelo EF Core

A seguir estão os comandos SQL equivalentes às operações realizadas no
projeto. Eles foram inferidos a partir das chamadas LINQ existentes nos
Services. Para coletar os comandos exatos em tempo de execução, o EF Core
oferece logging — basta adicionar `.LogTo(Console.WriteLine)` no
`DbContextOptionsBuilder`.

### 5.1. Operações sobre `Funcionarios`

`FuncionarioService.Salvar(novo)` →

```sql
INSERT INTO "Funcionarios"
    ("Nome", "CPF", "Cargo", "SenhaHash", "DataAdmissao", "Ativo", "DataDemissao")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
RETURNING "Id";
```

`FuncionarioService.ListarTodos()` →

```sql
SELECT f."Id", f."Nome", f."CPF", f."Cargo", f."SenhaHash",
       f."DataAdmissao", f."Ativo", f."DataDemissao"
FROM "Funcionarios" AS f;
```

`FuncionarioService.BuscarPorId(id)` →

```sql
SELECT f."Id", f."Nome", f."CPF", f."Cargo", f."SenhaHash",
       f."DataAdmissao", f."Ativo", f."DataDemissao"
FROM "Funcionarios" AS f
WHERE f."Id" = @id
LIMIT 1;
```

`FuncionarioService.Remover(id)` →

```sql
DELETE FROM "Funcionarios" WHERE "Id" = @id;
```

### 5.2. Operações sobre `Produtos`

`ProdutoService.Salvar(novo)` →

```sql
INSERT INTO "Produtos"
    ("NomeProduto", "PrecoVenda", "PrecoCusto", "IdFornecedor", "RequerReceita")
VALUES (@p0, @p1, @p2, @p3, @p4)
RETURNING "Id";
```

`ProdutoService.ListarTodos()` →

```sql
SELECT p."Id", p."NomeProduto", p."PrecoVenda", p."PrecoCusto",
       p."IdFornecedor", p."RequerReceita"
FROM "Produtos" AS p;
```

`ProdutoService.Remover(id)` →

```sql
DELETE FROM "Produtos" WHERE "Id" = @id;
```

### 5.3. Operações sobre `Fornecedores`

`FornecedorService.Salvar(novo)` →

```sql
INSERT INTO "Fornecedores" ("NomeEmpresa", "Cnpj", "Telefone")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
```

`FornecedorService.ListarTodos()` →

```sql
SELECT f."Id", f."NomeEmpresa", f."Cnpj", f."Telefone"
FROM "Fornecedores" AS f;
```

## 6. Otimização: eager loading e lazy loading

O EF Core oferece três estratégias para carregar dados relacionados entre
entidades:

| Estratégia       | Quando carrega os dados relacionados                        |
|------------------|--------------------------------------------------------------|
| Eager loading    | Junto da consulta principal, com `.Include()`                |
| Explicit loading | Sob demanda, com chamada explícita `context.Entry().Load()`  |
| Lazy loading     | Automaticamente quando a propriedade é acessada              |

### Decisão adotada no projeto

**Optamos por NÃO utilizar lazy loading e por NÃO utilizar eager loading
neste projeto.**

Motivos:

1. Cada Service trabalha sobre uma única entidade (`Funcionario`, `Produto`,
   `Fornecedor`). Não há, neste momento, navegações entre entidades que
   demandem `.Include()`.
2. A relação lógica entre `Produto.IdFornecedor` e `Fornecedor.Id` é resolvida
   pelo código da aplicação quando necessário, evitando carregar fornecedores
   inteiros em listagens que mostram apenas o `IdFornecedor`.
3. Lazy loading exigiria o pacote `Microsoft.EntityFrameworkCore.Proxies` e
   propriedades de navegação `virtual`. Como o tamanho do banco é pequeno e
   as consultas são poucas, o ganho não compensa o custo de configuração
   adicional e o risco do problema N+1 (descrito a seguir).

### O problema N+1

O problema N+1 acontece quando uma consulta principal retorna N registros
e, para cada um deles, é feita uma consulta extra ao banco para carregar
dados relacionados — gerando 1 + N consultas no total. Exemplo:

```csharp
// Forma errada: 1 consulta para listar + 1 por produto para o fornecedor
foreach (var p in context.Produtos.ToList())
{
    var f = context.Fornecedores.Find(p.IdFornecedor); // N consultas extras
}
```

A forma correta, quando for necessário trazer dados juntos no futuro:

```csharp
// 1 consulta apenas, com JOIN feito pelo EF Core
var resultado = (from p in context.Produtos
                 join f in context.Fornecedores on p.IdFornecedor equals f.Id
                 select new { p.NomeProduto, f.NomeEmpresa }).ToList();
```

### Boas práticas adotadas

- `ListarTodos()` retorna apenas as colunas necessárias por meio do mapeamento
  do `DbSet` da entidade, sem trazer informações desnecessárias.
- `BuscarPorId()` usa `FirstOrDefault(p => p.Id == id)`, que é traduzido para
  uma cláusula `WHERE` com `LIMIT 1` — o banco retorna no máximo uma linha.
- Não há laços que consultam o banco repetidamente; consultas são feitas uma
  vez e os resultados são iterados em memória.

## 7. Migrations

As migrations ficam na pasta `Migrations/`. Cada migration é uma classe C#
com dois métodos: `Up()` (aplica a mudança) e `Down()` (reverte). Os comandos
úteis são:

```bash
# Adicionar uma nova migration após mudar o modelo
dotnet ef migrations add NomeDaMigration

# Aplicar todas as migrations pendentes ao banco
dotnet ef database update

# Reverter para uma migration específica
dotnet ef database update NomeDaMigrationAnterior

# Remover a última migration (apenas se ainda não foi aplicada)
dotnet ef migrations remove

# Gerar um script SQL com todas as migrations
dotnet ef migrations script
```

## 8. Como reproduzir o banco do zero

1. Instalar o PostgreSQL 17 (https://www.postgresql.org/download/).
2. Criar o banco vazio:
   ```sql
   CREATE DATABASE estoque_farmacia;
   ```
3. Ajustar a string de conexão em `appsettings.json` com o usuário e a senha
   definidos na instalação.
4. Aplicar as migrations:
   ```bash
   cd backend/c_sharp/estoque_farmacia
   dotnet ef database update
   ```
