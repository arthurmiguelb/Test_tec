using System.Linq.Expressions;

namespace MinhasFinancas.Domain.Interfaces;

/// <summary>
/// Especificação que descreve projeção (select para DTO) e ordenação.
/// </summary>
public interface ISpecification<TEntity, TDto>
{
    /// <summary>
    /// Expressão de projeção de entidade para DTO.
    /// </summary>
    Expression<Func<TEntity, TDto>> Select { get; }

    /// <summary>
    /// Expressão de ordenação (opcional).
    /// </summary>
    Expression<Func<TEntity, object?>>? OrderBy { get; }

    /// <summary>
    /// Se true, aplica OrderByDescending.
    /// </summary>
    bool OrderByDescending { get; }
    /// <summary>
    /// Optional predicate to filter entities before projection.
    /// </summary>
    Expression<Func<TEntity, bool>>? Where { get; }
}
