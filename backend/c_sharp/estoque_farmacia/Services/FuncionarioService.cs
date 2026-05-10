using estoque_farmacia.Models;
using estoque_farmacia.Data;
using System.Collections.Generic;
using System.Linq;

namespace estoque_farmacia.Services;

/// <summary>
/// CLASSE FUNCIONARIOSERVICE (SERVICE/LÓGICA DE NEGÓCIO + DADOS)
/// ==============================================================
///
/// O QUE É UM SERVICE?
/// Service é uma classe que contém a LÓGICA DE NEGÓCIO.
/// Enquanto o Model define "O QUE SÃO os dados",
/// o Service define "O QUE FAZER COM os dados".
///
/// RESPONSABILIDADES DESTE SERVICE:
/// - Salvar novos funcionários no banco de dados
/// - Listar todos os funcionários (do banco de dados)
/// - Buscar um funcionário específico
/// - Atualizar informações de funcionário
/// - Remover/inativar funcionários
/// - Validar dados
///
/// POR QUE SEPARAR EM SERVICES?
/// - Deixa o código organizado e fácil de manter
/// - Cada classe tem UMA RESPONSABILIDADE (SOLID)
/// - Facilita testes (testamos a lógica separada da interface)
/// - Reutilizável (pode usar o mesmo Service em Web ou Desktop)
///
/// EVOLUÇÃO DO PADRÃO:
/// - ANTES: Em memória (List<Funcionario>) - dados perdidos ao fechar app
/// - AGORA: Entity Framework Core + PostgreSQL - dados persistem no banco
/// - RESULTADO: Dados reais, compartilhados, auditáveis
///
/// ---
///
/// MUDANÇA IMPORTANTE: List<T> → DbContext
///
/// COM List<T> (ANTES):
/// - Dados em RAM (memória)
/// - Dados perdidos quando app fecha
/// - Só funciona para UM usuário por vez
/// - Sem persistência
///
/// COM DbContext + PostgreSQL (AGORA):
/// - Dados em arquivo PostgreSQL
/// - Dados persistem para sempre (ou até deletar)
/// - Múltiplos usuários podem acessar simultaneamente
/// - Backup e segurança do banco de dados
///
/// Exemplo do fluxo agora:
/// 1. Usuario clica "Cadastrar Funcionário" na UI
/// 2. UI chama service.Salvar(novoFunc)
/// 3. Service valida dados
/// 4. Service chama context.Funcionarios.Add(novoFunc)
/// 5. Service chama context.SaveChanges() ← PERSISTE NO BANCO!
/// 6. EF Core gera e executa SQL INSERT automaticamente
/// 7. Banco de dados salva o funcionário
/// 8. Próxima execução do app, dados ainda estão lá!
/// </summary>
public class FuncionarioService
{
    /// <summary>
    /// Mensagem do ultimo erro ocorrido em qualquer operacao do service.
    /// A UI consulta esta propriedade para exibir o motivo real da falha
    /// quando Salvar/Atualizar/Inativar/Remover retornam false.
    /// </summary>
    public string UltimoErro { get; private set; } = string.Empty;

    // ATRIBUTO: Referência ao DbContext
    /// <summary>
    /// Referência ao DbContext - PONTE COM O BANCO DE DADOS
    ///
    /// O QUE É DbContext?
    /// - É a conexão entre código C# e banco de dados PostgreSQL
    /// - Gerencia entidades (objetos C#)
    /// - Rastreia mudanças (tracking)
    /// - Executa SaveChanges() para persistir dados
    ///
    /// POR QUE 'readonly'?
    /// - Não queremos trocar o DbContext durante a vida do objeto
    /// - Isso daria inconsistências
    /// - PADRÃO: Injetar via constructor e manter constante
    ///
    /// COMO FUNCIONA?
    /// this.context.Funcionarios.Add(novo)  → Marca como "Add" na memória (não salva ainda)
    /// this.context.SaveChanges()           → AGORA SIM salva no banco
    ///
    /// ANALOGIA:
    /// List<T> era como um "caderno local"
    /// DbContext é como um "gerente de banco que tem acesso ao arquivo central"
    /// SaveChanges() é como "enviar a alteração para o arquivo central"
    /// </summary>
    private readonly AppDbContext context;

    // ========================================================================
    // CONSTRUCTOR (INJEÇÃO DE DEPENDÊNCIA)
    // ========================================================================

