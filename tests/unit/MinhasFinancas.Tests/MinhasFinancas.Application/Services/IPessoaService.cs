using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Interface para serviço de Pessoas.
/// </summary>
public interface IPessoaService
{
    /// <summary>
    /// Obtém todas as pessoas.
    /// </summary>
    Task<PagedResult<PessoaDto>> GetAllAsync(PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém uma pessoa pelo ID.
    /// </summary>
    Task<PessoaDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    Task<PessoaDto> CreateAsync(CreatePessoaDto dto);

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    Task UpdateAsync(Guid id, UpdatePessoaDto dto);

    /// <summary>
    /// Remove uma pessoa pelo ID.
    /// </summary>
    Task DeleteAsync(Guid id);
}