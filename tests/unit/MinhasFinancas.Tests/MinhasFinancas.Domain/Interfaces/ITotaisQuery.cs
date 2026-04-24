using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Domain.Interfaces;

/// <summary>
/// Interface para consultas de totais.
/// </summary>
public interface ITotaisQuery
{
    /// <summary>
    /// Obtém totais por pessoa com paginação.
    /// </summary>
    Task<PagedResult<TotalPorPessoa>> GetTotaisPorPessoaAsync(TotaisPorPessoaFilter? filter = null, PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém totais por categoria com paginação.
    /// </summary>
    Task<PagedResult<TotalPorCategoria>> GetTotaisPorCategoriaAsync(TotaisPorCategoriaFilter? filter = null, PagedRequest? pageRequest = null);
}