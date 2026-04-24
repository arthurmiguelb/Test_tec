namespace MinhasFinancas.Domain.Entities;

/// <summary>
/// Classe base para todas as entidades do domínio.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public Guid Id { get; set; } = Guid.CreateVersion7();
}