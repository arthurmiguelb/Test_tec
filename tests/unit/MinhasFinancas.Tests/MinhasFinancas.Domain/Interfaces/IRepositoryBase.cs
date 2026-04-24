using System.Linq.Expressions;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.ValueObjects;

namespace MinhasFinancas.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios.
/// </summary>
public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Obtém pessoas com paginação.
    /// </summary>
    Task<PagedResult<TEntity>> GetPagedAsync(PagedRequest? pageRequest = null);

    /// <summary>
    /// Obtém entidades projetadas para um DTO usando uma specification.
    /// </summary>
    Task<PagedResult<TDto>> GetPagedAsync<TDto>(PagedRequest? pageRequest = null, ISpecification<TEntity, TDto>? specification = null);

    /// <summary>
    /// Obtém todas as entidades.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtém uma entidade pelo ID.
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém entidades que satisfazem uma condição.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Adiciona uma nova entidade.
    /// </summary>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Remove uma entidade.
    /// </summary>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Remove uma entidade pelo ID.
    /// </summary>
    Task DeleteAsync(Guid id);
}
