namespace MinhasFinancas.Domain.Interfaces;

/// <summary>
/// Interface para Unit of Work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositório de Pessoas.
    /// </summary>
    IPessoaRepository Pessoas { get; }

    /// <summary>
    /// Repositório de Categorias.
    /// </summary>
    ICategoriaRepository Categorias { get; }

    /// <summary>
    /// Repositório de Transações.
    /// </summary>
    ITransacaoRepository Transacoes { get; }

    /// <summary>
    /// Salva todas as mudanças.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Inicia uma transação.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma a transação.
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Reverte a transação.
    /// </summary>
    Task RollbackAsync();
}