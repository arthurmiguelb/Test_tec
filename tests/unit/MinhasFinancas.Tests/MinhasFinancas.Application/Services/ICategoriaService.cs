using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Interface para serviço de Categorias.
/// </summary>
public interface ICategoriaService
{
    /// <summary>
    /// Obtém todas as categorias.
    /// </summary>
    Task<PagedResult<CategoriaDto>> GetAllAsync(PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém uma categoria pelo ID.
    /// </summary>
    Task<CategoriaDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto);
}