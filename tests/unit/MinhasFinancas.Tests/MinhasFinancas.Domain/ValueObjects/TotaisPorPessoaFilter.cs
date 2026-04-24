using System;

namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Filtro específico para consultas de totais por pessoa.
/// </summary>
public record TotaisPorPessoaFilter
{
    public IdFilter? Pessoa { get; init; }
    public DataFilter? Periodo { get; init; }

    public TotaisPorPessoaFilter WithNormalizedDates()
    {
        return this with { Periodo = Periodo?.Normalize() ?? new DataFilter().Normalize() };
    }
}
