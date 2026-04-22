README TEMPORÁRIO.

# 💊 Sistema de Gerenciamento de Estoque - PIM III

Repositório central para o desenvolvimento do sistema de estoque de farmácia (CRUD).

## 📂 Estrutura do Projeto

- **/backend**: Contém a solução Back-end, lógica do sistema em C# e os scripts SQL para criação e manutenção do banco de dados.
- **/frontend**: Contém toda a estrutura visual do projeto, protótipos de UI/UX e front com HTML-CSS.
- **/docs**: Documentação do projeto e requisitos.

---

## 🚀 Guia Rápido para o Time

Siga estes passos para configurar o projeto na sua máquina:

### 1. Clonar o Repositório
Abra o terminal na pasta onde deseja salvar o projeto e digite:
git clone https://github.com/pimiii/pimIII.git

### 2. Acessar a Pasta do Código e Rodar
cd pimIII/c_sharp/estoque_farmacia
dotnet restore
dotnet run

---

## 🛠️ Regras de Colaboração (Git)

Para mantermos o código organizado e evitar conflitos:

1. **Sempre dê git pull antes de começar:** Isso evita que você tente subir algo baseado em uma versão antiga.
2. **Não suba pastas de compilação:** O arquivo .gitignore já ignora as pastas bin e obj. Não force o envio delas.
3. **Commits Claros:** Use mensagens que expliquem o que foi feito (Ex: git commit -m "Criada a classe Medicamento").
4. **Trabalho em Dupla no Back:** Dividam os arquivos para não editarem a mesma linha ao mesmo tempo.

---

## 🗄️ Banco de Dados

Os scripts de criação estão em /database. Antes de rodar o sistema, execute o script script_criacao.sql no seu SQL Server local.

---

## 🎨 Protótipo UI/UX (Entregáveis)

O sistema deve apresentar as seguintes funcionalidades/telas:
1. **Login:** Acesso restrito para funcionários.
2. **Dashboard:** Indicadores de produtos com estoque baixo ou vencendo.
3. **Inventário:** Tabela geral para visualização e exclusão de itens.
4. **Cadastro:** Tela para inserção de novos medicamentos.
5. **Movimentação:** Registro de entradas (compras) e saídas (vendas).
