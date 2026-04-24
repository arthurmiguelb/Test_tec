using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Interface para serviço de totais.
/// </summary>
public interface ITotalService
{
    /// <summary>
    /// Obtém totais por pessoa.
    /// </summary>
    Task<PagedResult<TotalPorPessoa>> GetTotaisPorPessoaAsync(TotaisPorPessoaFilter? filter = null, PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém totais por categoria.
    /// </summary>
    Task<PagedResult<TotalPorCategoria>> GetTotaisPorCategoriaAsync(TotaisPorCategoriaFilter? filter = null, PagedRequest? pageRequest = null);
}