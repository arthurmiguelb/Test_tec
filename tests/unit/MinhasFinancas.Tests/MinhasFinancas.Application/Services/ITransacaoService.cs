using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Interface para serviço de Transações.
/// </summary>
public interface ITransacaoService
{
    /// <summary>
    /// Obtém todas as transações.
    /// </summary>
    Task<PagedResult<TransacaoDto>> GetAllAsync(PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém uma transação pelo ID.
    /// </summary>
    Task<TransacaoDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Cria uma nova transação.
    /// </summary>
    Task<TransacaoDto> CreateAsync(CreateTransacaoDto dto);
}