using estoque_farmacia.Models;
using System.Collections.Generic;
using System.Linq;

namespace estoque_farmacia.Services;

/// <summary>
/// CLASSE FUNCIONARIOSERVICE (SERVICE/LÓGICA DE NEGÓCIO)
/// ======================================================
///
/// O QUE É UM SERVICE?
/// Service é uma classe que contém a LÓGICA DE NEGÓCIO.
/// Enquanto o Model define "O QUE SÃO os dados",
/// o Service define "O QUE FAZER COM os dados".
///
/// RESPONSABILIDADES DESTE SERVICE:
/// - Salvar novos funcionários
/// - Listar todos os funcionários
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
/// PADRÃO USADO: Em memória (List) por enquanto.
/// Depois integraremos com SQL Server (banco de dados real).
/// </summary>
public class FuncionarioService
{
    // ATRIBUTO PRIVADO (só esta classe pode acessar)
    // private = só pode ser acessado DENTRO desta classe
    // readonly = não pode ser reatribuído (segurança)
    /// <summary>
    /// Lista que armazena todos os funcionários em MEMÓRIA (RAM do computador).
    ///
    /// O QUE É List<T>?
    /// - List é uma coleção (lista) de itens
    /// - <Funcionario> significa que a lista contém objetos do tipo Funcionario
    /// - É dinâmica: cresce automaticamente conforme adicionamos itens
    ///
    /// EXEMPLO:
    /// List<string> nomes = new List<string>();
    /// nomes.Add("João"); // agora tem 1 item
    /// nomes.Add("Maria"); // agora tem 2 itens
    ///
    /// IMPORTANTE:
    /// Os dados aqui são PERDIDOS quando o programa fecha!
    /// Para dados permanentes, usamos SQL Server (banco de dados).
    /// </summary>
    private readonly List<Funcionario> listaFuncionarios = new List<Funcionario>();

    // VARIÁVEL PARA GERAR IDs ÚNICOS
    /// <summary>
    /// Contador para gerar IDs únicos e sequenciais.
    /// Cada novo funcionário recebe um ID = proximoId (depois incrementa).
    ///
    /// EXEMPLO:
    /// Primeiro funcionário: proximoId = 1 (depois vira 2)
    /// Segundo funcionário: proximoId = 2 (depois vira 3)
    /// Terceiro funcionário: proximoId = 3 (depois vira 4)
    /// </summary>
    private int proximoId = 1;

    // ========================================================================
    // MÉTODO: SALVAR (CREATE)
    // ========================================================================

    /// <summary>
    /// Salva (adiciona) um novo funcionário na lista em memória.
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Valida se o funcionário não é nulo
    /// 2. Atribui um ID único
    /// 3. Define a data de admissão como hoje
    /// 4. Marca como ativo por padrão
    /// 5. Adiciona à lista
    /// 6. Incrementa o próximo ID
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
    /// if (sucesso) Console.WriteLine("Funcionário salvo!");
    /// </summary>
    public bool Salvar(Funcionario novoFuncionario)
    {
        // VALIDAÇÃO: verifica se o objeto é nulo (vazio)
        // null = sem valor, não existe
        if (novoFuncionario == null)
        {
            Console.WriteLine("ERRO: Funcionário não pode ser nulo!");
            return false;
        }

        // VALIDAÇÃO: verifica se o nome está preenchido
        if (string.IsNullOrWhiteSpace(novoFuncionario.Nome))
        {
            Console.WriteLine("ERRO: Nome do funcionário é obrigatório!");
            return false;
        }

        // VALIDAÇÃO: verifica se o CPF está preenchido
        if (string.IsNullOrWhiteSpace(novoFuncionario.CPF))
        {
            Console.WriteLine("ERRO: CPF é obrigatório!");
            return false;
        }

        // VALIDAÇÃO: verifica se já existe funcionário com este CPF
        // Any() = verifica se existe algum item que satisfaz a condição
        if (listaFuncionarios.Any(f => f.CPF == novoFuncionario.CPF))
        {
            Console.WriteLine("ERRO: Já existe funcionário com este CPF!");
            return false;
        }

        // Atribui um ID único (auto-incremento)
        novoFuncionario.Id = proximoId;
        proximoId++; // incrementa para o próximo funcionário

        // Define a data de admissão como hoje
        novoFuncionario.DataAdmissao = DateTime.Now;

        // Marca como ativo por padrão
        novoFuncionario.Ativo = true;

        // Adiciona à lista
        listaFuncionarios.Add(novoFuncionario);

        Console.WriteLine($"✓ Funcionário '{novoFuncionario.Nome}' salvo com sucesso! ID: {novoFuncionario.Id}");
        return true;
    }

    // ========================================================================
    // MÉTODO: LISTAR TODOS (READ ALL)
    // ========================================================================

