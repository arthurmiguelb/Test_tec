using System;

namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Filtro específico para consultas de totais por categoria.
/// </summary>
public record TotaisPorCategoriaFilter
{
    public IdFilter? Categoria { get; init; }
    public DataFilter? Periodo { get; init; }

    public TotaisPorCategoriaFilter WithNormalizedDates()
    {
        return this with { Periodo = Periodo?.Normalize() ?? new DataFilter().Normalize() };
    }
}
