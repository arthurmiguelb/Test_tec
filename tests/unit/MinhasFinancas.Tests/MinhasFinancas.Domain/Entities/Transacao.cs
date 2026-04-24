using System;
using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Domain.Entities;

/// <summary>
/// Representa uma transação financeira.
/// </summary>
public class Transacao : EntityBase
{
    /// <summary>
    /// Enum para o tipo de transação.
    /// </summary>
    public enum ETipo
    {
        Despesa,
        Receita
    }

    /// <summary>
    /// Descrição da transação.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação (deve ser positivo).
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo da transação (Despesa ou Receita).
    /// </summary>
    [Required]
    public ETipo Tipo { get; set; }

    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    [Required]
    public Guid CategoriaId { get; private set; }

    /// <summary>
    /// Identificador da pessoa associada.
    /// </summary>
    [Required]
    public Guid PessoaId { get; private set; }

    /// <summary>
    /// Data em que a transação ocorreu.
    /// </summary>
    [Required]
    public DateTime Data { get; set; } = DateTime.Today;
    /// <summary>
    /// Categoria associada (propriedade de navegação).
    /// </summary>
    public Categoria? Categoria
    {
        get => _categoria;
        internal set
        {
            _categoria = value;
            if (value != null)
            {
                CategoriaId = value.Id;
                if (!value.PermiteTipo(Tipo))
                {
                    throw new InvalidOperationException(
                        Tipo == ETipo.Despesa
                            ? "Não é possível registrar despesa em categoria de receita."
                            : "Não é possível registrar receita em categoria de despesa.");
                }
            }
        }
    }

    private Categoria? _categoria;

    /// <summary>
    /// Pessoa associada (propriedade de navegação).
    /// </summary>
    public Pessoa? Pessoa
    {
        get => _pessoa;
        internal set
        {
            _pessoa = value;
            if (value != null)
            {
                PessoaId = value.Id;
                if (Tipo == ETipo.Receita && !value.EhMaiorDeIdade())
                {
                    throw new InvalidOperationException("Menores de 18 anos não podem registrar receitas.");
                }
            }
        }
    }

    private Pessoa? _pessoa;
}
