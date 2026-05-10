# REQUISITOS E ESPECIFICAÇÕES DO SISTEMA
## Sistema de Gerenciamento de Estoque de Farmácia - PIM III

---

## SUMÁRIO

1. Visão Geral do Projeto
2. Requisitos Funcionais (RF)
3. Requisitos Não-Funcionais (RNF)
4. Casos de Uso
5. Atores do Sistema
6. Regras de Negócio
7. Matriz de Rastreabilidade
8. Referências

---

## 1 VISÃO GERAL DO PROJETO

### 1.1 Nome do Sistema

Sistema de Gerenciamento de Estoque de Farmácia

### 1.2 Descrição Geral

Sistema web/desktop para gerenciar o estoque de uma farmácia, controlando produtos, funcionários, fornecedores, vendas e movimentações de estoque.

### 1.3 Objetivo Principal

Automatizar o controle de inventário, vendas e funcionários de uma farmácia, reduzindo erros e oferecendo relatórios em tempo real.

### 1.4 Público-Alvo

- Gerentes de Farmácia: relatórios e controle total
- Farmacêuticos: venda de medicamentos controlados
- Atendentes: venda e atendimento básico
- Estoquistas: controle de entrada/saída de produtos

---

## 2 REQUISITOS FUNCIONAIS (RF)

Os Requisitos Funcionais descrevem o que o sistema faz (as funcionalidades).

### 2.1 RF01 - Autenticação e Login

O sistema deve permitir que funcionários façam login com usuário e senha.

#### 2.1.1 Requisitos Detalhados

- O usuário digita Login e Senha
- Sistema valida as credenciais
- Se válidas, usuário acessa o menu principal
- Se inválidas, mostra erro e volta para login
- Máximo 3 tentativas erradas (opcional: bloqueia temporariamente)
- Senha deve ser criptografada no banco (hash)

#### 2.1.2 Atores Envolvidos

Todos os funcionários

#### 2.1.3 Prioridade

ALTA

---

### 2.2 RF02 - Gerenciar Funcionários

O sistema deve permitir cadastrar, listar, atualizar e inativar funcionários.

#### 2.2.1 RF02.1 - Cadastrar Funcionário
- Nome (obrigatório)
- CPF (obrigatório, único, validado)
- Cargo (Gerente, Farmacêutico, Atendente, Estoquista)
- Senha (obrigatório)
- Data de Admissão (preenchida automaticamente com hoje)
- Status Ativo (preenchido automaticamente como true)

Validações: Nome não vazio, CPF único, senha mínimo 6 caracteres

Resultado: Novo funcionário salvo com ID gerado automaticamente

#### 2.2.2 RF02.2 - Listar Funcionários

- Mostrar todos os funcionários cadastrados em uma tabela
- Colunas: ID, Nome, CPF, Cargo, Status (Ativo/Inativo)
- Permitir filtrar por Status (Ativos/Inativos)
- Mostrar total de funcionários

#### 2.2.3 RF02.3 - Buscar Funcionário por ID

- Usuário digita o ID
- Sistema retorna os detalhes completos
- Se não encontrar, mostra erro

#### 2.2.4 RF02.4 - Atualizar Funcionário

- Usuário seleciona funcionário
- Sistema mostra dados atuais
- Usuário pode atualizar: Nome, CPF, Cargo
- Não pode alterar ID ou Data de Admissão
- Salva as alterações

#### 2.2.5 RF02.5 - Inativar Funcionário

- Em vez de DELETAR, marca como inativo
- Registra a data de saída (DataDemissao)
- Funcionário ainda aparece no histórico, mas não pode mais fazer login

#### 2.2.6 Atores Envolvidos

Gerente (criar, editar, inativar), Funcionário (visualizar próprios dados)

#### 2.2.7 Prioridade

ALTA

---

### 2.3 RF03 - Gerenciar Produtos

O sistema deve permitir cadastrar, listar, atualizar e deletar produtos do estoque.

#### 2.3.1 RF03.1 - Cadastrar Produto

- Nome do Produto (obrigatório)
- Preço de Venda (obrigatório, > 0)
- Preço de Custo (obrigatório, > 0)
- ID Fornecedor (obrigatório - deve existir no banco)
- Requer Receita (true/false)
- Descrição (opcional)

Validação: Todos os campos obrigatórios preenchidos

Resultado: Novo produto com ID gerado

#### 2.3.2 RF03.2 - Listar Produtos

- Mostrar todos os produtos em tabela
- Colunas: ID, Nome, Preço Venda, Preço Custo, Fornecedor, Requer Receita
- Mostrar Margem de Lucro (Venda - Custo)
- Permitir buscar por nome

