using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Application.DTOs;

/// <summary>
/// Validações compartilhadas para Pessoa.
/// </summary>
public static class PessoaValidation
{
    /// <summary>
    /// Validação customizada para data de nascimento.
    /// </summary>
    public static ValidationResult? ValidarDataNascimento(DateTime dataNascimento, ValidationContext context)
    {
        if (dataNascimento > DateTime.Today)
        {
            return new ValidationResult("Data de nascimento não pode ser no futuro.");
        }
        return ValidationResult.Success;
    }
}