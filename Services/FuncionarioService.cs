using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class FuncionarioService
    {
        private readonly ApplicationDbContext _dbContext;

        public FuncionarioService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Funcionario>> GetAllAsync()
        {
            return await _dbContext.Funcionarios
                                   .AsNoTracking()
                                   .OrderBy(f => f.Nome)
                                   .ToListAsync();
        }

        public async Task<Funcionario> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id inválido.");
            return await _dbContext.Funcionarios
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Funcionario> CreateAsync(Funcionario funcionario)
        {
            if (funcionario == null) throw new ArgumentNullException(nameof(funcionario));

            funcionario.ValidarDominio();
            if (!IsCpfValido(funcionario.CPF))
                throw new ArgumentException("CPF inválido.");

            bool cpfExistente = await _dbContext.Funcionarios
                                               .AsNoTracking()
                                               .AnyAsync(f => f.CPF == funcionario.CPF);
            if (cpfExistente)
                throw new InvalidOperationException("CPF já cadastrado.");

            await _dbContext.Funcionarios.AddAsync(funcionario);
            await _dbContext.SaveChangesAsync();

            return funcionario;
        }

        public async Task<Funcionario> UpdateAsync(Funcionario funcionario)
        {
            if (funcionario == null) throw new ArgumentNullException(nameof(funcionario));
            if (funcionario.Id <= 0) throw new ArgumentException("Id inválido para atualização.");

            funcionario.ValidarDominio();
            if (!IsCpfValido(funcionario.CPF))
                throw new ArgumentException("CPF inválido.");

            bool cpfOutro = await _dbContext.Funcionarios
                                            .AsNoTracking()
                                            .AnyAsync(f => f.CPF == funcionario.CPF && f.Id != funcionario.Id);
            if (cpfOutro)
                throw new InvalidOperationException("Outro funcionário já possui este CPF.");

            var existing = await _dbContext.Funcionarios.FindAsync(funcionario.Id);
            if (existing == null) throw new KeyNotFoundException("Funcionário não encontrado.");

            existing.Nome = funcionario.Nome;
            existing.CPF = funcionario.CPF;
            existing.Cargo = funcionario.Cargo;
            existing.Salario = funcionario.Salario;
            existing.DataAdmissao = funcionario.DataAdmissao;

            _dbContext.Funcionarios.Update(existing);
            await _dbContext.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id inválido para exclusão.");

            var existing = await _dbContext.Funcionarios.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException("Funcionário não encontrado.");

            _dbContext.Funcionarios.Remove(existing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Funcionario>> SearchByNameAsync(string nome, int page = 1, int pageSize = 20)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var query = _dbContext.Funcionarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var pattern = $"%{nome}%";
                // ILike não está disponível em todas as provedoras; usar Contains como alternativa portável.
                query = query.Where(f => EF.Functions.Like(f.Nome, pattern));
            }

            return await query.OrderBy(f => f.Nome)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        private bool IsCpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            var digits = new string(cpf.Where(char.IsDigit).ToArray());
            if (digits.Length != 11) return false;

            if (new string(digits[0], 11) == digits) return false;

            int[] multiplicador1 = {10,9,8,7,6,5,4,3,2};
            int[] multiplicador2 = {11,10,9,8,7,6,5,4,3,2};

            string tempCpf = digits.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            digito = digito + resto.ToString();

            return digits.EndsWith(digito);
        }
    }
}