#### 2.3.3 RF03.3 - Buscar Produto

- Busca por ID ou Nome
- Retorna detalhes completos

#### 2.3.4 RF03.4 - Atualizar Produto

- Editar dados do produto existente
- Não pode alterar ID

#### 2.3.5 RF03.5 - Deletar Produto

- Remove o produto do sistema
- Validação: Não pode deletar se tem estoque ativo

#### 2.3.6 Atores Envolvidos

Gerente, Farmacêutico, Estoquista

#### 2.3.7 Prioridade

ALTA

---

### 2.4 RF04 - Gerenciar Fornecedores

O sistema deve permitir cadastrar, listar, atualizar e deletar fornecedores.

#### 2.4.1 RF04.1 - Cadastrar Fornecedor

- Nome da Empresa (obrigatório)
- CNPJ (obrigatório, único, validado)
- Telefone (obrigatório)
- Email (opcional)
- Endereço (opcional)

#### 2.4.2 RF04.2 - Listar Fornecedores

- Mostrar todos em tabela com: ID, Nome, CNPJ, Telefone
- Mostrar quantos produtos cada fornecedor tem

#### 2.4.3 RF04.3 - Atualizar Fornecedor

- Editar dados do fornecedor

#### 2.4.4 RF04.4 - Deletar Fornecedor

- Validação: Não pode deletar se tem produtos associados

#### 2.4.5 Atores Envolvidos

Gerente, Estoquista

#### 2.4.6 Prioridade

MÉDIA

---

### 2.5 RF05 - Controle de Estoque / Lotes

O sistema deve controlar o estoque de cada produto por lote (data de validade).

#### 2.5.1 RF05.1 - Cadastrar Lote de Produto

- ID Produto (obrigatório)
- Número do Lote (obrigatório, único)
- Data de Validade (obrigatório)
- Quantidade Recebida (obrigatório, > 0)
- Data de Entrada (preenchida automaticamente)

#### 2.5.2 RF05.2 - Listar Estoque

- Mostrar quantidade disponível de cada produto
- Alertar produtos com estoque baixo (< 10 unidades)
- Alertar produtos vencidos ou vencendo (< 30 dias)
- Mostrar por Lote: Produto, Quantidade, Data Validade, Status

#### 2.5.3 RF05.3 - Diminuir Estoque

- Ao vender um produto, reduz quantidade do lote
- Usa primeiro o lote com vencimento mais próximo (FIFO)

#### 2.5.4 Atores Envolvidos

Estoquista, Farmacêutico, Gerente

#### 2.5.5 Prioridade

ALTA

---

### 2.6 RF06 - Registrar Vendas

O sistema deve registrar e controlar as vendas realizadas.

#### 2.6.1 RF06.1 - Registrar Venda

- ID Funcionário (quem vendeu) - preenchido automaticamente
- Data da Venda - preenchida automaticamente
- Produto(s) - permitir múltiplos
- Quantidade de cada produto
- Valor Total - calculado automaticamente
- Forma de Pagamento (Dinheiro, Cartão, Cheque, PIX)

Validação:
- Produto tem estoque disponível?
- Se Requer Receita, solicita confirmação da receita
- Estoque não pode ficar negativo

#### 2.6.2 RF06.2 - Listar Vendas

- Mostrar histórico de vendas
- Filtrar por: Data, Funcionário, Forma de Pagamento
- Mostrar Total de Vendas do Dia/Mês

#### 2.6.3 RF06.3 - Cancelar Venda

- Devolve o estoque ao produto
- Registra o cancelamento com motivo

#### 2.6.4 Atores Envolvidos

Farmacêutico, Atendente, Gerente

#### 2.6.5 Prioridade

ALTA

---

### 2.7 RF07 - Dashboard com Alertas

Sistema mostra dashboard com informações importantes quando usuário faz login.

#### 2.7.1 Informações Exibidas

- Total de Produtos no Estoque
- Produtos com Estoque Baixo (< 10 unidades)
- Produtos Vencidos ou Vencendo em 30 dias
- Total de Vendas do Dia
- Funcionários Ativos
- Último login

#### 2.7.2 Prioridade

MÉDIA

---

### 2.8 RF08 - Relatórios

Sistema gera relatórios para análise de negócio.

#### 2.8.1 Tipos de Relatórios

- Relatório de Estoque: quantidade e valor total de produtos
- Relatório de Vendas: vendas por período, por produto, por funcionário
- Relatório de Funcionários: lista de ativos/inativos
- Relatório de Fornecedores: produtos por fornecedor

#### 2.8.2 Formato

Exibir em tela e permitir exportar para PDF/Excel

