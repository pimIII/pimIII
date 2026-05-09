# GUIA PARA CRIAR DIAGRAMAS UML
## Sistema de Gerenciamento de Estoque de Farmácia

---

## SUMÁRIO

1. Ferramentas Recomendadas
2. Diagrama 1: Diagrama de Caso de Uso
3. Diagrama 2: Diagrama de Classes
4. Diagrama 3: Diagrama de Sequência
5. Diagrama 4: Diagrama de Implantação
6. Checklist
7. Dicas Importantes
8. Estrutura de Armazenamento
9. Próximas Etapas
10. Referências

---

## 1 FERRAMENTAS RECOMENDADAS

### 1.1 Opção 1: Draw.io (Gratuita, Online)

Site: https://draw.io

Vantagens: Grátis, online, não precisa instalar

Desvantagem: Precisa de internet

### 1.2 Opção 2: Lucidchart (Gratuita com limite)

Site: https://lucidchart.com

Vantagens: Interface profissional

Desvantagem: Limite de diagramas grátis

### 1.3 Opção 3: StarUML (Pago, mas tem versão trial)

Site: http://staruml.io

Vantagens: Feito especificamente para UML

Desvantagem: Pago após trial

### 1.4 Opção 4: Visual Studio Code + Extensão Mermaid

Extensão: Mermaid Diagram

Vantagens: Direto no código, sem instalar programas

Desvantagem: Precisa aprender sintaxe Mermaid

### 1.5 Recomendação

Use Draw.io (é mais fácil e visual)

---

## 2 DIAGRAMA 1: DIAGRAMA DE CASO DE USO

### 2.1 Objetivo

Representar quem (Atores) faz o quê (Casos de Uso) no sistema.

### 2.2 Elementos Principais

Atores (bonecos de palito): Gerente, Farmacêutico, Atendente, Estoquista

Casos de Uso (elipses): Login, Cadastrar Funcionário, Registrar Venda, etc

Relacionamentos: linhas conectando atores aos casos de uso

### 2.3 Como criar no Draw.io:

1. Criar retângulo para delimitador (Sistema): Desenhe um retângulo grande chamado "Sistema de Estoque"

2. Adicionar Atores (fora do retângulo): Coloque figuras de bonecos de palito (procura por "actor" no Draw.io). Nomes: Gerente, Farmacêutico, Atendente, Estoquista

3. Adicionar Casos de Uso (dentro do retângulo): Desenhe elipses com os nomes:
   - UC01: Login
   - UC02: Cadastrar Funcionário
   - UC03: Listar Funcionários
   - UC04: Gerenciar Produtos
   - UC05: Registrar Venda
   - UC06: Consultar Estoque
   - UC07: Gerar Relatórios
   - UC08: Gerenciar Fornecedores

4. Conectar Atores aos Casos de Uso: Desenhe linhas associando:
   - Gerente → Todos os casos de uso
   - Farmacêutico → Login, Registrar Venda, Consultar Estoque
   - Atendente → Login, Registrar Venda, Consultar Estoque
   - Estoquista → Login, Gerenciar Produtos, Consultar Estoque

### 2.4 Exemplo visual

```
                    ┌─────────────────────────────────────┐
                    │  SISTEMA DE ESTOQUE FARMÁCIA       │
                    │                                     │
   ┌─ Gerente ──────┼─→ UC01: Login                      │
   │                │                                     │
   │                │─→ UC02: Cadastrar Funcionário      │
   │                │                                     │
Farmacêutico ───────┼─→ UC03: Registrar Venda            │
   │                │                                     │
   │                │─→ UC04: Consultar Estoque          │
   │                │                                     │
Atendente ──────────┼─→ UC05: Gerar Relatórios           │
   │                │                                     │
   │                │─→ UC06: Gerenciar Produtos         │
   │                │                                     │
Estoquista ─────────┼─→ UC07: Gerenciar Fornecedores    │
                    │                                     │
                    └─────────────────────────────────────┘
```

---

## 3 DIAGRAMA 2: DIAGRAMA DE CLASSES

### 3.1 Objetivo

Representar a estrutura das classes, propriedades, métodos e relacionamentos.

### 3.2 Classes Principais

Funcionário: Id, Nome, CPF, Cargo, SenhaHash, DataAdmissao, Ativo, DataDemissao

