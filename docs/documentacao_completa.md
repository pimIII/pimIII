Documentação Completa — Projeto PIM (Controle de Estoque da Farmácia)

Versão: 1.0
Autor: Equipe PIM
Idioma: Português

---

Sumário

1. Introdução
2. Escopo e Objetivos
3. Levantamento de Requisitos
   3.1 Requisitos Funcionais
   3.2 Requisitos Não Funcionais
4. Modelagem
   4.1 Diagrama de Classes (texto)
   4.2 Diagrama de Sequência (Cadastrar Produto)
   4.3 Diagrama de Sequência (Cadastrar Funcionário)
   4.4 Diagrama de Implantação
5. Casos de Uso (detalhados)
   5.1 UC01 - Autenticar Usuário
   5.2 UC02 - Gerenciar Produtos
   5.3 UC03 - Gerenciar Fornecedores
   5.4 UC04 - Gerenciar Funcionários
   5.5 UC05 - Registrar Venda
   5.6 UC06 - Gerenciar Lotes
6. Modelagem de Dados (Entidades e atributos)
7. API / Serviços (visão das classes e responsabilidades)
8. Interface do Usuário (console)
9. Validações e Regras de Negócio
10. Manual de Instalação e Execução
11. Guia de Testes (cenários de aceitação)
12. Manutenção e Considerações Futuras
13. Anexos (arquivos UML e scripts)

---

1. Introdução

Este documento apresenta a documentação completa do projeto PIM — sistema de controle de estoque de uma farmácia. Contém requisitos, modelagem, casos de uso, diagramas, instruções de instalação e guia de testes.

2. Escopo e Objetivos

O sistema tem por objetivo gerenciar produtos, fornecedores, funcionários, lotes e vendas em um protótipo de aplicação de console em C#. Trata-se de um protótipo com armazenamento em memória e serviços que simulam operações CRUD.

3. Levantamento de Requisitos

3.1 Requisitos Funcionais

- RF01: Autenticação de usuários (login simples no protótipo).
- RF02: CRUD de Produtos (cadastrar, listar, remover).
- RF03: CRUD de Fornecedores (cadastrar, listar, remover).
- RF04: CRUD de Funcionários (cadastrar, editar, remover) com validação de CPF.
- RF05: Gerenciamento de Lotes (vínculo a produtos, validade, quantidade).
- RF06: Registro de Vendas (itens, valor, funcionário, forma de pagamento).
- RF07: Relatórios básicos (listagens).

3.2 Requisitos Não Funcionais

- RNF01: Portabilidade para Windows/.NET (recomendado .NET 8)
- RNF02: Usabilidade por interface de console.
- RNF03: Segurança mínima (login estático no protótipo).
- RNF04: Código documentado com XML comments para manutenção.
- RNF05: Performance para listas pequenas (operações em memória).

4. Modelagem

4.1 Diagrama de Classes (texto)

- Produto: NomeProduto, PrecoVenda, PrecoCusto, IdFornecedor, RequerReceita
- Fornecedor: NomeEmpresa, Cnpj, Telefone
- Funcionario: Id, Nome, CPF, Cargo, Salario, DataAdmissao
- Lote: Id, IdProduto, NumeroLote, DataValidade, Quantidade
- Venda: Id, IdFuncionario, DataVenda, ValorTotal, FormaPagamento
- Serviços: ProdutoService, FornecedorService, FuncionarioService — operam sobre coleções ou persistência
- UIs: Menu, ProdutoUI, FornecedorUI, ProdutoUI — implementam interação por console

4.2 Diagrama de Sequência (Cadastrar Produto)

(Descrição passo a passo)
1. Usuário seleciona 'Cadastrar produto' no menu.
2. UI solicita nome, preços, id do fornecedor e se requer receita.
3. Usuário fornece dados.
4. UI instancia Produto e chama ProdutoService.Salvar(produto).
5. ProdutoService adiciona produto à coleção em memória (ou persiste no BD).
6. UI exibe confirmação.

4.3 Diagrama de Sequência (Cadastrar Funcionário)

1. Administrador seleciona 'Cadastrar funcionário'.
2. UI solicita nome e CPF.
3. UI passa dados para FuncionarioService.CreateAsync(funcionario).
4. Serviço valida domínio (ValidarDominio) e CPF pelo algoritmo dos dígitos.
5. Serviço persiste funcionário se válido; caso contrário lança exceção.
6. UI captura resultado e exibe mensagem apropriada.

4.4 Diagrama de Implantação

- Máquina do usuário: executa aplicação console (dotnet run) que contém UI e serviços.
- Futuro: servidor de banco de dados (PostgreSQL / SQL Server) conectado por EF Core.

