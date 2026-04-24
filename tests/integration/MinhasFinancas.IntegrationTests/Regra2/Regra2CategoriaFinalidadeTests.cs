using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MinhasFinancas.IntegrationTests.Regra2
{
    public class Regra2CategoriaFinalidadeApiTests
    {
        private readonly HttpClient _client;

        public Regra2CategoriaFinalidadeApiTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000") 
            };
        }

        [Fact(DisplayName = "Categoria Receita usada em transação Despesa → deve falhar")]
        public async Task CategoriaReceita_TransacaoDespesa_DeveFalhar()
        {
            var pessoaId = await CriarPessoa();
            var categoriaId = await CriarCategoria(0); // Receita

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Inválido",
                valor = 100,
                tipo = 1, // Despesa 
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Categoria Despesa usada em transação Receita → deve falhar")]
        public async Task CategoriaDespesa_TransacaoReceita_DeveFalhar()
        {
            var pessoaId = await CriarPessoa();
            var categoriaId = await CriarCategoria(1); // Despesa

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Inválido",
                valor = 200,
                tipo = 0, // Receita 
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Categoria Ambas usada em Receita → deve permitir")]
        public async Task CategoriaAmbas_Receita_DevePermitir()
        {
            var pessoaId = await CriarPessoa();
            var categoriaId = await CriarCategoria(2); // Ambas

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Valido",
                valor = 300,
                tipo = 0, // Receita
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "Categoria Ambas usada em Despesa → deve permitir")]
        public async Task CategoriaAmbas_TransacaoDespesa_DevePermitir()
        {
            var pessoaId = await CriarPessoa();
            var categoriaId = await CriarCategoria(2); // Ambas

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Valido",
                valor = 150,
                tipo = 1, // Despesa
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // ========================
        // HELPERS
        // ========================

        private async Task<Guid> CriarPessoa()
        {
            var response = await _client.PostAsJsonAsync("/api/v1/pessoas", new
            {
                nome = $"Teste_{DateTime.Now.Ticks}",
                dataNascimento = "1990-01-01"
            });

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetGuid();
        }

        private async Task<Guid> CriarCategoria(int finalidade)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/categorias", new
            {
                descricao = $"Cat_{DateTime.Now.Ticks}",
                finalidade = finalidade
            });

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetGuid();
        }
    }
}