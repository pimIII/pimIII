# Modelagem do Sistema

## Diagrama de Classes (Resumo)

- Produto
  - Id (int)
  - NomeProduto (string)
  - PrecoVenda (decimal)
  - PrecoCusto (decimal)
  - IdFornecedor (int)
  - RequerReceita (bool)

- Fornecedor
  - Id (int)
  - NomeEmpresa (string)
  - Cnpj (string)
  - Telefone (string)

- Funcionario
  - Id (int)
  - Nome (string)
  - CPF (string)
  - Cargo (string)
  - Salario (decimal)
  - DataAdmissao (DateTime)

- Lote
  - Id (int)
  - IdProduto (int)
  - NumeroLote (string)
  - DataValidade (DateTime)
  - Quantidade (int)

- Venda
  - Id (int)
  - IdFuncionario (int)
  - DataVenda (DateTime)
  - ValorTotal (decimal)
  - FormaPagamento (string)


## Diagrama de Sequência (Exemplo - Cadastrar Produto)

1. Usuário seleciona "Cadastrar produto" no menu.
2. UI solicita dados do produto (nome, preços, id fornecedor, requer receita).
3. UI cria objeto Produto e chama ProdutoService.Salvar(produto).
4. ProdutoService adiciona o produto à lista em memória.
5. UI confirma sucesso ao usuário.


## Diagrama de Implantação (Resumo)

- Cliente (Console App)
  - Executa a aplicação de console (dotnet run) interagindo com usuário.

- Servidor (opcional, futuro)
  - Banco de dados relacional (ex.: PostgreSQL ou SQL Server)


<!-- Arquivo com a modelagem em texto; para entrega acadêmica, pode-se gerar diagramas visuais a partir destas descrições. -->