5. Casos de Uso (detalhados)

(Os casos de uso foram documentados em docs/casos_de_uso_detalhado.md — resumidos abaixo.)

5.1 UC01 - Autenticar Usuário
- Fluxo principal e alternativos (credenciais inválidas, limite de tentativas).

5.2 UC02 - Gerenciar Produtos
- Fluxo principal descrito; fluxos alternativos para entradas inválidas e fornecedor inexistente.

5.3 UC03 - Gerenciar Fornecedores
- Cadastro, listagem, remoção; validação de CNPJ (futuro).

5.4 UC04 - Gerenciar Funcionários
- Validações de domínio: Nome obrigatório, CPF válido, CPF único, Salário >= 0, Data válida.

5.5 UC05 - Registrar Venda
- Itens, verificação de estoque, atualização de quantidades.

5.6 UC06 - Gerenciar Lotes
- Cadastro, atualização, alerta de validade.

6. Modelagem de Dados (Entidades e Atributos)

Apresenta as tabelas esperadas ao migrar para BD relacional (nomes e tipos):
- produtos (id int, nome varchar(100), preco_venda numeric(10,2), preco_custo numeric(10,2), id_fornecedor int, requer_receita bit)
- fornecedores (id int, nome_empresa varchar(150), cnpj varchar(20), telefone varchar(20))
- funcionarios (id int, nome varchar(150), cpf varchar(14), cargo varchar(100), salario numeric(12,2), data_admissao date)
- lotes (id int, id_produto int, numero_lote varchar(50), data_validade date, quantidade int)
- vendas (id int, id_funcionario int, data_venda datetime, valor_total numeric(12,2), forma_pagamento varchar(30))

7. API / Serviços (visão das classes e responsabilidades)

- ProdutoService: Salvar, ListarTodos, Remover
- FornecedorService: Salvar, ListarTodos, Remover
- FuncionarioService: GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync, SearchByNameAsync
- Cada serviço valida regras de domínio antes de persistir.

8. Interface do Usuário (console)

- Menu principal: opções para controle de produtos, funcionários, vendas e fornecedores.
- UIs específicas (ProdutoUI, FornecedorUI) implementam leitura robusta (validações de entrada) e exibição de listas.

9. Validações e Regras de Negócio

- CPF: algoritmo de validação dos dígitos verificadores (implementado em FuncionarioService).
- Valores numéricos: parse seguro com retorno de prompt em caso de entrada inválida.
- Campos obrigatórios: Nome, CPF, Data de admissão.
- Regras de integridade: CPF único (quando persistido em BD).

10. Manual de Instalação e Execução

Pré-requisitos:
- .NET SDK/Runtime (recomendado net8.0)
- git (opcional)

Passos:
1. git clone https://github.com/pimiii/pimIII
2. cd pimIII
3. dotnet restore
4. dotnet build
5. dotnet run --project backend/c_sharp/estoque_farmacia/estoque_farmacia.csproj

Conversão da documentação para Word (local):
- Instalar pandoc (https://pandoc.org/) e um runtime .NET ou usar o Word diretamente.
- Com pandoc: pandoc docs/documentacao_completa.md -o docs/documentacao_completa.docx --from markdown+yaml_metadata_block+tables -V geometry:margin=1in

11. Guia de Testes (cenários de aceitação)

- TC01: Criar funcionário válido -> sucesso
- TC02: Criar funcionário com CPF inválido -> erro (ArgumentException)
- TC03: Criar produto com dados válidos -> sucesso
- TC04: Remover produto por ID válido -> sucesso
- TC05: Listar fornecedores quando vazio -> mensagem apropriada

12. Manutenção e Considerações Futuras

- Migrar armazenamento em memória para banco relacional com EF Core.
- Implementar autenticação real e controle de permissões (roles).
- Adicionar logs, tratamento centralizado de erros, testes de integração e cobertura automatizada.
- Gerar diagramas UML visuais e anexar ao relatório final.

13. Anexos

- Arquivos de modelagem em docs/:
  - docs/analise_requisitos.md
  - docs/modelagem.md
  - docs/casos_de_uso_detalhado.md
  - docs/diagrama_sequencia_cadastrar_produto.puml
  - docs/diagrama_classe.uml.txt

---

Gerar arquivo Word (DOCX)

Para produzir o arquivo Word localmente execute (recomendado):

pandoc docs/documentacao_completa.md -o docs/documentacao_completa.docx --from markdown+yaml_metadata_block+tables -V geometry:margin=1in

Se preferir que eu gere o DOCX no repositório, posso criar um .docx simples via conversão automática em ambiente com pandoc; confirme se quer que eu tente gerar o .docx aqui (posso usar pandoc se disponível neste ambiente).