using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MinhasFinancas.IntegrationTests.Regra1
{
    public class Regra1MenorIdadeReceitasApiTests
    {
        private readonly HttpClient _client;

        public Regra1MenorIdadeReceitasApiTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000") 
            };
        }

        // =========================
        // TESTES
        // =========================

        [Fact(DisplayName = "Menor + Receita → deve falhar")]
        public async Task MenorIdade_Receita_DeveFalhar()
        {
            var pessoaId = await CriarPessoaMenor();
            var categoriaId = await CriarCategoria(0); // Receita

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Receita menor",
                valor = 100,
                tipo = 0, // Receita 
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Menor + Despesa → deve permitir")]
        public async Task MenorIdade_Despesa_DevePermitir()
        {
            var pessoaId = await CriarPessoaMenor();
            var categoriaId = await CriarCategoria(1); // Despesa

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Despesa menor",
                valor = 50,
                tipo = 1, // Despesa 
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "17 anos e 364 dias + Receita → deve falhar")]
        public async Task QuaseMaior_Receita_DeveFalhar()
        {
            var pessoaId = await CriarPessoaQuaseMaior();
            var categoriaId = await CriarCategoria(0); // Receita

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Quase maior",
                valor = 200,
                tipo = 0, // Receita 
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Maior + Receita → deve permitir")]
        public async Task MaiorIdade_Receita_DevePermitir()
        {
            var pessoaId = await CriarPessoaMaior();
            var categoriaId = await CriarCategoria(0); // Receita

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Receita maior",
                valor = 300,
                tipo = 0,
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "18 anos + Receita → deve permitir")]
        public async Task DezoitoAnos_Receita_DevePermitir()
        {
            var pessoaId = await CriarPessoaDezoito();
            var categoriaId = await CriarCategoria(0); // Receita

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", new
            {
                descricao = "Receita 18",
                valor = 150,
                tipo = 0,
                categoriaId,
                pessoaId,
                data = DateTime.Today
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // =========================
        // HELPERS
        // =========================

        private async Task<Guid> CriarPessoaMenor()
        {
            var data = DateTime.Today.AddYears(-17);

            return await CriarPessoa(data);
        }

        private async Task<Guid> CriarPessoaQuaseMaior()
        {
            var data = DateTime.Today.AddYears(-18).AddDays(1);

            return await CriarPessoa(data);
        }

        private async Task<Guid> CriarPessoaMaior()
        {
            var data = DateTime.Today.AddYears(-25);

            return await CriarPessoa(data);
        }

        private async Task<Guid> CriarPessoaDezoito()
        {
            var data = DateTime.Today.AddYears(-18);

            return await CriarPessoa(data);
        }

        private async Task<Guid> CriarPessoa(DateTime dataNascimento)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/pessoas", new
            {
                nome = $"Pessoa_{DateTime.Now.Ticks}",
                dataNascimento
            });

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetGuid();
        }

        private async Task<Guid> CriarCategoria(int finalidade)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/categorias", new
            {
                descricao = $"Categoria_{DateTime.Now.Ticks}",
                finalidade
            });

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetGuid();
        }
    }
}