    /// <summary>
    /// Retorna uma lista com TODOS os funcionários cadastrados.
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// Retorna uma cópia da lista interna (para segurança).
    ///
    /// RETORNO:
    /// - List<Funcionario>: lista com todos os funcionários
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
        // Retorna uma cópia da lista (segurança)
        // Se retornássemos direto a lista, alguém poderia modificar ela de fora
        return new List<Funcionario>(listaFuncionarios);
    }

    /// <summary>
    /// Retorna APENAS os funcionários ATIVOS (em trabalho).
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// Usa LINQ (Language Integrated Query) para filtrar apenas os ativos.
    ///
    /// O QUE É LINQ?
    /// É uma forma elegante e poderosa de consultar dados.
    /// Sintaxe: .Where(condição).ToList()
    ///
    /// EXEMPLO:
    /// var maiores = numeros.Where(n => n > 10).ToList(); // só maiores que 10
    /// var ativos = funcionarios.Where(f => f.Ativo == true).ToList(); // só ativos
    ///
    /// RETORNO:
    /// - List<Funcionario>: lista apenas com funcionários ativos
    ///
    /// EXEMPLO DE USO:
    /// var ativos = funcionarioService.ListarAtivos();
    /// Console.WriteLine($"Funcionários ativos: {ativos.Count()}");
    /// </summary>
    public List<Funcionario> ListarAtivos()
    {
        // Where = filtra (seleciona apenas os que atendem a condição)
        // f => f.Ativo == true = para cada funcionário f, verifica se está ativo
        // ToList() = converte o resultado em List
        return listaFuncionarios.Where(f => f.Ativo == true).ToList();
    }

    // ========================================================================
    // MÉTODO: BUSCAR POR ID (READ ONE)
    // ========================================================================

    /// <summary>
    /// Busca um funcionário específico pelo ID.
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// Procura na lista um funcionário com o ID específico.
    /// Se encontrar, retorna. Se não, retorna null.
    ///
    /// PARÂMETRO:
    /// - id: o ID do funcionário que você quer buscar
    ///
    /// RETORNO:
    /// - Funcionario: o funcionário encontrado, ou null se não existir
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
    /// - FirstOrDefault = retorna o PRIMEIRO item que satisfaz a condição
    /// - Se nenhum satisfizer, retorna null (o padrão)
    /// - É mais seguro que First() (que dá erro se não encontrar)
    /// </summary>
    public Funcionario BuscarPorId(int id)
    {
        // Procura o funcionário com este ID
        return listaFuncionarios.FirstOrDefault(f => f.Id == id);
    }

    // ========================================================================
    // MÉTODO: ATUALIZAR (UPDATE)
    // ========================================================================

    /// <summary>
    /// Atualiza as informações de um funcionário existente.
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Busca o funcionário pelo ID
    /// 2. Se encontrar, atualiza os dados
    /// 3. Retorna true se conseguiu, false se não encontrou
    ///
    /// PARÂMETRO:
    /// - funcionarioAtualizado: objeto com os dados novos
    ///
    /// RETORNO:
    /// - bool: true se atualizou, false se não encontrou
    ///
    /// EXEMPLO DE USO:
    /// Funcionario func = new Funcionario()
    /// {
    ///     Id = 1,
    ///     Nome = "João Silva",
    ///     Cargo = "Farmacêutico"
    /// };
    /// bool atualizado = funcionarioService.Atualizar(func);
    /// </summary>
    public bool Atualizar(Funcionario funcionarioAtualizado)
    {
        // Validação
        if (funcionarioAtualizado == null || funcionarioAtualizado.Id <= 0)
        {
            Console.WriteLine("ERRO: Funcionário ou ID inválido!");
            return false;
        }

        // Busca o funcionário existente pelo ID
        Funcionario funcionarioExistente = BuscarPorId(funcionarioAtualizado.Id);

        // Se não encontrou, retorna false
        if (funcionarioExistente == null)
        {
            Console.WriteLine($"ERRO: Funcionário com ID {funcionarioAtualizado.Id} não encontrado!");
            return false;
        }

        // Atualiza os dados (exceção do ID, que não pode mudar)
        funcionarioExistente.Nome = funcionarioAtualizado.Nome;
        funcionarioExistente.CPF = funcionarioAtualizado.CPF;
        funcionarioExistente.Cargo = funcionarioAtualizado.Cargo;
        funcionarioExistente.SenhaHash = funcionarioAtualizado.SenhaHash;

        Console.WriteLine($"✓ Funcionário '{funcionarioExistente.Nome}' atualizado com sucesso!");
        return true;
    }

    // ========================================================================
    // MÉTODO: REMOVER/INATIVAR (DELETE)
    // ========================================================================

    /// <summary>
    /// Inativa um funcionário (marca como não ativo).
    ///
    /// IMPORTANTE: NÃO DELETAMOS, apenas marcamos como inativo!
    ///
    /// POR QUÊ?
    /// - Segurança: mantém histórico e auditoria
    /// - Dados: não perde informações do banco de dados
    /// - Referências: se houver vendas de um funcionário, a referência continua válida
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
        // Busca o funcionário
        Funcionario funcionario = BuscarPorId(id);

        // Se não encontrou
        if (funcionario == null)
        {
            Console.WriteLine($"ERRO: Funcionário com ID {id} não encontrado!");
            return false;
        }

        // Marca como inativo e registra a data
        funcionario.Ativo = false;
        funcionario.DataDemissao = DateTime.Now;

        Console.WriteLine($"✓ Funcionário '{funcionario.Nome}' inativado com sucesso!");
        return true;
    }

    /// <summary>
    /// Remove FISICAMENTE um funcionário da lista (se realmente necessário).
    ///
    /// CUIDADO: Use com moderação!
    /// Na maioria dos casos, prefira Inativar() em vez de Remover().
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
        // Busca o funcionário
        Funcionario funcionario = BuscarPorId(id);

        if (funcionario == null)
        {
            Console.WriteLine($"ERRO: Funcionário com ID {id} não encontrado!");
            return false;
        }

        // Remove da lista
        listaFuncionarios.Remove(funcionario);

        Console.WriteLine($"✓ Funcionário removido completamente do sistema!");
        return true;
    }

    // ========================================================================
    // MÉTODO: CONTAR
    // ========================================================================

    /// <summary>
    /// Retorna o total de funcionários cadastrados.
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
        return listaFuncionarios.Count;
    }

    /// <summary>
    /// Retorna o total de funcionários ATIVOS.
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
        return listaFuncionarios.Where(f => f.Ativo == true).Count();
    }

}