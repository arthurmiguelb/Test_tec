using System;

namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Reutilizável para representar um período de datas com conveniência de mês/ano.
/// </summary>
public record DataFilter
{
    public DateTime? DataInicio { get; init; }
    public DateTime? DataFim { get; init; }
    public int? Mes { get; init; }
    public int? Ano { get; init; }

    public DataFilter Normalize()
    {
        if (Mes is int m && Ano is int y)
        {
            var inicio = new DateTime(y, m, 1);
            var fim = inicio.AddMonths(1).AddTicks(-1);
            return this with { DataInicio = inicio, DataFim = fim };
        }

        return this;
    }
}
