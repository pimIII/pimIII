# Levantamento de Requisitos

Este documento apresenta o levantamento de requisitos para o sistema de controle de estoque da farmácia (projeto PIM).

## Objetivo
Gerenciar produtos, fornecedores, funcionários, lotes e vendas de uma farmácia, com interface consolar para operação básica.

## Stakeholders
- Dono/Gerente da farmácia
- Farmacêutico
- Atendente
- Administrador do sistema

---

## Requisitos Funcionais (RF)
RF01 - Autenticação
- O sistema deverá permitir login de usuários (admin) para acessar funcionalidades administrativas.

RF02 - Gerenciar Produtos
- Cadastrar produto com nome, preço de venda, preço de custo, id do fornecedor e indicar se requer receita.
- Listar produtos cadastrados.
- Remover produto por ID.

RF03 - Gerenciar Fornecedores
- Cadastrar fornecedor (nome da empresa, CNPJ, telefone).
- Listar fornecedores.
- Remover fornecedor por ID.

RF04 - Gerenciar Funcionários
- Cadastrar/Editar/Remover funcionário com dados básicos (nome, CPF, cargo, salário, data de admissão).
- Validar CPF e regras de domínio (campos obrigatórios, salário >= 0, data válida).

RF05 - Gerenciar Lotes
- Registrar lote associado a um produto, com número de lote, validade e quantidade.

RF06 - Registrar Vendas
- Registrar vendas com itens (produto, quantidade, preço), funcionário responsável e forma de pagamento.

RF07 - Relatórios básicos
- Gerar listagens de produtos, fornecedores e funcionários.

---

## Requisitos Não Funcionais (RNF)
RNF01 - Portabilidade
- O sistema deverá ser executável em Windows com .NET instalado.

RNF02 - Usabilidade
- Interface de texto simples e orientada por menus.

RNF03 - Segurança
- Senha mínima para login administrativo (implementação atual: login estático para protótipo).

RNF04 - Manutenibilidade
- Código documentado com comentários XML para facilitar manutenção e geração de documentação.

RNF05 - Performance
- Operações em memória para protótipo; deve responder em tempo interativo para listas pequenas.

RNF06 - Persistência
- Provisório: armazenamento em memória. Futuro: persistência por banco de dados relacional.

---

## Prioridades
1. Autenticação e menus básicos
2. CRUD de produtos e fornecedores
3. CRUD de funcionários com validação
4. Lotes e vendas
5. Relatórios e persistência


<!-- Este arquivo contém os requisitos em português e estruturados para uso no relatório do PIM. -->