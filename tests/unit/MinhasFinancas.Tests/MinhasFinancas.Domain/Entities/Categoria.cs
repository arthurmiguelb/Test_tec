using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Domain.Entities;

/// <summary>
/// Representa uma categoria de transação.
/// </summary>
public class Categoria : EntityBase
{
    /// <summary>
    /// Enum para a finalidade da categoria.
    /// </summary>
    public enum EFinalidade
    {
        Despesa,
        Receita,
        Ambas
    }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria (Despesa, Receita ou Ambas).
    /// </summary>
    [Required]
    public EFinalidade Finalidade { get; set; }

    /// <summary>
    /// Transações associadas à categoria (propriedade de navegação).
    /// </summary>
    public ICollection<Transacao> Transacoes { get; } = new List<Transacao>();

    /// <summary>
    /// Valida se a categoria permite o tipo de transação especificado.
    /// </summary>
    public bool PermiteTipo(Transacao.ETipo tipo)
    {
        return Finalidade switch
        {
            EFinalidade.Despesa => tipo == Transacao.ETipo.Despesa,
            EFinalidade.Receita => tipo == Transacao.ETipo.Receita,
            EFinalidade.Ambas => true,
            _ => false
        };
    }
}