Produto: Id, NomeProduto, PrecoVenda, PrecoCusto, IdFornecedor, RequerReceita

Fornecedor: Id, NomeEmpresa, Cnpj, Telefone

Lote: Id, IdProduto, NumeroLote, DataValidade, Quantidade

Venda: Id, IdFuncionario, DataVenda, ValorTotal, FormaPagamento

Estoque: Gerencia os lotes de produtos

### 3.3 Como criar no Draw.io:

1. Para cada classe, desenhe um retângulo dividido em 3 partes:

```
┌─────────────────────────────┐
│ Funcionário                 │ ← Nome da classe
├─────────────────────────────┤
│ - Id: int                   │
│ - Nome: string              │ ← Propriedades (atributos)
│ - CPF: string               │
│ - Cargo: string             │
│ - SenhaHash: string         │
│ - DataAdmissao: DateTime    │
│ - Ativo: bool               │
│ - DataDemissao: DateTime?   │
├─────────────────────────────┤
│ + Salvar()                  │
│ + ListarTodos()             │ ← Métodos
│ + BuscarPorId(id)           │
│ + Atualizar()               │
│ + Inativar()                │
└─────────────────────────────┘
```

2. Desenhar relacionamentos entre classes:
   - Seta simples (→) para agregação (um tem um)
   - Seta preenchida (▶) para composição (deve ter um)
   - Linha simples (-) para associação

3. Relacionamentos principais:
   - Produto → Fornecedor (Um Fornecedor tem MUITOS Produtos)
   - Venda → Funcionário (Uma Venda foi feita por UM Funcionário)
   - Venda → Produto (Uma Venda contém MUITOS Produtos)
   - Lote → Produto (Um Produto tem MUITOS Lotes)
   - Estoque → Lote (Estoque controla os Lotes)

### 3.4 Notação UML para relacionamentos:

```
Classe A → Classe B  (A tem um B)
1 * (A tem 1, B tem muitos)

Exemplo:
Fornecedor ──────── 1 ─→ * ─────── Produto
(Um fornecedor tem MUITOS produtos)
```

---

## 4 DIAGRAMA 3: DIAGRAMA DE SEQUÊNCIA

### 4.1 Objetivo

Representar a ordem de interação entre objetos para realizar uma operação.

### 4.2 Exemplo: Registrar Venda

```
Atendente     UI              Service         Banco
   │           │                 │            │
   │─Registrar Venda─→│          │            │
   │           │─Validar Estoque→│            │
   │           │                 │            │
   │           │                 │─Buscar Lotes─→│
   │           │                 │←─Retorna Lotes│
   │           │                 │            │
   │           │                 │─Validar Data→│
   │           │                 │←─OK─────────│
   │           │                 │            │
   │           │                 │─Reduzir Estoque─→│
   │           │                 │←─Atualizado────│
   │           │                 │            │
   │           │←─Venda OK───────│            │
   │←─"Venda #123"──│           │            │
   │           │                 │            │
```

### 4.3 Como criar no Draw.io:

1. Desenhe colunas (uma para cada ator/classe):
   - Atendente
   - FuncionárioUI
   - VendaService
   - Banco de Dados

2. Desenhe linhas pontilhadas para cada coluna (lifeline)

3. Desenhe setas entre colunas com labels:
   - Seta simples: chamada de método
   - Seta tracejada: retorno

4. Exemplo do caso "Registrar Venda":

   ```
   1. Atendente → UI: "Registrar Venda"
   2. UI → Service: "Validar(produto, quantidade)"
   3. Service → BD: "Buscar Estoque(idProduto)"
   4. BD → Service: "Retorna Lote [...]"
   5. Service → Service: "Validar Validade"
   6. Service → BD: "Atualizar Estoque(...)"
   7. BD → Service: "OK"
   8. Service → UI: "Retorna true"
   9. UI → Atendente: "Venda Realizada #123"
   ```

---

## 5 DIAGRAMA 4: DIAGRAMA DE IMPLANTAÇÃO

### 5.1 Objetivo

Representar como o sistema é instalado e as máquinas envolvidas.

### 5.2 Componentes Principais

Cliente (Máquina do Usuário):
- Sistema de Estoque (aplicativo .exe)
- .NET Runtime

Servidor (Máquina do Servidor):
- SQL Server
- Backup automatizado

Rede:
- Conexão entre cliente e servidor

