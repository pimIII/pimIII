-- Script de criação da tabela funcionarios (PostgreSQL)
CREATE TABLE IF NOT EXISTS public.funcionarios (
    id serial PRIMARY KEY,
    nome varchar(150) NOT NULL,
    cpf varchar(14) NOT NULL,
    cargo varchar(100),
    salario numeric(12,2) NOT NULL CHECK (salario >= 0),
    data_admissao date NOT NULL CHECK (data_admissao <= CURRENT_DATE)
);

CREATE UNIQUE INDEX IF NOT EXISTS ix_funcionarios_cpf ON public.funcionarios (cpf);
CREATE INDEX IF NOT EXISTS ix_funcionarios_nome_lower ON public.funcionarios (lower(nome));
