# Manual de Instalação

Este manual descreve como instalar e executar o sistema de controle de estoque (protótipo PIM) no ambiente Windows.

Pré-requisitos
- .NET SDK/Runtime (recomendado .NET 8) — baixar em https://dotnet.microsoft.com/

Passos de instalação
1. Clonar o repositório:
   git clone <repo-url>
2. Navegar até o projeto de console:
   cd backend/c_sharp/estoque_farmacia
3. Restaurar dependências e construir:
   dotnet restore
   dotnet build
4. Executar a aplicação:
   dotnet run --project backend/c_sharp/estoque_farmacia/estoque_farmacia.csproj

Observações adicionais
- Para executar os testes (caso existam), utilize dotnet test na solução adequada.
- Se ocorrerem avisos sobre valores nulos, recomenda-se usar .NET 8 e revisar inicialização de strings e tratamento de Console.ReadLine().

<!-- Manual de instalação em Português conforme solicitado -->