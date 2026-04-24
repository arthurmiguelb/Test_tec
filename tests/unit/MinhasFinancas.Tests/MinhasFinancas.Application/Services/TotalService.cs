using MinhasFinancas.Domain.ValueObjects;
using MinhasFinancas.Domain.Interfaces;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Serviço para cálculos de totais.
/// </summary>
public class TotalService : ITotalService
{
    private readonly ITotaisQuery _totaisQuery;

    /// <summary>
    /// Construtor.
    /// </summary>
    public TotalService(ITotaisQuery totaisQuery)
    {
        _totaisQuery = totaisQuery;
    }

    /// <summary>
    /// Obtém totais por pessoa.
    /// </summary>
    public async Task<PagedResult<TotalPorPessoa>> GetTotaisPorPessoaAsync(TotaisPorPessoaFilter? filter = null, PagedRequest? pageRequest = null)
    {
        return await _totaisQuery.GetTotaisPorPessoaAsync(filter, pageRequest).ConfigureAwait(false);
    }

    /// <summary>
    /// Obtém totais por categoria.
    /// </summary>
    public async Task<PagedResult<TotalPorCategoria>> GetTotaisPorCategoriaAsync(TotaisPorCategoriaFilter? filter = null, PagedRequest? pageRequest = null)
    {
        return await _totaisQuery.GetTotaisPorCategoriaAsync(filter, pageRequest).ConfigureAwait(false);
    }
}