#### 2.8.3 Prioridade

BAIXA

---

## 3 REQUISITOS NÃO-FUNCIONAIS (RNF)

Os Requisitos Não-Funcionais descrevem como o sistema funciona (qualidades).

### 3.1 RNF01 - Segurança

- Autenticação: Login obrigatório com usuário e senha
- Criptografia de Senha: Usar hash (BCrypt ou PBKDF2), não texto simples
- Permissões de Acesso:
  - Gerente: acesso total
  - Farmacêutico: produtos, estoque, vendas controladas
  - Atendente: vendas básicas
  - Estoquista: entrada/saída de estoque
- Auditoria: Registrar quem fez o quê e quando
- Proteção de Dados Sensíveis: CPF, CNPJ criptografados no banco

Prioridade: CRÍTICA

---

### 3.2 RNF02 - Performance

- Tempo de Resposta: Todas as operações < 2 segundos
- Listas com Paginação: Máximo 50 registros por página
- Busca Rápida: Índices no banco de dados para CPF, CNPJ, Nome Produto
- Concorrência: Suportar múltiplos usuários simultâneos (mínimo 10)

Prioridade: ALTA

---

### 3.3 RNF03 - Usabilidade

- Interface Amigável: Menus claros e intuitivos
- Mensagens de Erro: Explicar o problema em português claro
- Validação de Entrada: Não permitir dados inválidos
- Confirmações: Pedir confirmação antes de deletar/inativar
- Documentação: Guia de usuário disponível
- Help Contextual: Dicas sobre cada campo

Prioridade: ALTA

---

### 3.4 RNF04 - Disponibilidade

- Uptime: Sistema disponível 99% do tempo (exceto manutenção)
- Backup: Dados salvos diariamente
- Recuperação: Plano de recuperação em caso de falha
- Sincronização: Dados atualizados em tempo real

Prioridade: ALTA

---

### 3.5 RNF05 - Banco de Dados

- SGBD: PostgreSQL (ou PostgreSQL)
- Normalização: Até 3ª forma normal
- Backup: Automático diariamente
- Capacidade: Suportar mínimo 100.000 registros
- Integridade Referencial: Chaves estrangeiras habilitadas

Prioridade: CRÍTICA

---

### 3.6 RNF06 - Manutenibilidade

- Código Limpo: Seguir padrões C# (PascalCase para classes, camelCase para variáveis)
- Documentação: XML comments em todas as classes e métodos
- Arquitetura: Padrão em 3 camadas (Model, Service, UI)
- Testes: Testes unitários para Services
- Versionamento: Usar Git com commits descritivos

Prioridade: ALTA

---

### 3.7 RNF07 - Compatibilidade

- Plataformas: Windows, Linux (com .NET Core)
- Browsers: Chrome, Edge, Firefox (se virar Web)
- Resoluções: Suportar telas 1024x768 em diante

Prioridade: MÉDIA

---

### 3.8 RNF08 - Escalabilidade

- Arquitetura: Preparada para migração para Web (ASP.NET)
- Banco de Dados: Preparado para crescimento
- ORM: Usar Entity Framework para facilitar futura migração
- APIs: Preparar endpoints REST para futura integração com outras sistemas

Prioridade: MÉDIA

---

## 4 CASOS DE USO

### 4.1 UC01 - Login no Sistema

#### 4.1.1 Ator Principal

Funcionário

#### 4.1.2 Pré-condição

Sistema ligado

#### 4.1.3 Fluxo Principal

1. Funcionário inicia o sistema
2. Sistema exibe tela de login
3. Funcionário digita usuário e senha
4. Sistema valida credenciais
5. Se válidas, mostra menu principal
6. Se inválidas, mostra erro e volta para login

#### 4.1.4 Pós-condição

Funcionário autenticado e logado

---

### 4.2 UC02 - Cadastrar Novo Funcionário

#### 4.2.1 Ator Principal

Gerente

#### 4.2.2 Pré-condição

Gerente logado, não existe funcionário com esse CPF

#### 4.2.3 Fluxo Principal

1. Gerente escolhe "Cadastrar Funcionário"
2. Sistema mostra formulário
3. Gerente preenche: Nome, CPF, Cargo, Senha
4. Gerente clica "Salvar"
5. Sistema valida dados
6. Sistema gera ID único
7. Sistema salva no banco
8. Sistema mostra "Salvo com sucesso"

#### 4.2.4 Validações

- Nome não vazio
- CPF válido e único
- Senha com mínimo 6 caracteres

---

### 4.3 UC03 - Registrar Venda

#### 4.3.1 Ator Principal

Farmacêutico / Atendente

#### 4.3.2 Pré-condição

