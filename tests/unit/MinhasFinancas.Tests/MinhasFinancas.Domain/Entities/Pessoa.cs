using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Domain.Entities;

/// <summary>
/// Representa uma pessoa no sistema de controle de gastos.
/// </summary>
public class Pessoa : EntityBase
{
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento da pessoa.
    /// </summary>
    [Required]
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Idade calculada da pessoa.
    /// </summary>
    public int Idade => CalcularIdade();

    /// <summary>
    /// Transações associadas à pessoa (propriedade de navegação).
    /// </summary>
    public ICollection<Transacao> Transacoes { get; } = new List<Transacao>();

    /// <summary>
    /// Calcula a idade baseada na data de nascimento.
    /// </summary>
    private int CalcularIdade()
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - DataNascimento.Year;
        if (DataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }

    /// <summary>
    /// Verifica se a pessoa é maior de idade.
    /// </summary>
    public bool EhMaiorDeIdade()
    {
        return Idade >= 18;
    }
}
