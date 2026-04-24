// Importações necessárias para os testes
using System.ComponentModel.DataAnnotations; 
using System.Text.Json; 
using Xunit; 
using Xunit.Abstractions; 
using MinhasFinancas.Application.DTOs; 
using MinhasFinancas.Domain.Entities; 

namespace MinhasFinancas.Tests; 

public class CategoriaValidationTests
{
    private readonly ITestOutputHelper _output;
    
    /// <param name="output">Helper para escrever logs nos resultados do teste</param>
    public CategoriaValidationTests(ITestOutputHelper output)
    {
        _output = output; 
    }
    private List<ValidationResult> ValidarDTO(object dto)
    {
        var context = new ValidationContext(dto); 
        var results = new List<ValidationResult>(); 

        Validator.TryValidateObject(
            dto,
            context,
            results,
            validateAllProperties: true); // Valida todas as propriedades do objeto

        return results; // Retorna a lista de erros gerada pelo validator
    }

    [Fact(DisplayName = "DTO válido com finalidade Despesa passa na validação")]
    public void DTOValidoComFinalidadeDespesa()
    {
        
        var dto = new CreateCategoriaDto
        {
            Descricao = "Categoria Válida", // Campo obrigatório preenchido corretamente
            Finalidade = Categoria.EFinalidade.Despesa // Enum válido para finalidade
        };

        var results = ValidarDTO(dto);

        // Assert: garante que não existam erros de validação
        Assert.Empty(results);
    }

    [Fact(DisplayName = "DTO com descrição vazia é rejeitado")]
    public void DTOComDescricaoVaziaEhRejeitado()
    {
        // prepara um DTO com um valor inválido para Descricao
        var dto = new CreateCategoriaDto
        {
            Descricao = string.Empty, // String vazia deve falhar na validação
            Finalidade = Categoria.EFinalidade.Despesa
        };

        var results = ValidarDTO(dto);

        // verifica se pelo menos um erro foi retornado
        Assert.NotEmpty(results);

        // verifica se o erro está associado ao campo Descricao
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(CreateCategoriaDto.Descricao)));
    }

    [Fact(DisplayName = "DTO com descrição nula é rejeitado")]
    public void DTOComDescricaoNulaEhRejeitado()
    {
        var dto = new CreateCategoriaDto
        {
            Descricao = null!, // Nulo deve causar erro de validação
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Logs de depuração para inspeção durante o teste
        _output.WriteLine("=== DTO ===");
        _output.WriteLine(JsonSerializer.Serialize(dto));

        var results = ValidarDTO(dto);

        // Loga a quantidade de erros encontrados
        _output.WriteLine($"Quantidade de erros: {results.Count}");

        // Loga cada erro e seus campos associados
        foreach (var erro in results)
        {
            _output.WriteLine($"Erro: {string.Join(",", erro.MemberNames)}");
        }

        Assert.NotEmpty(results);

        // Verifica se o erro está relacionado ao campo Descricao
        Assert.Contains(results, r =>
            r.MemberNames.Contains(nameof(CreateCategoriaDto.Descricao)));

        // Indica que o teste terminou corretamente
        _output.WriteLine("Teste finalizado com sucesso");
    }
}