# Sistema de Estoque Farmácia (Pharmastock)

Sistema de gestão de estoque para farmácia, desenvolvido em C# com **.NET 10.0**. Os dados são mantidos **em memória** pelos serviços (listas estáticas): não há banco de dados nem ORM na aplicação em execução; ao fechar o programa, os cadastros são perdidos. Material SQL e documentação de apoio ficam em `backend/database` e `docs`.

O projeto possui duas interfaces:

- **Aplicação Console** (`estoque_farmacia`) — menus em terminal (marca **PHARMASTOCK** no login).
- **Aplicação Windows Forms** (`estoque_farmacia_winforms`) — desktop **Pharmastock** com login, menu e telas de cadastro e venda.

Ambas compartilham os mesmos **Models** e **Services** (o projeto WinForms referencia os arquivos do projeto console via `Link` no `.csproj`).

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 10.0 SDK** ou superior  
  - Download: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

- **Windows** (para rodar o projeto **Windows Forms** — alvo `net10.0-windows`). O projeto console é `net10.0` e pode ser compilado em outros sistemas, mas o repositório está focado no fluxo Windows + WinForms.

## Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/pimIII/pimIII.git
cd pimIII
```

(Ajuste a URL e o nome da pasta se o remoto for outro.)

### 2. Restaurar dependências

Na pasta da solução C#:

```bash
cd backend/c_sharp/estoque_farmacia
dotnet restore estoque_farmacia.sln
```

Não é necessário criar banco de dados, configurar connection string nem rodar migrations para executar o código atual.

## Executando a aplicação

### Aplicação Console

```bash
cd backend/c_sharp/estoque_farmacia
dotnet run
```

### Aplicação Windows Forms

```bash
cd backend/c_sharp/estoque_farmacia_winforms
dotnet run
```

### Credenciais padrão (login)

- **Usuário:** `admin`  
- **Senha:** `123`  

O login fixo vale para console e WinForms (até três tentativas incorretas).

## Compilando um executável

```bash
# Console
cd backend/c_sharp/estoque_farmacia
dotnet publish -c Release -o ./publish

# Windows Forms
cd backend/c_sharp/estoque_farmacia_winforms
dotnet publish -c Release -o ./publish
```

## Estrutura do repositório

```
pimIII/   (raiz do clone)
├── README.md
├── LICENSE
├── .gitignore
├── backend/
│   ├── c_sharp/
│   │   ├── c_sharp.slnx
│   │   ├── estoque_farmacia/              # Projeto Console (.NET 10)
│   │   │   ├── Models/                    # Classes de domínio
│   │   │   ├── Services/                  # Regras de negócio (armazenamento em memória)
│   │   │   ├── UI/                        # Telas em texto (Console)
│   │   │   ├── Program.cs
│   │   │   ├── estoque_farmacia.csproj
│   │   │   └── estoque_farmacia.sln       # Solução (console + WinForms)
│   │   └── estoque_farmacia_winforms/     # Projeto Windows Forms (.NET 10, Windows)
│   │       ├── Forms/                     # Login, menu, CRUDs, venda
│   │       ├── UIHelper.cs                # Cores e estilo dos controles
│   │       ├── Program.cs
│   │       └── estoque_farmacia_winforms.csproj
│   └── database/                          # Scripts e notas SQL (apoio acadêmico)
├── docs/                                  # Documentação, HTML, atividade de extensão
└── ...
```

## Funcionalidades

- **Login** com usuário e senha (`admin` / `123`).
- **Cadastro de produtos** (nome, código de barras numérico, preços, fornecedor, receita).
- **Cadastro de funcionários** (nome, CPF, cargo); no **WinForms**, a senha informada é armazenada com **hash SHA-256**; no console, o fluxo de cadastro trata a senha como texto conforme a UI.
- **Cadastro de fornecedores** e **lotes** vinculados a produto existente.
- **Registro de vendas** (carrinho, desconto, total) na interface **Windows Forms**.
- No **Console**, a opção de menu **“Controle de Venda”** (`[3]`) ainda **não está implementada** (sem ação).

## Tecnologias utilizadas

- **.NET 10.0** — plataforma  
- **C#** — linguagem  
- **Windows Forms** — interface desktop (projeto `estoque_farmacia_winforms`)

## Persistência e dados

- Os serviços usam **coleções em memória** compartilhadas durante uma única execução do processo.  
- **Console** e **WinForms** são processos separados: cada um mantém seu próprio estado ao rodar ao mesmo tempo.  
- Para modelagem e SQL de referência (tabelas, integridade, relatórios), use os arquivos em `backend/database/` e a documentação em `docs/`.

## Solução de problemas

### Erro ao restaurar ou compilar: SDK não encontrado

**Causa:** o SDK do .NET 10 não está instalado ou não está no PATH.

**Solução:** instale o [.NET 10 SDK](https://dotnet.microsoft.com/download) e confira com `dotnet --version`.

### WinForms não abre em Linux/macOS

**Causa:** o projeto WinForms exige Windows (`net10.0-windows`).

**Solução:** use uma máquina Windows ou compile apenas o projeto console, se atender ao seu cenário.

### Dados “somem” ao fechar o app

**Causa:** comportamento esperado — não há persistência em arquivo ou banco na aplicação atual.

**Solução:** nenhuma; para persistir seria necessário evoluir o projeto (por exemplo, arquivo ou banco de dados).

## Autores

Desenvolvido como projeto acadêmico (PIM III) pelos seguintes integrantes:

- Anderson da Silva Araújo  
- Daniel Reis Oliveira  
- Eduardo Goulart Pessini  
- Geovanna Xavier Alves  
- Geovanni da Silva de Andrade  
- Patrick Hugo Costa Freitas  
- Roberto Matheus da Costa Domingues Martins  
- Vinicius Umeda de Araujo  

## Licença

MIT License
