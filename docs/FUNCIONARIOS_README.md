#  GUIA EDUCATIVO - MÓDULO DE FUNCIONÁRIOS

---

##  ÍNDICE
1. [Visão Geral](#visão-geral)
2. [Arquitetura em Camadas](#arquitetura-em-camadas)
3. [Estrutura das Classes](#estrutura-das-classes)
4. [Como Funciona Junto](#como-funciona-junto)
5. [Conceitos Importantes](#conceitos-importantes)
6. [Exemplos de Uso](#exemplos-de-uso)

---

##  VISÃO GERAL

Este módulo implementa o gerenciamento completo de **Funcionários** da farmácia usando o padrão de **3 camadas**:

```
USUÁRIO
  
[UI] FuncionarioUI.cs  O que o usuário VÊ (menus, formulários)
  
[SERVICE] FuncionarioService.cs  A LÓGICA de negócio (CRUD, validações)
  
[MODEL] Funcionario.cs  A ESTRUTURA dos dados (propriedades)
```

---

## ️ ARQUITETURA EM CAMADAS

### Por que 3 camadas?

**SEPARAÇÃO DE RESPONSABILIDADES** = cada coisa tem um trabalho específico

| Camada | Arquivo | Responsabilidade | Exemplo |
|--------|---------|------------------|---------|
| **MODEL** | `Funcionario.cs` | Define ESTRUTURA dos dados | Que propriedades um funcionário tem? (Nome, CPF, Cargo, etc) |
| **SERVICE** | `FuncionarioService.cs` | Define LÓGICA de negócio | Como salvar? Como validar? Como listar? |
| **UI** | `FuncionarioUI.cs` | Define APRESENTAÇÃO | Como mostrar menus? Como pedir dados ao usuário? |

### Vantagem 1: Manutenção Fácil
Se você quer mudar **como mostra** os funcionários (console  Web),
muda APENAS a UI. Service e Model continuam os mesmos!

### Vantagem 2: Testabilidade
Você pode testar a lógica (Service) **independente** da interface.

### Vantagem 3: Reutilização
O mesmo Service pode ser usado em:
- Console (atual)
- Web API (futuro)
- App Mobile (futuro)

---

##  ESTRUTURA DAS CLASSES

### 1️⃣ MODEL: Funcionario.cs

**O que é?** Uma "ficha" ou "formulário" que define que informações guardamos sobre cada funcionário.

**Propriedades:**
```csharp
public int Id { get; set; }              // ID único (1, 2, 3...)
public string Nome { get; set; }         // "João Silva"
public string CPF { get; set; }          // "123.456.789-00"
public string Cargo { get; set; }        // "Gerente", "Farmacêutico", etc
public string SenhaHash { get; set; }    // Senha criptografada
public DateTime DataAdmissao { get; set; } // Quando foi contratado
public bool Ativo { get; set; }          // true = trabalhando, false = saiu
public DateTime? DataDemissao { get; set; } // Quando saiu (pode ser vazio)
```

**Get/Set?**
```csharp
public string Nome { get; set; }
                    
      tipo      get  set

get  = permite LER o valor
set  = permite ESCREVER o valor
```

---

### 2️⃣ SERVICE: FuncionarioService.cs

**O que é?** A LÓGICA. Aqui acontecem as operações (salvar, listar, atualizar, deletar).

**Atributo:**
```csharp
private readonly List<Funcionario> listaFuncionarios = new List<Funcionario>();
                                   
      privado  protegido         armazena
                                   dados
```
- `private` = só Esta classe pode acessar
- `readonly` = não pode ser mudado depois
- `List<Funcionario>` = lista de funcionários em MEMÓRIA

**Métodos Principais:**

####  SALVAR (CREATE)
```csharp
public bool Salvar(Funcionario novoFuncionario)
```
- **O que faz?** Adiciona um novo funcionário à lista
- **Valida?** Sim! Verifica nome, CPF, duplicatas
- **Retorna?** true (sucesso) ou false (erro)

####  LISTAR (READ)
```csharp
public List<Funcionario> ListarTodos()
public List<Funcionario> ListarAtivos()
```
- **O que faz?** Retorna funcionários da lista
- `ListarTodos()` = todos (ativos + inativos)
- `ListarAtivos()` = só quem está trabalhando

####  BUSCAR (READ ONE)
```csharp
public Funcionario BuscarPorId(int id)
```
- **O que faz?** Encontra um funcionário específico pelo ID
- **Retorna?** O funcionário (ou null se não encontrar)

#### ️ ATUALIZAR (UPDATE)
```csharp
public bool Atualizar(Funcionario funcionarioAtualizado)
```
- **O que faz?** Muda os dados de um funcionário existente
- **O que NÃO muda?** O ID (é fixo, único)

####  INATIVAR/REMOVER (DELETE)
```csharp
public bool Inativar(int id)   // Marca como inativo (recomendado)
public bool Remover(int id)    // Remove totalmente (perigoso)
```
- **Inativar?** Marca `Ativo = false` + registra data
- **Remover?** Deleta fisicamente da lista

---

### 3️⃣ UI: FuncionarioUI.cs

**O que é?** A INTERFACE. O que o usuário VÊ e INTERAGE.

**Estrutura:**

1. **Constructor**
```csharp
public FuncionarioUI(FuncionarioService funcionarioService)
{
    this._funcionarioService = funcionarioService;
}
```
- Recebe o Service pelo construtor (INJEÇÃO DE DEPENDÊNCIA)
- Assim o UI usa o Service sem criar um novo

2. **Menu Principal**
```csharp
public void ProcessarMenuFuncionario()
```
- Loop infinito que mostra opções
- Chamar Services conforme a opção
- Mostra resultados ao usuário

3. **Opções (6 métodos)**

| Método | Ação |
|--------|------|
| `CadastrarFuncionario()` | Pede dados e chama `Service.Salvar()` |
| `ListarTodosFuncionarios()` | Chama `Service.ListarTodos()` e mostra em tabela |
| `BuscarFuncionarioPorId()` | Chama `Service.BuscarPorId()` e mostra detalhes |
| `AtualizarFuncionario()` | Pede dados novos e chama `Service.Atualizar()` |
| `InativarFuncionario()` | Pede confirmação e chama `Service.Inativar()` |
| `PauseMenu()` | Aguarda Enter do usuário |

---

##  COMO FUNCIONA JUNTO

### Fluxo: Cadastrar um Funcionário

```
1. USUÁRIO escolhe opção "1" no menu
           
2. UI chama CadastrarFuncionario()
           
3. UI pede ao usuário: Nome, CPF, Cargo, Senha
           
4. UI VALIDA: não é nulo? está preenchido?
           
5. UI cria um NOVO Funcionario()
   {
     Nome = "João",
     CPF = "123.456.789-00",
     Cargo = "Farmacêutico",
     SenhaHash = "abc123xyz"
   }
           
6. UI chama: _funcionarioService.Salvar(novoFuncionario)
           
7. SERVICE:
    Valida novamente (CPF já existe?)
    Atribui ID único (próximo ID)
    Define DataAdmissao = hoje
    Define Ativo = true
    Adiciona à lista
    Incrementa próximo ID
           
8. SERVICE retorna true (sucesso)
           
9. UI mostra: " Funcionário 'João' salvo com sucesso! ID: 1"
           
10. UI aguarda Enter do usuário
           
11. Volta ao menu principal
```

---

##  CONCEITOS IMPORTANTES

### 1. PROPERTIES (get; set;)

```csharp
public string Nome { get; set; }
```

**Segurança:**
```csharp
//  RUIM (variável pública)
public string Nome;  // qualquer código pode mexer sem validação

//  BOM (property)
public string Nome { get; set; }  // pode adicionar validação depois
```

**Pode evoluir assim:**
```csharp
// HOJE (simples)
public string Nome { get; set; }

// DEPOIS (com validação)
private string _nome;
public string Nome
{
    get { return _nome; }
    set
    {
        if (value.Length < 3) throw new Exception("Nome muito curto!");
        _nome = value;
    }
}
```

### 2. LINQ - Language Integrated Query

```csharp
// Filtrar apenas os ativos
var ativos = listaFuncionarios.Where(f => f.Ativo == true).ToList();
                                     
                            "para cada f em listaFuncionarios"

// Contar quantos são ativos
int total = listaFuncionarios.Count(f => f.Ativo == true);

// Encontrar o primeiro
var primeiro = listaFuncionarios.FirstOrDefault(f => f.Id == 1);
```

### 3. NULLABLE (pode ser vazio)

```csharp
// Normal - DEVE ter valor
DateTime dataNascimento = new DateTime(1990, 5, 15);

// Nullable - pode ser null (vazio)
DateTime? dataDemissao = null;

// Depois, você coloca um valor:
dataDemissao = DateTime.Now;

// Para verificar:
if (dataDemissao.HasValue)
{
    Console.WriteLine(dataDemissao.Value); // DateTimeKind = Local
}
```

### 4. TRY/CATCH - Tratamento de Erros

```csharp
try
{
    // código que pode dar erro
    int numero = int.Parse("abc"); // ERRO aqui!
}
catch (Exception ex)
{
    // se deu erro, faz algo
    Console.WriteLine($"Erro: {ex.Message}");
}
```

### 5. INT.TRYPARSE - Conversão Segura

```csharp
// Perigoso:
int id = int.Parse("123");  // OK, vira 123
int id = int.Parse("abc");  // ERRO! Programa cai

// Seguro:
if (int.TryParse("123", out int id))
{
    Console.WriteLine($"ID: {id}"); // OK
}
else
{
    Console.WriteLine("ID inválido!");
}
```

### 6. OPERADOR TERNÁRIO

```csharp
// if/else tradicional:
if (funcionario.Ativo == true)
{
    Console.WriteLine("Ativo");
}
else
{
    Console.WriteLine("Inativo");
}

// Ternário (mais conciso):
string status = funcionario.Ativo ? "Ativo" : "Inativo";
                                             
                          condição  true    false
```

---

##  EXEMPLOS DE USO

### Exemplo 1: Criar e Salvar um Funcionário

```csharp
// Criar um novo funcionário
Funcionario novoFunc = new Funcionario
{
    Nome = "Maria Silva",
    CPF = "987.654.321-00",
    Cargo = "Atendente",
    SenhaHash = "senha123"
};

// Usar o Service para salvar
FuncionarioService service = new FuncionarioService();
bool sucesso = service.Salvar(novoFunc);

if (sucesso)
{
    Console.WriteLine($"Funcionário criado com ID: {novoFunc.Id}");
    // Output: Funcionário criado com ID: 1
}
```

### Exemplo 2: Listar Todos

```csharp
var todos = service.ListarTodos();

foreach (var func in todos)
{
    Console.WriteLine($"{func.Id} - {func.Nome} ({func.Cargo})");
}
// Output:
// 1 - Maria Silva (Atendente)
// 2 - João Santos (Farmacêutico)
```

### Exemplo 3: Buscar e Atualizar

```csharp
// Buscar
Funcionario func = service.BuscarPorId(1);

if (func != null)
{
    // Atualizar
    func.Cargo = "Gerente"; // mudou de Atendente para Gerente
    service.Atualizar(func);
}
```

### Exemplo 4: Inativar

```csharp
// Em vez de deletar:
service.Inativar(1); // marca como inativo, mas mantém no banco
```

---

##  RESUMO

| Coisa | Arquivo | Responsabilidade |
|-------|---------|------------------|
| **Model** | `Funcionario.cs` | Propriedades (Nome, CPF, etc) |
| **Service** | `FuncionarioService.cs` | CRUD + Validações |
| **UI** | `FuncionarioUI.cs` | Menus + Interação com usuário |

**Fluxo:** Usuário  UI  Service  Model  Lista em Memória

---

##  Próximos Passos (Para o Grupo)

1. **Banco de Dados:** Conectar ao PostgreSQL (em vez de lista em memória)
2. **ORM:** Usar Entity Framework para simplificar as queries
3. **Validações:** Adicionar máscaras de CPF, validação de email, etc
4. **Hash de Senha:** Usar BCrypt ou PBKDF2 para segurança real
5. **Testes:** Escrever testes unitários para cada método

---

**Para:** Grupo de PIM III - Farmácia  
**Nível:** Educativo (para quem nunca programou)

