using System;
namespace estoque_farmacia.Models;

/// <summary>
/// CLASSE FUNCIONARIO (MODEL)
/// ============================
///
/// O QUE É ESTA CLASSE?
/// Esta é uma classe MODEL que define a ESTRUTURA dos dados de um funcionário.
/// Pense nela como um "formulário" ou "ficha" que descreve todas as informações
/// que precisamos guardar sobre cada pessoa que trabalha na farmácia.
///
/// POR QUE USAR CLASSES MODELS?
/// - Organização: separa a definição de dados (Model) da lógica de negócio (Service)
/// - Segurança: usando Properties (get; set;) controlamos como os dados são acessados
/// - Reutilização: esta classe é usada em vários lugares do projeto
/// - Banco de dados: cada property vira uma coluna na tabela do banco de dados
///
/// EXEMPLO DE USO:
/// Funcionario novoFunc = new Funcionario();
/// novoFunc.Nome = "João Silva";
/// novoFunc.CPF = "123.456.789-00";
/// novoFunc.Cargo = "Farmacêutico";
///
/// NOTA IMPORTANTE:
/// Este é um exemplo de ENCAPSULAMENTO (conceito de POO).
/// Usamos "public" para permitir acesso, mas podemos adicionar validações depois.
/// </summary>
public class Funcionario
{
    /// <summary>
    /// ID ÚNICO do funcionário (identificador).
    ///
    /// O QUE É ID?
    /// É um número que identifica de forma ÚNICA cada funcionário no banco de dados.
    /// Exemplo: Funcionário 1 = João, Funcionário 2 = Maria, etc.
    ///
    /// POR QUE É IMPORTANTE?
    /// - Diferencia cada funcionário
    /// - Permite fazer buscas rápidas no banco
    /// - Evita confusão entre pessoas com nomes parecidos
    /// - Auto-incrementa no banco (1, 2, 3, 4...)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome completo do funcionário.
    ///
    /// O QUE É "get; set;"?
    /// - "get;" = permite LER o valor (Console.WriteLine(funcionario.Nome))
    /// - "set;" = permite ESCREVER o valor (funcionario.Nome = "João")
    /// Junto são chamados de "Property" (propriedade).
    /// É mais seguro que usar variáveis públicas diretas.
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// CPF do funcionário (documento de identificação).
    /// Formato esperado: "123.456.789-00"
    ///
    /// NOTA DE SEGURANÇA:
    /// O CPF é um dado sensível. Em um sistema real, deveria:
    /// - Ser criptografado no banco de dados
    /// - Nunca aparecer inteiro na tela
    /// - Ser validado antes de salvar (verificar se é válido)
    ///
    /// PARA ESTE PROJETO:
    /// Estamos armazenando em texto simples para fins educacionais.
    /// Em produção, SEMPRE criptografe dados sensíveis!
    /// </summary>
    public string CPF { get; set; }

    /// <summary>
    /// Cargo ou função do funcionário na farmácia.
    ///
    /// EXEMPLOS DE CARGOS:
    /// - "Gerente" → acesso total, pode gerenciar tudo
    /// - "Farmacêutico" → pode vender medicamentos controlados
    /// - "Atendente" → venda básica e atendimento
    /// - "Estoquista" → controla entrada/saída de produtos
    ///
    /// DICA: Em um sistema real, isso seria um ENUM (tipo limitado)
    /// para garantir que só existem cargos válidos:
    /// public enum CargoEnum { Gerente, Farmaceutico, Atendente }
    /// </summary>
    public string Cargo { get; set; }

    /// <summary>
    /// Hash criptografado da senha do funcionário.
    ///
    /// O QUE É HASH?
    /// É uma transformação matemática que torna a senha irreversível.
    /// Exemplo: "123456" vira "abc123xyz789" (não volta para "123456")
    ///
    /// POR QUE USAR HASH?
    /// - Segurança: mesmo se alguém hackear o banco, não vê a senha original
    /// - Comparação: ao fazer login, transforma a senha digitada em hash e compara
    /// - Padrão: é o jeito correto de armazenar senhas
    ///
    /// COMO FUNCIONA O LOGIN:
    /// 1. Usuário digita: "minha_senha_123"
    /// 2. Sistema transforma em hash: "xyz789abc123"
    /// 3. Compara com o hash armazenado no banco
    /// 4. Se forem iguais, libera o acesso
    ///
    /// NUNCA faça isso:
    /// public string Senha { get; set; } // ERRADO! Perigoso!
    /// </summary>
    public string SenhaHash { get; set; }

    /// <summary>
    /// Data em que o funcionário foi admitido na empresa.
    ///
    /// O QUE É DateTime?
    /// É um tipo de dado que armazena DATA + HORA completas.
    /// Exemplo: DateTime.Now retorna "2026-05-09 14:30:45"
    ///
    /// POR QUE USAR?
    /// - Auditoria: saber quando cada pessoa foi contratada
    /// - Cálculo de tempo: calcular anos de empresa, férias, etc
    /// - Ordenação: listar funcionários por data de admissão
    ///
    /// EXEMPLO:
    /// Funcionario func = new Funcionario();
    /// func.DataAdmissao = new DateTime(2024, 1, 15); // 15 de janeiro de 2024
    ///
    /// OUTRO JEITO:
    /// func.DataAdmissao = DateTime.Now; // usa a data de hoje
    /// </summary>
    public DateTime DataAdmissao { get; set; }

    /// <summary>
    /// Indica se o funcionário está ativo (trabalhando) no sistema.
    ///
    /// O QUE É "bool"?
    /// bool é um tipo que SÓ pode ter 2 valores:
    /// - true = verdadeiro (sim, ativo)
    /// - false = falso (não, inativo)
    ///
    /// POR QUE USAR?
    /// - Em vez de DELETAR um funcionário (perigoso!), marcamos como inativo
    /// - Mantém histórico no banco de dados
    /// - Segurança: não perde dados
    ///
    /// EXEMPLO:
    /// // Em vez de deletar:
    /// funcionario.Ativo = false; // marca como inativo
    /// // O funcionário continua no banco, mas não aparece mais
    ///
    /// FILTRO:
    /// // Ao listar funcionários ativos:
    /// var ativos = listaFuncionarios.Where(f => f.Ativo == true);
    /// </summary>
    public bool Ativo { get; set; }

    /// <summary>
    /// Data em que o funcionário saiu ou foi demitido (opcional).
    /// Null significa que o funcionário ainda está ativo.
    ///
    /// O QUE É "DateTime?"?
    /// O "?" significa que pode ser NULL (vazio/sem valor).
    /// Sem o "?", DateTime OBRIGATORIAMENTE tem um valor.
    ///
    /// EXEMPLO:
    /// Funcionario func = new Funcionario();
    /// func.DataDemissao = null; // funcionário ainda ativo
    ///
    /// // Depois quando demitir:
    /// func.DataDemissao = DateTime.Now; // registra a data
    ///
    /// TIPO NULLABLE:
    /// int? numero = null; // pode ser um número ou null
    /// string texto = ""; // string não precisa de ?, pode ser vazio
    /// </summary>
    public DateTime? DataDemissao { get; set; }

}
