namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Value object para totais por categoria.
/// </summary>
public record TotalPorCategoria
{
    /// <summary>
    /// Identificador da categoria.
    /// </summary>
    public Guid CategoriaId { get; init; }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Descricao { get; init; } = string.Empty;

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