### 5.3 Como criar no Draw.io:

```
┌─────────────────────────┐          ┌──────────────────────────┐
│   MÁQUINA DO CLIENTE    │          │  SERVIDOR DE BANCO       │
│  (Gerente/Estoquista)   │          │  (Sala de Servidores)    │
│                         │          │                          │
│ ┌─────────────────────┐ │          │ ┌──────────────────────┐ │
│ │ App Estoque.exe     │ │          │ │ SQL Server 2019      │ │
│ │ .NET 6.0            │ │          │ │ (Banco de Dados)     │ │
│ └─────────────────────┘ │          │ └──────────────────────┘ │
│         │               │          │         │                │
│         │ HTTP/TCP 1433 │←────────→│         │                │
│         │               │          │         │                │
│   Windows 10/11         │          │  Windows Server 2019     │
│                         │          │                          │
└─────────────────────────┘          └──────────────────────────┘
         │                                      │
         │          Rede Local / Internet       │
         │─────────────────────────────────────│
         │                                      │
   Impressora                            Backup Externo
   (relatórios)                          (diário)
```

### 5.4 Elementos do Diagrama

Nós (caixas 3D): Máquinas/dispositivos

Artefatos: Software/aplicações dentro dos nós

Conectores: Conexões entre componentes

---

## 6 CHECKLIST: O QUE INCLUIR EM CADA DIAGRAMA

### 6.1 Diagrama de Caso de Uso

- [ ] Retângulo do sistema
- [ ] 4 Atores (Gerente, Farmacêutico, Atendente, Estoquista)
- [ ] Mínimo 7 casos de uso
- [ ] Relacionamentos entre atores e casos de uso
- [ ] Título e legenda

### 6.2 Diagrama de Classes

- [ ] 6 classes principais (Funcionário, Produto, Fornecedor, Lote, Venda, Estoque)
- [ ] Propriedades (atributos) de cada classe
- [ ] Métodos (operações) principais
- [ ] Relacionamentos entre classes (setas)
- [ ] Multiplicidade (1, *, 1..*, etc)
- [ ] Título e legenda

### 6.3 Diagrama de Sequência

- [ ] Atores/objetos nas colunas
- [ ] Lifelines (linhas pontilhadas)
- [ ] Mensagens entre objetos (setas)
- [ ] Retornos (setas tracejadas)
- [ ] Numeração das mensagens
- [ ] Título (qual caso de uso é)

### 6.4 Diagrama de Implantação

- [ ] Máquina cliente
- [ ] Servidor de banco de dados
- [ ] Conectores de rede
- [ ] Software em cada máquina
- [ ] Periféricos (impressora, backup)
- [ ] Título e legenda

---

## 7 DICAS IMPORTANTES

1. Use cores: Cada tipo de elemento com cor diferente
2. Use fontes legíveis: Tamanho mínimo 12pt
3. Não misture diagramas: Um diagrama = uma coisa
4. Seja consistente: Mesma notação em todos os diagramas
5. Documente: Adicione legenda explicando símbolos
6. Valide: Mostre para alguém do grupo revisar antes de enviar
7. Exporte: Salve em PNG/PDF além do Draw.io (para documentação)

---

## 8 ESTRUTURA DE ARMAZENAMENTO

Salve os arquivos em:

```
docs/
├── UML/
│   ├── 01_Diagrama_Caso_de_Uso.png (ou .pdf)
│   ├── 02_Diagrama_Classes.png
│   ├── 03_Diagrama_Sequencia_Venda.png
│   ├── 04_Diagrama_Implantacao.png
│   └── Arquivo_Draw_io.drawio (arquivo editável)
```

---

## 9 PRÓXIMAS ETAPAS

1. Fazer Diagrama de Caso de Uso
2. Fazer Diagrama de Classes
3. Fazer Diagrama de Sequência (mínimo 2 casos de uso)
4. Fazer Diagrama de Implantação
5. Exportar todos como PNG/PDF
6. Salvar na pasta docs/UML/
7. Fazer commit e push

---

## 10 REFERÊNCIAS

Lucidchart: Tipos de Diagrama UML. Disponível em: https://www.lucidchart.com/pages/uml-diagram-types

Tutorialspoint: UML Tutorial. Disponível em: https://www.tutorialspoint.com/uml/

---

Data: 2026-05-09  
Versão: 1.0  
Status: Aprovado

