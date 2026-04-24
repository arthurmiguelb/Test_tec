namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Value object para totais por pessoa.
/// </summary>
public record TotalPorPessoa
{
    /// <summary>
    /// Identificador da pessoa.
    /// </summary>
    public Guid PessoaId { get; init; }

    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string Nome { get; init; } = string.Empty;

    /// <summary>
    /// Total de receitas.
    /// </summary>
    public decimal TotalReceitas { get; init; }

    /// <summary>
    /// Total de despesas.
    /// </summary>
    public decimal TotalDespesas { get; init; }

    /// <summary>
    /// Saldo (receitas - despesas).
    /// </summary>
    public decimal Saldo => TotalReceitas - TotalDespesas;
}