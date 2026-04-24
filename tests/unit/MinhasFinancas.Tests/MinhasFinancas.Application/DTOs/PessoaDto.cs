using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Application.DTOs;

/// <summary>
/// DTO para criação de Pessoa.
/// </summary>
public record CreatePessoaDto
{
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento da pessoa.
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(PessoaValidation), nameof(PessoaValidation.ValidarDataNascimento))]
    public DateTime DataNascimento { get; set; }
}

/// <summary>
/// DTO para atualização de Pessoa.
/// </summary>
public record UpdatePessoaDto
{
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento da pessoa.
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(PessoaValidation), nameof(PessoaValidation.ValidarDataNascimento))]
    public DateTime DataNascimento { get; set; }
}

/// <summary>
/// DTO de resposta para Pessoa.
/// </summary>
public record PessoaDto
{
    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string Nome { get; init; } = string.Empty;

    /// <summary>
    /// Data de nascimento da pessoa.
    /// </summary>
    public DateTime DataNascimento { get; init; }

    /// <summary>
    /// Idade calculada da pessoa.
    /// </summary>
    public int Idade { get; init; }
}