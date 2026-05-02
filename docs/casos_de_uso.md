# Casos de Uso Detalhados

Este documento detalha casos de uso principais do sistema, atores envolvidos, fluxo principal e fluxos alternativos. Baseado no manual de instalação e na modelagem do sistema.

Resumo dos principais casos de uso:
- UC01: Autenticar Usuário (Login)
- UC02: Gerenciar Produtos (Cadastrar, Listar, Remover)
- UC03: Gerenciar Fornecedores (Cadastrar, Listar, Remover)
- UC04: Gerenciar Funcionários (Cadastrar, Editar, Remover)
- UC05: Registrar Venda
- UC06: Gerenciar Lotes

---

UC02 - Gerenciar Produtos (Cadastrar Produto)

Ator primário: Atendente / Farmacêutico
Pré-condição: Usuário autenticado no sistema (ou execução local do protótipo)
Pós-condição: Produto adicionado ao repositório em memória

Fluxo principal:
1. Ator seleciona a opção "Cadastrar produto" no menu.
2. Sistema solicita Nome do produto.
3. Ator fornece Nome (validação: não pode ser vazio).
4. Sistema solicita Preço de venda (validação numérica).
5. Ator fornece Preço de venda.
6. Sistema solicita Preço de custo (validação numérica).
7. Ator fornece Preço de custo.
8. Sistema solicita ID do fornecedor (validação numérica).
9. Ator fornece ID do fornecedor.
10. Sistema solicita se requer receita (0/1).
11. Ator informa valor.
12. Sistema cria objeto Produto e chama ProdutoService.Salvar(produto).
13. Sistema exibe mensagem de sucesso.

Fluxos alternativos:
A1: Nome vazio — o sistema solicita novamente até preencher.
A2: Valor numérico inválido (preços ou ID) — o sistema exibe mensagem e solicita novamente.
A3: Fornecedor inexistente — (futuro) o sistema exibirá aviso e poderá solicitar criação do fornecedor ou confirmação.

Exceções:
- Erro de parse de decimal/int: tratado na UI pedindo reentrada.

Observações:
- Atualmente o armazenamento é em memória; ao reiniciar a aplicação os dados são perdidos.

---

UC04 - Gerenciar Funcionários (Criar funcionário)

Ator primário: Gerente / Administrador
Pré-condição: Usuário autenticado com permissão
Pós-condição: Funcionário persistido (em memória ou BD dependendo da implementação)

Fluxo principal:
1. Ator seleciona "Cadastrar funcionário" no menu.
2. Sistema solicita Nome.
3. Ator fornece Nome.
4. Sistema solicita CPF.
5. Ator fornece CPF.
6. Sistema valida CPF via algoritmo (verificação de dígitos).
7. Se inválido, sistema rejeita e informa o usuário (fluxo alternativo A1).
8. Sistema solicita Cargo, Salário, Data de admissão.
9. Ator fornece os dados (validações aplicadas: salário >= 0, data válida).
10. Sistema chama FuncionarioService.CreateAsync(funcionario).
11. Serviço valida domínio e persiste o funcionário.
12. Sistema exibe confirmação.

Fluxos alternativos:
A1: CPF inválido — CreateAsync lança ArgumentException e UI deve exibir erro e pedir correção.
A2: CPF já cadastrado — CreateAsync lança InvalidOperationException.
A3: Dados obrigatórios ausentes — CreateAsync lança ArgumentException.

---

UC05 - Registrar Venda (fluxo resumido)

Ator primário: Atendente / Farmacêutico
Fluxo principal:
1. Selecionar "Registrar venda".
2. Informar itens (produto e quantidade) repetidamente.
3. Confirmar venda, informar forma de pagamento.
4. Sistema calcula valor total e registra venda associando funcionário e itens.

Fluxos alternativos: item sem estoque suficiente — impedir inclusão e avisar o usuário.

---

Observação final:
Estes casos de uso são detalhados para o protótipo atual (console + lógica em memória). Para implantação com banco de dados, ajustar pré-condições e pós-condições (persistência durável).