    /// <summary>
    /// Constructor que recebe o DbContext via Dependency Injection.
    ///
    /// O QUE É INJEÇÃO DE DEPENDÊNCIA?
    /// É um padrão onde a classe recebe suas dependências (AppDbContext)
    /// ao invés de criar elas mesma.
    ///
    /// BENEFÍCIOS:
    /// - Mais fácil testar (pode usar DbContext fake para testes)
    /// - Mais flexível (pode trocar implementação sem mexer na classe)
    /// - Segue SOLID principles
    ///
    /// EXEMPLO:
    /// // Em Program.cs:
    /// services.AddDbContext<AppDbContext>(options =>
    ///     options.UseNpgsql(connectionString)
    /// );
    ///
    /// // Depois, ASP.NET automaticamente injeta AppDbContext no constructor
    /// var service = new FuncionarioService(context); // context é injetado
    ///
    /// ALTERNATIVA (Sem injeção):
    /// private AppDbContext context = new AppDbContext(); // PRATICA RUIM
    /// - Difícil testar
    /// - Tight coupling
    /// - Menos flexível
    /// </summary>
    public FuncionarioService(AppDbContext context)
    {
        // Recebe o DbContext e armazena como atributo privado
        // this.context = context é o mesmo que this._context = context
        // 'this' = referência ao objeto atual
        this.context = context;
    }

    // ========================================================================
    // MÉTODO: SALVAR (CREATE) - AGORA COM EF CORE
    // ========================================================================

