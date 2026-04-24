using System;

namespace MinhasFinancas.Domain.ValueObjects;

/// <summary>
/// Representa um filtro por id (GUID).
/// </summary>
public record IdFilter
{
    public Guid? Id { get; init; }
}
