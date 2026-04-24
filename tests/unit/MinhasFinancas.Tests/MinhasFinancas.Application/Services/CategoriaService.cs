using System.Linq.Expressions;
using MinhasFinancas.Domain.ValueObjects;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Implementação do serviço de Categorias.
/// </summary>
public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Construtor.
    /// </summary>
    public CategoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtém todas as categorias.
    /// </summary>
    public async Task<PagedResult<CategoriaDto>> GetAllAsync(PagedRequest? pageRequest = null)
    {
        Expression<Func<Categoria, bool>>? where = null;
        if (!string.IsNullOrWhiteSpace(pageRequest?.Search))
        {
            var search = pageRequest!.Search!;
            where = c => c.Descricao.Contains(search);
        }

        var spec = new MinhasFinancas.Application.Specifications.LambdaSpecification<Categoria, CategoriaDto>(
            c => new CategoriaDto
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade
            },
            c => c.Descricao,
            false,
            where
        );

        var paged = await _unitOfWork.Categorias.GetPagedAsync(pageRequest, spec).ConfigureAwait(false);
        return paged;
    }

    /// <summary>
    /// Obtém uma categoria pelo ID.
    /// </summary>
    public async Task<CategoriaDto?> GetByIdAsync(Guid id)
    {
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(id).ConfigureAwait(false);
        if (categoria == null) return null;

        return new CategoriaDto
        {
            Id = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade
        };
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    public async Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var categoria = new Categoria
        {
            Descricao = dto.Descricao,
            Finalidade = dto.Finalidade
        };

        await _unitOfWork.Categorias.AddAsync(categoria).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return new CategoriaDto
        {
            Id = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade
        };
    }
}
