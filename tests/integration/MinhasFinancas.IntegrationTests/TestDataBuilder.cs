using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.IntegrationTests;

/// <summary>
/// Builder estático para criar entidades de teste de forma consistente.
/// Centraliza a criação de dados de teste para evitar duplicação.
/// </summary>
public static class TestDataBuilder
{
    /// <summary>
    /// Cria uma pessoa menor de idade (17 anos).
    /// </summary>
    public static Pessoa CriarPessoaMenor()
    {
        return new Pessoa
        {
            Nome = "João Menor",
            DataNascimento = DateTime.Today.AddYears(-17)
        };
    }

    /// <summary>
    /// Cria uma pessoa maior de idade (25 anos).
    /// </summary>
    public static Pessoa CriarPessoaMaior()
    {
        return new Pessoa
        {
            Nome = "Maria Maior",
            DataNascimento = DateTime.Today.AddYears(-25)
        };
    }

    /// <summary>
    /// Cria uma pessoa com exatamente 18 anos (boundary value).
    /// </summary>
    public static Pessoa CriarPessoaDezoitoAnos()
    {
        return new Pessoa
        {
            Nome = "Pedro Dezoito",
            DataNascimento = DateTime.Today.AddYears(-18)
        };
    }

    /// <summary>
    /// Cria uma pessoa com 17 anos e 364 dias (boundary value - quase 18).
    /// </summary>
    public static Pessoa CriarPessoaQuaseMaior()
    {
        return new Pessoa
        {
            Nome = "Ana Quase Maior",
            DataNascimento = DateTime.Today.AddYears(-18).AddDays(1) // 17 anos e 364 dias
        };
    }

    /// <summary>
    /// Cria uma categoria do tipo Receita.
    /// </summary>
    public static Categoria CriarCategoriaReceita()
    {
        return new Categoria
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };
    }

    /// <summary>
    /// Cria uma categoria do tipo Despesa.
    /// </summary>
    public static Categoria CriarCategoriaDespesa()
    {
        return new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };
    }

    /// <summary>
    /// Cria uma categoria do tipo Ambas.
    /// </summary>
    public static Categoria CriarCategoriaAmbas()
    {
        return new Categoria
        {
            Descricao = "Investimentos",
            Finalidade = Categoria.EFinalidade.Ambas
        };
    }

    /// <summary>
    /// Cria uma transação de receita válida.
    /// </summary>
    public static Transacao CriarTransacaoReceita(Pessoa pessoa, Categoria categoria)
    {
        var transacao = new Transacao
        {
            Descricao = "Salário mensal",
            Valor = 3000.00m,
            Tipo = Transacao.ETipo.Receita,
            Data = DateTime.Today
        };

        // Usar reflexão para definir os IDs (propriedades private set)
        var categoriaIdProperty = typeof(Transacao).GetProperty("CategoriaId");
        var pessoaIdProperty = typeof(Transacao).GetProperty("PessoaId");

        categoriaIdProperty?.SetValue(transacao, categoria.Id);
        pessoaIdProperty?.SetValue(transacao, pessoa.Id);

        return transacao;
    }

    /// <summary>
    /// Cria uma transação de despesa válida.
    /// </summary>
    public static Transacao CriarTransacaoDespesa(Pessoa pessoa, Categoria categoria)
    {
        var transacao = new Transacao
        {
            Descricao = "Compra no mercado",
            Valor = 150.00m,
            Tipo = Transacao.ETipo.Despesa,
            Data = DateTime.Today
        };

        // Usar reflexão para definir os IDs (propriedades private set)
        var categoriaIdProperty = typeof(Transacao).GetProperty("CategoriaId");
        var pessoaIdProperty = typeof(Transacao).GetProperty("PessoaId");

        categoriaIdProperty?.SetValue(transacao, categoria.Id);
        pessoaIdProperty?.SetValue(transacao, pessoa.Id);

        return transacao;
    }

    /// <summary>
    /// Cria um DTO para criação de pessoa.
    /// </summary>
    public static CreatePessoaDto CriarDtoPessoa(string nome, DateTime dataNascimento)
    {
        return new CreatePessoaDto
        {
            Nome = nome,
            DataNascimento = dataNascimento
        };
    }

    /// <summary>
    /// Cria um DTO para criação de categoria.
    /// </summary>
    public static CreateCategoriaDto CriarDtoCategoria(string descricao, Categoria.EFinalidade finalidade)
    {
        return new CreateCategoriaDto
        {
            Descricao = descricao,
            Finalidade = finalidade
        };
    }

    /// <summary>
    /// Cria um DTO para criação de transação.
    /// </summary>
    public static CreateTransacaoDto CriarDtoTransacao(
        string descricao,
        decimal valor,
        Transacao.ETipo tipo,
        Guid categoriaId,
        Guid pessoaId,
        DateTime data)
    {
        return new CreateTransacaoDto
        {
            Descricao = descricao,
            Valor = valor,
            Tipo = tipo,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = data
        };
    }
}
