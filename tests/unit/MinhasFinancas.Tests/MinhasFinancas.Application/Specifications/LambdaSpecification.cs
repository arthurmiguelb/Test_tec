using System.Linq.Expressions;
using MinhasFinancas.Domain.Interfaces;

namespace MinhasFinancas.Application.Specifications;

/// <summary>
/// Implementação simples de specification baseada em expressões lambda.
/// </summary>
public sealed class LambdaSpecification<TEntity, TDto> : ISpecification<TEntity, TDto>
{
    public LambdaSpecification(
        Expression<Func<TEntity, TDto>> select,
        Expression<Func<TEntity, object?>>? orderBy = null,
        bool orderByDescending = false,
        Expression<Func<TEntity, bool>>? where = null)
    {
        Select = select ?? throw new ArgumentNullException(nameof(select));
        OrderBy = orderBy;
        OrderByDescending = orderByDescending;
        Where = where;
    }

    public Expression<Func<TEntity, TDto>> Select { get; }

    public Expression<Func<TEntity, object?>>? OrderBy { get; }

    public bool OrderByDescending { get; }
    public Expression<Func<TEntity, bool>>? Where { get; }
}
