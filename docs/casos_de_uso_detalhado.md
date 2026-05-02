# Casos de Uso Detalhados (Completo)

Documento com casos de uso, atores, fluxos principais, fluxos alternativos e exceções para o sistema PIM (estoque de farmácia). Baseado no manual de instalação e nos documentos de requisitos.

Atores:
- Administrador (Gerente): gestão completa (funcionários, relatórios, configurações).
- Farmacêutico: gerência de produtos, lote, vendas, acesso a receitas.
- Atendente: registrar vendas, consultar produtos, cadastrar fornecedores básicos.
- Sistema (sub-sistema de persistência): serviço responsável por armazenar/dar persistência aos dados (quando houver BD).

-----------------------------------------------------------------
UC01 - Autenticar Usuário (Login)
- Ator primário: Qualquer usuário que acesso a área autenticada.
- Pré-condição: Aplicação em execução.
- Pós-condição: Sessão autenticada ou acesso negado.

Fluxo principal:
1. Usuário abre aplicação.
2. Sistema solicita login e senha.
3. Usuário informa credenciais.
4. Sistema valida credenciais (comparação com armazenado).
5. Sistema concede acesso ao menu apropriado conforme permissão.

Fluxos alternativos:
A1: Credenciais inválidas — sistema notifica e permite nova tentativa; após N tentativas pode encerrar (configurável).
A2: Usuário não autorizado para operação — exibe mensagem e retorna ao menu.

Exceções:
- Erro no subsistema de autenticação (BD indisponível) — exibir erro técnico e permitir operação limitada.

-----------------------------------------------------------------
UC02 - Cadastrar Produto (detalhado)
- Ator primário: Farmacêutico / Atendente
- Pré-condição: Usuário autenticado (ou modo local sem autenticação)
- Pós-condição: Produto cadastrado na coleção de produtos em memória ou BD.

Fluxo principal:
1. Ator escolhe opção "Cadastrar produto".
2. Sistema solicita Nome do produto.
3. Ator informa Nome (validação: não vazio, comprimento máximo).
4. Sistema solicita preço de venda (decimal positivo).
5. Ator informa preço de venda (validar formato e valor >= 0).
6. Sistema solicita preço de custo (decimal >= 0).
7. Ator informa preço de custo.
8. Sistema solicita ID do fornecedor ou oferece lista de fornecedores.
9. Ator informa ID do fornecedor (validar existência).
10. Sistema pergunta se produto requer receita (S/N ou 0/1).
11. Ator informa resposta.
12. Sistema chama ProdutoService.Salvar(produto).
13. Serviço valida regras de domínio (ex.: preços coerentes) e persiste.
14. Sistema confirma sucesso ao usuário.

Fluxos alternativos:
A1: Nome inválido — repete a solicitação até válido.
A2: Preço inválido — solicita reentrada.
A3: Fornecedor inexistente — oferta criação de novo fornecedor ou cancela o cadastro.
A4: Falha na persistência — exibe erro técnico, opção de salvar em rascunho.

Regras de negócio relevantes:
- Preço de venda não pode ser menor que zero.
- Preço de custo não pode ser negativo.
- Se requer receita, marcar produto como controlado.

-----------------------------------------------------------------
UC03 - Gerenciar Fornecedores
- Ator primário: Farmacêutico / Administrador
- Fluxo principal (Cadastrar): solicita nome, CNPJ, telefone; valida CNPJ (formato) e persiste.
- Fluxos alternativos: CNPJ inválido, CNPJ já cadastrado.

-----------------------------------------------------------------
UC04 - Cadastrar Funcionário (detalhado)
- Ator primário: Administrador (Gerente)
- Pré-condição: Administrador autenticado
- Pós-condição: Funcionário persistido com validações de domínio.

Fluxo principal:
1. Administrador escolhe "Cadastrar funcionário".
2. Sistema solicita Nome.
3. Administrador informa Nome.
4. Sistema solicita CPF.
5. Administrador informa CPF.
6. Sistema executa validação de CPF (algoritmo dos dígitos verificadores).
7. Se CPF inválido, sistema informa e retorna para passo 4 (fluxo alternativo A1).
8. Sistema solicita Cargo, Salário, Data de admissão.
9. Administrador informa dados.
10. Sistema valida domínio: Nome não vazio, CPF único, Salário >= 0, Data válida.
11. Chamada a FuncionarioService.CreateAsync(funcionario).
12. Serviço persiste o funcionário.
13. Sistema confirma sucesso.

Fluxos alternativos:
A1: CPF inválido — informar e permitir correção.
A2: CPF já cadastrado — informar e abortar operação ou oferecer edição do registro existente.
A3: Dados obrigatórios ausentes — solicitar preenchimento.

Tratamento de erros:
- Validações devolvem mensagens claras que a UI deve exibir.
- Exceções de integridade no BD devem ser capturadas e tratadas.

-----------------------------------------------------------------
UC05 - Registrar Venda (detalhado)
- Ator primário: Atendente / Farmacêutico
- Pré-condição: Produto(s) disponíveis em estoque
- Pós-condição: Venda registrada e estoque atualizado

Fluxo principal resumido:
1. Atendente inicia novo registro de venda.
2. Informa ID do funcionário que registrou a venda.
3. Para cada item: informar produto (ID), quantidade (verificar estoque), preço unitário (confirmar), adicionar ao carrinho.
4. Informar forma de pagamento.
5. Confirmação final e persistência da venda com itens.
6. Atualizar quantidades em lotes e em estoque.

Fluxos alternativos:
A1: Produto sem estoque suficiente — informar e impedir inclusão.
A2: Cancelamento pelo usuário — abortar processo e não persistir.

Regras:
- Descontos e impostos podem ser aplicados em etapas futuras.

-----------------------------------------------------------------
UC06 - Gerenciar Lotes
- Ator primário: Farmacêutico
- Funcionalidades: cadastrar lote (associado a produto), alterar quantidade, marcar validade.
- Regras: alertas para vencimento, validação de data de validade.

-----------------------------------------------------------------
Notas finais
- Para cada UC, a interface de console atual implementa fluxos principais e validações básicas. Em uma versão com BD/WEB, adicionar autenticação real, controle de permissões (roles) e persistência transacional.
- Os fluxos alternativos detalhados devem ser mapeados nos diagramas de sequência e testes de aceitação automatizados.

<!-- Documento em Português; leve e direto para utilização no relatório do PIM. -->