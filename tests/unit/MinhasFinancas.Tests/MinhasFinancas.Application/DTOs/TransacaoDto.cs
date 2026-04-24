using System;
using System.ComponentModel.DataAnnotations;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.Application.DTOs;

/// <summary>
/// DTO para criação de Transação.
/// </summary>
public record CreateTransacaoDto
{
    /// <summary>
    /// Descrição da transação.
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação.
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser positivo.")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo da transação.
    /// </summary>
    [Required(ErrorMessage = "Tipo é obrigatório.")]
    public Transacao.ETipo Tipo { get; set; }

    /// <summary>
    /// Identificador da categoria.
    /// </summary>
    [Required(ErrorMessage = "Categoria é obrigatória.")]
    public Guid CategoriaId { get; set; }

    /// <summary>
    /// Identificador da pessoa.
    /// </summary>
    [Required(ErrorMessage = "Pessoa é obrigatória.")]
    public Guid PessoaId { get; set; }

    /// <summary>
    /// Data em que a transação ocorreu.
    /// </summary>
    [Required(ErrorMessage = "Data da transação é obrigatória.")]
    public DateTime Data { get; set; }
}

/// <summary>
/// DTO de resposta para Transação.
/// </summary>
public record TransacaoDto
{
    /// <summary>
    /// Identificador único da transação.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Descrição da transação.
    /// </summary>
    public string Descricao { get; init; } = string.Empty;

    /// <summary>
    /// Valor da transação.
    /// </summary>
    public decimal Valor { get; init; }

    /// <summary>
    /// Tipo da transação.
    /// </summary>
    public Transacao.ETipo Tipo { get; init; }

    /// <summary>
    /// Identificador da categoria.
    /// </summary>
    public Guid CategoriaId { get; init; }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string CategoriaDescricao { get; init; } = string.Empty;

    /// <summary>
    /// Identificador da pessoa.
    /// </summary>
    public Guid PessoaId { get; init; }

    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string PessoaNome { get; init; } = string.Empty;
    
    /// <summary>
    /// Data em que a transação ocorreu.
    /// </summary>
    public DateTime Data { get; init; }
}