Funcionário logado, produtos em estoque

#### 4.3.3 Fluxo Principal

1. Funcionário escolhe "Registrar Venda"
2. Sistema mostra tela de venda
3. Funcionário busca e seleciona produto(s)
4. Funcionário digita quantidade
5. Sistema mostra valor total
6. Funcionário escolhe forma de pagamento
7. Se produto requer receita, sistema pede comprovação
8. Funcionário clica "Finalizar Venda"
9. Sistema reduz estoque
10. Sistema mostra "Venda realizada com sucesso"
11. Mostra número da venda para nota

#### 4.3.4 Validações

- Produto existe?
- Quantidade disponível?
- Se requer receita, tem comprovação?

---

### 4.4 UC04 - Consultar Estoque Baixo

#### 4.4.1 Ator Principal

Gerente / Estoquista

#### 4.4.2 Pré-condição

Sistema ligado

#### 4.4.3 Fluxo Principal

1. Usuário acessa Dashboard
2. Sistema mostra lista de produtos com < 10 unidades
3. Sistema mostra também produtos vencendo em 30 dias
4. Usuário pode clicar em produto para ver detalhes
5. Usuário pode gerar relatório de reposição de estoque

---

## 5 ATORES DO SISTEMA

### 5.1 Gerente

Permissões: Acesso total

Funcionalidades:
- Gerenciar funcionários (criar, editar, inativar)
- Gerenciar produtos e fornecedores
- Ver Dashboard completo
- Gerar relatórios
- Ver todas as vendas

### 5.2 Farmacêutico

Permissões: Acesso médio

Funcionalidades:
- Registrar vendas (inclusive medicamentos controlados)
- Verificar estoque
- Ver medicamentos que requerem receita
- Listar funcionários (apenas visualizar)

### 5.3 Atendente

Permissões: Acesso limitado

Funcionalidades:
- Registrar vendas (apenas venda livre)
- Verificar estoque básico
- Listar produtos

### 5.4 Estoquista

Permissões: Acesso específico

Funcionalidades:
- Receber produtos (entrada de estoque)
- Registrar lotes
- Verificar validade
- Listar estoque por lote

---

## 6 REGRAS DE NEGÓCIO

### 6.1 RN01 - Estoque não pode ser negativo

Uma venda SÓ pode ser realizada se existir quantidade suficiente do produto.

### 6.2 RN02 - Medicamentos controlados precisam de receita

Se um medicamento tem RequerReceita = true, não pode ser vendido sem comprovação de receita médica.

### 6.3 RN03 - Funcionário inativo não pode fazer login

Se Ativo = false, sistema rejeita o login, mesmo com senha correta.

### 6.4 RN04 - Produto sem fornecedor não pode existir

Todo produto DEVE ter um fornecedor associado.

### 6.5 RN05 - Usar FIFO para vencimento

Ao vender um produto com múltiplos lotes, usa-se primeiro o lote com vencimento mais próximo.

### 6.6 RN06 - Único identificador

CPF e CNPJ devem ser únicos no sistema.

### 6.7 RN07 - Auditoria de deletar

Ao invés de DELETAR funcionários, marca-se como inativo para manter histórico.

### 6.8 RN08 - Margem de lucro

O Preço de Venda DEVE ser maior que o Preço de Custo.

---

## 7 MATRIZ DE RASTREABILIDADE

| RF | RNF | UC | Ator | Prioridade |
|----|-----|-----|------|-----------|
| RF01 | RNF01 | UC01 | Todos | CRÍTICA |
| RF02 | RNF01, RNF03 | UC02 | Gerente | ALTA |
| RF03 | RNF02, RNF05 | UC03 | Farm, Atend | ALTA |
| RF04 | RNF03 | - | Gerente | MÉDIA |
| RF05 | RNF02, RNF05 | UC03 | Estoquista | ALTA |
| RF06 | RNF02, RNF03 | UC03 | Farm, Atend | ALTA |
| RF07 | RNF03 | UC04 | Gerente | MÉDIA |
| RF08 | RNF02 | - | Gerente | BAIXA |

---

## 8 OBSERVAÇÕES IMPORTANTES

Este documento serve como base para os Diagramas UML. Os Casos de Uso serão detalhados em diagramas separados. As Regras de Negócio devem ser validadas nos Services (C#). Os Requisitos Não-Funcionais guiam as decisões técnicas relacionadas à arquitetura e banco de dados.

---

## REFERÊNCIAS

SOMMERVILLE, I. Engenharia de Software. 9. ed. São Paulo: Pearson Prentice Hall, 2011.

---

Data: 2026-05-09  
Versão: 1.0  
Status: Aprovado

