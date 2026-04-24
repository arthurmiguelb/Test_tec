using System.ComponentModel.DataAnnotations;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.Application.DTOs;

/// <summary>
/// DTO para criação de Categoria.
/// </summary>
public record CreateCategoriaDto
{
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria.
    /// </summary>
    [Required(ErrorMessage = "Finalidade é obrigatória.")]
    public Categoria.EFinalidade Finalidade { get; set; }
}

/// <summary>
/// DTO de resposta para Categoria.
/// </summary>
public record CategoriaDto
{
    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Descricao { get; init; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria.
    /// </summary>
    public Categoria.EFinalidade Finalidade { get; init; }
}