    /// <summary>
    /// Salva (adiciona) um novo funcionário NO BANCO DE DADOS via EF Core.
    ///
    /// MUDANÇA IMPORTANTE (List → EF Core):
    /// ANTES: Adicionava à lista em memória - dados perdidos ao fechar
    /// AGORA: Adiciona ao banco de dados - dados PERSISTEM para sempre
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Valida se o funcionário não é nulo
    /// 2. Validações de dados (nome, CPF, etc)
    /// 3. Verifica se CPF já existe no BANCO (não na memória)
    /// 4. Define a data de admissão como hoje
    /// 5. Marca como ativo por padrão
    /// 6. Chama context.Funcionarios.Add() para marcar como novo
    /// 7. Chama context.SaveChanges() para PERSISTIR NO BANCO
    /// 8. Entity Framework Core:
    ///    a. Gera SQL INSERT automaticamente
    ///    b. Executa no PostgreSQL
    ///    c. Retorna o ID gerado (AUTO INCREMENT)
    ///
    /// FLUXO DETALHADO:
    /// context.Funcionarios.Add(novoFuncionario)  ← Marca como "Added" (em tracking)
    /// context.SaveChanges()                      ← Executa INSERT e gera ID!
    ///
    /// IMPORTANTE: SaveChanges()
    /// - Sem SaveChanges(), nada é gravado no banco
    /// - SaveChanges() executa transação (tudo ou nada)
    /// - Se erro em SaveChanges(), exceção é lançada
    ///
    /// PARÂMETRO:
    /// - novoFuncionario: objeto da classe Funcionario a ser salvo
    ///
    /// RETORNO:
    /// - bool: true se salvou com sucesso, false se deu erro
    ///
    /// EXEMPLO DE USO:
    /// Funcionario func = new Funcionario() { Nome = "João", CPF = "123.456.789-00" };
    /// bool sucesso = funcionarioService.Salvar(func);
    /// if (sucesso)
    /// {
    ///     Console.WriteLine($"Funcionário salvo com ID: {func.Id}"); // ID gerado!
    /// }
    /// </summary>
    public bool Salvar(Funcionario novoFuncionario)
    {
        try
        {
            // VALIDAÇÃO: verifica se o objeto é nulo (vazio)
            if (novoFuncionario == null)
            {
                UltimoErro = "ERRO: Funcionário não pode ser nulo!"; Console.WriteLine(UltimoErro);
                return false;
            }

            // VALIDAÇÃO: verifica se o nome está preenchido
            if (string.IsNullOrWhiteSpace(novoFuncionario.Nome))
            {
                UltimoErro = "ERRO: Nome do funcionário é obrigatório!"; Console.WriteLine(UltimoErro);
                return false;
            }

            // VALIDAÇÃO: verifica se o CPF está preenchido
            if (string.IsNullOrWhiteSpace(novoFuncionario.CPF))
            {
                UltimoErro = "ERRO: CPF é obrigatório!"; Console.WriteLine(UltimoErro);
                return false;
            }

            // VALIDAÇÃO: verifica se já existe funcionário com este CPF NO BANCO
            // Agora consultamos o BANCO DE DADOS, não uma lista em memória!
            //
            // LINQ QUERY COM EF CORE:
            // context.Funcionarios.Any(f => f.CPF == cpf)
            // |                     |
            // |                     └─ Operação LINQ (mesmo que antes com List)
            // └─ DbSet (tabela do banco)
            //
            // IMPORTANTE: This executes a SELECT COUNT query!
            // EF Core converte em: SELECT COUNT(*) FROM Funcionarios WHERE CPF = '123...'
            if (context.Funcionarios.Any(f => f.CPF == novoFuncionario.CPF))
            {
                UltimoErro = "ERRO: Já existe funcionário com este CPF!"; Console.WriteLine(UltimoErro);
                return false;
            }

            // Define a data de admissão como hoje
            // EF Core salva como DATETIME no PostgreSQL
            novoFuncionario.DataAdmissao = DateTime.Now;

            // Marca como ativo por padrão
            novoFuncionario.Ativo = true;

            // Adiciona ao DbSet (marca como novo para ser inserido)
            // Add() = marca a entidade com estado "Added"
            // Ainda NÃO foi gravado no banco!
            context.Funcionarios.Add(novoFuncionario);

            // EXECUTA A PERSISTÊNCIA NO BANCO
            // SaveChanges():
            // 1. Analisa todas as entidades "Added"
            // 2. Gera SQL INSERT
            // 3. Executa no PostgreSQL
            // 4. PostgreSQL gera o ID automaticamente
            // 5. Retorna o número de registros afetados
            // 6. Atualiza novoFuncionario.Id com o valor gerado!
            int registrosAfetados = context.SaveChanges();

            // Verifica se realmente salvou
            if (registrosAfetados > 0)
            {
                Console.WriteLine($"Funcionário '{novoFuncionario.Nome}' salvo com sucesso! ID: {novoFuncionario.Id}");
                return true;
            }
            else
            {
                UltimoErro = "ERRO: Não conseguiu salvar o funcionário (SaveChanges retornou 0)"; Console.WriteLine(UltimoErro);
                return false;
            }
        }
        catch (Exception ex)
        {
            // Se houver erro (banco offline, constraint violado, etc)
            // Captura a exceção e mostra mensagem
            // Depois de SaveChanges() em erro, DbContext pode ficar inconsistente
            // Por isso é bom usar try/catch
            UltimoErro = $"ERRO ao salvar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    // ========================================================================
    // MÉTODO: LISTAR TODOS (READ ALL) - COM EF CORE
    // ========================================================================

    /// <summary>
    /// Retorna uma lista com TODOS os funcionários cadastrados NO BANCO.
    ///
    /// MUDANÇA (List → EF Core):
    /// ANTES: Retornava cópia da lista em memória
    /// AGORA: Consulta o banco de dados
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Acessa DbSet<Funcionario> (tabela no banco)
    /// 2. Executa SELECT * FROM Funcionarios
    /// 3. Converte resultado para List<Funcionario>
    /// 4. Retorna os dados
    ///
    /// LINQ vs SQL:
    /// C#:  context.Funcionarios.ToList()
    /// SQL: SELECT * FROM Funcionarios
    /// ↑ EF Core converte automaticamente!
    ///
    /// RETORNO:
    /// - List<Funcionario>: lista com todos os funcionários do banco
    ///
    /// EXEMPLO DE USO:
    /// List<Funcionario> todos = funcionarioService.ListarTodos();
    /// foreach (var func in todos)
    /// {
    ///     Console.WriteLine($"{func.Id} - {func.Nome} - {func.Cargo}");
    /// }
    /// </summary>
    public List<Funcionario> ListarTodos()
    {
        // Retorna todos os funcionários do banco
        // context.Funcionarios = DbSet (tabela Funcionarios)
        // .ToList() = executa SELECT e traz para memória
        //
        // IMPORTANTE: ToList() executa a query AGORA!
        // Se não chamar ToList(), query é executada depois (lazy evaluation)
        // Isso é importante para performance (saiba quando a query executa)
        return context.Funcionarios.ToList();
    }

    /// <summary>
    /// Retorna APENAS os funcionários ATIVOS (em trabalho).
    ///
    /// LINQ com EF Core (mesma sintaxe, executa no banco):
    /// LINQ query: .Where(f => f.Ativo == true)
    /// SQL gerado: SELECT * FROM Funcionarios WHERE Ativo = 1
    ///
    /// O QUE É LINQ?
    /// É uma forma elegante de consultar dados.
    /// Mesmo código LINQ funciona com:
    /// - List<T> (em memória)
    /// - DbSet<T> (no banco, via EF Core)
    /// - Arquivos XML
    /// - Outros bancos de dados
    ///
    /// SINTAXE LINQ:
    /// .Where(condição) = filtra (SELECT... WHERE)
    /// .OrderBy(propriedade) = ordena (ORDER BY)
    /// .Select(projeção) = escolhe colunas
    /// .Count() = conta registros
    /// .FirstOrDefault() = primeiro ou null
    /// .ToList() = executa e traz para memória
    ///
    /// RETORNO:
    /// - List<Funcionario>: lista apenas com funcionários ativos
    ///
    /// EXEMPLO DE USO:
    /// var ativos = funcionarioService.ListarAtivos();
    /// Console.WriteLine($"Funcionários ativos: {ativos.Count}");
    /// </summary>
    public List<Funcionario> ListarAtivos()
    {
        // Where = filtra (seleciona apenas os que satisfazem condição)
        // f => f.Ativo == true = para cada f, verifica se Ativo é true
        // ToList() = executa query no banco e traz resultado
        //
        // EF Core converte para: SELECT * FROM Funcionarios WHERE Ativo = 1
        return context.Funcionarios.Where(f => f.Ativo == true).ToList();
    }

    // ========================================================================
    // MÉTODO: BUSCAR POR ID (READ ONE) - COM EF CORE
    // ========================================================================

    /// <summary>
    /// Busca um funcionário específico pelo ID no banco de dados.
    ///
    /// MUDANÇA (List → EF Core):
    /// ANTES: Procurava na lista em memória
    /// AGORA: Consulta o banco de dados
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Executa LINQ: FirstOrDefault(f => f.Id == id)
    /// 2. EF Core converte para: SELECT * FROM Funcionarios WHERE Id = @id
    /// 3. Parâmetro @id é automático (proteção SQL Injection)
    /// 4. Se encontrar, retorna o funcionário
    /// 5. Se não, retorna null
    ///
    /// PARÂMETRO:
    /// - id: o ID do funcionário que você quer buscar
    ///
    /// RETORNO:
    /// - Funcionario: o funcionário encontrado, ou null se não existir
    ///
    /// IMPORTANTE: null safety
    /// if (func != null) { ... }  // Antes de usar!
    /// Ou use C# 8+: func?.Nome   // Safe navigation
    ///
    /// EXEMPLO DE USO:
    /// Funcionario func = funcionarioService.BuscarPorId(1);
    /// if (func != null)
    /// {
    ///     Console.WriteLine($"Encontrado: {func.Nome}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Funcionário não encontrado!");
    /// }
    ///
    /// O QUE É FirstOrDefault()?
    /// - FirstOrDefault = retorna PRIMEIRO que satisfaz OU null
    /// - É SEGURO (não dá erro se não encontrar)
    /// - First() daria exceção se não encontrasse (evite!)
    ///
    /// FirstOrDefault() vs Find():
    /// FirstOrDefault(): executa SELECT... WHERE (pode ser lento)
    /// Find(): busca em cache primeiro (mais rápido)
    /// Para agora usamos FirstOrDefault, futuros ajustes podem usar Find()
    /// </summary>
    public Funcionario BuscarPorId(int id)
    {
        // Procura o funcionário com este ID no banco
        // EF Core executa: SELECT * FROM Funcionarios WHERE Id = @id
        // @id = parâmetro (proteção automática contra SQL Injection!)
        return context.Funcionarios.FirstOrDefault(f => f.Id == id);
    }

    // ========================================================================
    // MÉTODO: ATUALIZAR (UPDATE) - COM EF CORE
    // ========================================================================

    /// <summary>
    /// Atualiza as informações de um funcionário existente NO BANCO.
    ///
    /// MUDANÇA (List → EF Core):
    /// ANTES: Buscava na memória, atualizava referência diretamente
    /// AGORA: Busca no banco, atualiza, persiste com SaveChanges()
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Busca o funcionário pelo ID no banco
    /// 2. Se encontrar, atualiza os dados
    /// 3. Marca como "Modified" no EF Core
    /// 4. Chama SaveChanges() para PERSISTIR
    /// 5. EF Core gera SQL UPDATE automaticamente
    ///
    /// EF CORE CHANGE TRACKING:
    /// Quando você busca um objeto via DbContext:
    ///   var func = context.Funcionarios.FirstOrDefault(...);
    /// EF Core RASTREIA mudanças automáticamente
    ///   func.Nome = "novo"  ← EF Core vê que mudou!
    ///   context.SaveChanges() ← Executa UPDATE
    ///
    /// IMPORTANTE:
    /// Não é necessário chamar context.Update() manualmente
    /// EF Core já sabe que precisa fazer UPDATE
    /// Mas para deixar explícito, podemos chamar: context.Update(func)
    ///
    /// PARÂMETRO:
    /// - funcionarioAtualizado: objeto com os dados novos (deve ter Id válido)
    ///
    /// RETORNO:
    /// - bool: true se atualizou, false se não encontrou ou erro
    ///
    /// EXEMPLO DE USO:
    /// Funcionario func = new Funcionario()
    /// {
    ///     Id = 1,  // ← OBRIGATÓRIO! Sem Id, EF não sabe qual atualizar
    ///     Nome = "João Silva",
    ///     Cargo = "Farmacêutico"
    /// };
    /// bool atualizado = funcionarioService.Atualizar(func);
    /// </summary>
    public bool Atualizar(Funcionario funcionarioAtualizado)
    {
        try
        {
            // Validação
            if (funcionarioAtualizado == null || funcionarioAtualizado.Id <= 0)
            {
                UltimoErro = "ERRO: Funcionário ou ID inválido!"; Console.WriteLine(UltimoErro);
                return false;
            }

            // Busca o funcionário existente no banco pelo ID
            Funcionario funcionarioExistente = BuscarPorId(funcionarioAtualizado.Id);

            // Se não encontrou, retorna false
            if (funcionarioExistente == null)
            {
                Console.WriteLine($"ERRO: Funcionário com ID {funcionarioAtualizado.Id} não encontrado!");
                return false;
            }

            // Atualiza os dados (exceção do ID e DataAdmissao que não podem mudar)
            // Nota: Não atualizam Id pois é chave primária
            // Nota: Não atualizam DataAdmissao pois é quando foi contratado
            funcionarioExistente.Nome = funcionarioAtualizado.Nome;
            funcionarioExistente.CPF = funcionarioAtualizado.CPF;
            funcionarioExistente.Cargo = funcionarioAtualizado.Cargo;
            funcionarioExistente.SenhaHash = funcionarioAtualizado.SenhaHash;

            // IMPORTANTE: EF Core já está rastreando este objeto
            // Não é necessário chamar context.Update() pois:
            // - Ele foi buscado via context.FirstOrDefault()
            // - EF Core sabe que é uma entidade monitorada
            // - Mudanças são detectadas automaticamente

            // Persiste no banco de dados
            // SaveChanges() vai executar: UPDATE Funcionarios SET ... WHERE Id = @id
            int registrosAfetados = context.SaveChanges();

            if (registrosAfetados > 0)
            {
                Console.WriteLine($"Funcionário '{funcionarioExistente.Nome}' atualizado com sucesso!");
                return true;
            }
            else
            {
                UltimoErro = "ERRO: Não conseguiu atualizar o funcionário"; Console.WriteLine(UltimoErro);
                return false;
            }
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao atualizar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    // ========================================================================
    // MÉTODO: INATIVAR (SOFT DELETE) - COM EF CORE
    // ========================================================================

    /// <summary>
    /// Inativa um funcionário (marca como não ativo) sem deletar do banco.
    ///
    /// POR QUÊ SOFT DELETE (inativar em vez de deletar)?
    /// 1. AUDITORIA: Histórico permanece para auditar quem fez o quê
    /// 2. REFERÊNCIAS: Se houver vendas, referência continua válida
    /// 3. COMPLIANCE: Pode ser exigido manter registros por lei
    /// 4. RECUPERAÇÃO: Pode reativar se for erro
    ///
    /// IMPORTANTE: A linha ainda existe no banco
    /// SELECT * FROM Funcionarios WHERE Id = 1  ← Ainda retorna!
    /// SELECT * FROM Funcionarios WHERE Ativo = 1  ← Não retorna
    ///
    /// PARÂMETRO:
    /// - id: ID do funcionário a inativar
    ///
    /// RETORNO:
    /// - bool: true se inativou, false se não encontrou
    ///
    /// EXEMPLO DE USO:
    /// bool inativado = funcionarioService.Inativar(1);
    /// if (inativado) Console.WriteLine("Funcionário inativado!");
    /// </summary>
    public bool Inativar(int id)
    {
        try
        {
            // Busca o funcionário no banco
            Funcionario funcionario = BuscarPorId(id);

            // Se não encontrou
            if (funcionario == null)
            {
                Console.WriteLine($"ERRO: Funcionário com ID {id} não encontrado!");
                return false;
            }

            // Marca como inativo e registra a data de saída
            funcionario.Ativo = false;
            funcionario.DataDemissao = DateTime.Now;

            // EF Core rastreia as mudanças automaticamente
            // SaveChanges() executa: UPDATE Funcionarios SET Ativo = 0, DataDemissao = ... WHERE Id = @id
            int registrosAfetados = context.SaveChanges();

            if (registrosAfetados > 0)
            {
                Console.WriteLine($"Funcionário '{funcionario.Nome}' inativado com sucesso!");
                return true;
            }
            else
            {
                UltimoErro = "ERRO: Não conseguiu inativar o funcionário"; Console.WriteLine(UltimoErro);
                return false;
            }
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao inativar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    /// <summary>
    /// Remove FISICAMENTE um funcionário do banco de dados (HARD DELETE).
    ///
    /// ATENCAO: Use com MUITA moderação!
    /// Na maioria dos casos, prefira Inativar() em vez de Remover()!
    ///
    /// CONSEQUÊNCIAS DE HARD DELETE:
    /// - Histórico é perdido permanentemente
    /// - Se houver vendas referenciando este funcionário, pode quebrar referência
    /// - Não é auditável
    /// - Pode violar LGPD/compliance
    ///
    /// USE REMOVER() APENAS SE:
    /// - Funcionário foi cadastrado por engano
    /// - Antes de qualquer venda ser registrada
    /// - Você TEM CERTEZA que ninguém o referencia
    ///
    /// PARÂMETRO:
    /// - id: ID do funcionário a remover
    ///
    /// RETORNO:
    /// - bool: true se removeu, false se não encontrou
    ///
    /// EXEMPLO DE USO:
    /// bool removido = funcionarioService.Remover(1);
    /// </summary>
    public bool Remover(int id)
    {
        try
        {
            // Busca o funcionário no banco
            Funcionario funcionario = BuscarPorId(id);

            if (funcionario == null)
            {
                Console.WriteLine($"ERRO: Funcionário com ID {id} não encontrado!");
                return false;
            }

            // REMOVE fisicamente do banco (HARD DELETE)
            // context.Remove() marca para deleção
            // SaveChanges() executa: DELETE FROM Funcionarios WHERE Id = @id
            context.Funcionarios.Remove(funcionario);

            int registrosAfetados = context.SaveChanges();

            if (registrosAfetados > 0)
            {
                Console.WriteLine($"Funcionário removido completamente do sistema!");
                return true;
            }
            else
            {
                UltimoErro = "ERRO: Não conseguiu remover o funcionário"; Console.WriteLine(UltimoErro);
                return false;
            }
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao remover funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    // ========================================================================
    // MÉTODO: CONTAR - COM EF CORE
    // ========================================================================

    /// <summary>
    /// Retorna o total de funcionários cadastrados NO BANCO.
    ///
    /// LINQ: context.Funcionarios.Count()
    /// SQL: SELECT COUNT(*) FROM Funcionarios
    ///
    /// RETORNO:
    /// - int: número total de funcionários
    ///
    /// EXEMPLO DE USO:
    /// int total = funcionarioService.Contar();
    /// Console.WriteLine($"Total de funcionários: {total}");
    /// </summary>
    public int Contar()
    {
        // Retorna contagem de todos os registros
        // EF Core executa SELECT COUNT(*) no banco (é eficiente!)
        return context.Funcionarios.Count();
    }

    /// <summary>
    /// Retorna o total de funcionários ATIVOS NO BANCO.
    ///
    /// LINQ: context.Funcionarios.Count(f => f.Ativo == true)
    /// SQL: SELECT COUNT(*) FROM Funcionarios WHERE Ativo = 1
    ///
    /// RETORNO:
    /// - int: número de funcionários ativos
    ///
    /// EXEMPLO DE USO:
    /// int ativos = funcionarioService.ContarAtivos();
    /// Console.WriteLine($"Funcionários ativos: {ativos}");
    /// </summary>
    public int ContarAtivos()
    {
        // Retorna contagem apenas de ativos
        // Count() com Where é otimizado: executa WHERE na consulta
        return context.Funcionarios.Count(f => f.Ativo == true);
    }

}