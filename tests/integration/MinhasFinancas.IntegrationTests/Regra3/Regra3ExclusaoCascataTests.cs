using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

public class Regra3ExclusaoCascataApiTests
{
    private readonly HttpClient _client;

    public Regra3ExclusaoCascataApiTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/v1") // ajuste sua API
        };
    }

    [Fact(DisplayName = "Deletar pessoa → deve remover transações mas manter categorias (via API)")]
    public async Task DeletarPessoa_DeveRemoverTransacoes_ManterCategorias()
    {
        // ========================
        // ARRANGE
        // ========================

        // 1. Criar pessoa
        var pessoaResponse = await _client.PostAsJsonAsync("/pessoas", new
        {
            nome = "Teste API",
            dataNascimento = "1990-01-01"
        });

        pessoaResponse.EnsureSuccessStatusCode();

        var pessoa = await pessoaResponse.Content.ReadFromJsonAsync<dynamic>();
        int pessoaId = pessoa.id;

        // 2. Criar categoria
        var categoriaResponse = await _client.PostAsJsonAsync("/categorias", new
        {
            descricao = "Despesa teste",
            finalidade = 0
        });

        categoriaResponse.EnsureSuccessStatusCode();

        var categoria = await categoriaResponse.Content.ReadFromJsonAsync<dynamic>();
        int categoriaId = categoria.id;

        // 3. Criar transação
        var transacaoResponse = await _client.PostAsJsonAsync("/transacoes", new
        {
            descricao = "Transação teste",
            valor = 100,
            tipo = 0,
            categoriaId = categoriaId,
            pessoaId = pessoaId,
            data = DateTime.Today
        });

        transacaoResponse.EnsureSuccessStatusCode();

        // ========================
        // ACT
        // ========================

        // Deletar pessoa
        var deleteResponse = await _client.DeleteAsync($"/pessoas/{pessoaId}");
        deleteResponse.EnsureSuccessStatusCode();

        // ========================
        // ASSERT
        // ========================

        // 1. Pessoa deve não existir
        var pessoaGet = await _client.GetAsync($"/pessoas/{pessoaId}");
        pessoaGet.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // 2. Transações devem ter sido removidas
        var transacoesResponse = await _client.GetAsync($"/transacoes?pessoaId={pessoaId}");
        transacoesResponse.EnsureSuccessStatusCode();

        var transacoes = await transacoesResponse.Content.ReadFromJsonAsync<List<object>>();
        transacoes.Should().BeEmpty();

        // 3. Categoria deve continuar existindo
        var categoriaGet = await _client.GetAsync($"/categorias/{categoriaId}");
        categoriaGet.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}