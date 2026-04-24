// Importações necessárias para os testes
using Moq;
using MinhasFinancas.Application.DTOs; 
using MinhasFinancas.Application.Services; 
using MinhasFinancas.Domain.Entities; 
using MinhasFinancas.Domain.Interfaces; 
using MinhasFinancas.Domain.ValueObjects; 

namespace MinhasFinancas.Tests; 
public class TransacaoServiceTests
{
    // Campos privados para os mocks - um para cada dependência do serviço
    private readonly Mock<IUnitOfWork> _unitOfWorkMock; 
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryMock; 
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock; 
    private readonly Mock<IPessoaRepository> _pessoaRepositoryMock; 
    private readonly TransacaoService _service; 
        public TransacaoServiceTests()
    {
        // Cria os mocks para todas as dependências
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _pessoaRepositoryMock = new Mock<IPessoaRepository>();

        _unitOfWorkMock.SetupGet(u => u.Transacoes).Returns(_transacaoRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(u => u.Categorias).Returns(_categoriaRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(u => u.Pessoas).Returns(_pessoaRepositoryMock.Object);

        // Cria a instância do serviço com o mock da unidade de trabalho
        _service = new TransacaoService(_unitOfWorkMock.Object);
    }

    /// Teste que verifica se uma transação válida é criada com sucesso.

    [Fact(DisplayName = "Deve criar transação válida com sucesso")]
    public async Task DeveCriarTransacaoValida()
    {
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Cria objetos de domínio simulados
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };
        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "João",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // Cria o DTO de entrada para o serviço
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra no mercado",
            Valor = 150.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        var transacaoId = Guid.NewGuid();

        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoa);

        _transacaoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transacao>()))
            .Callback<Transacao>(t => t.Id = transacaoId)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.CreateAsync(dto);

        // Verificação dos resultados
        // Verifica que o resultado não é nulo
        Assert.NotNull(result);

        // Verifica que todos os campos foram copiados corretamente do DTO para a entidade
        Assert.Equal(dto.Descricao, result.Descricao);
        Assert.Equal(dto.Valor, result.Valor);
        Assert.Equal(dto.Tipo, result.Tipo);
        Assert.Equal(categoriaId, result.CategoriaId);
        Assert.Equal(pessoaId, result.PessoaId);

        // Verifica que o ID foi gerado corretamente pelo callback do mock
        Assert.Equal(transacaoId, result.Id);
    }


    /// Teste que verifica se uma transação com categoria inexistente é rejeitada.
    [Fact(DisplayName = "Deve rejeitar transação com categoria inexistente")]
    public async Task DeveRejeitarTransacaoComCategoriaInexistente()
    {
        // Preparação dos dados de teste
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(), // ID que não existe
            PessoaId = Guid.NewGuid(), // ID que não existe
            Data = DateTime.Today
        };
        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(dto.CategoriaId)).ReturnsAsync((Categoria?)null);

        // Verifica que uma ArgumentException é lançada
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));

        // Verifica que a mensagem da exception contém o texto esperado
        Assert.Contains("Categoria não encontrada", exception.Message);
    }


    /// Teste que verifica se uma transação com pessoa inexistente é rejeitada.
    /// Este teste valida a validação de existência de pessoa.
    [Fact(DisplayName = "Deve rejeitar transação com pessoa inexistente")]
    public async Task DeveRejeitarTransacaoComPessoaInexistente()
    {
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Cria uma categoria válida
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Cria o DTO com pessoa inexistente
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };
        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync((Pessoa?)null);
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        Assert.Contains("Pessoa não encontrada", exception.Message);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve rejeitar receita para menor de idade")]
    public async Task DeveRejeitarReceitaParaMenorDeIdade()
    {
        // cria categoria e pessoa para um cenário de receita
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Categoria de receita válida
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Pessoa menor de idade, inválida para registrar receita
        var pessoaMenor = new Pessoa
        {
            Id = pessoaId,
            Nome = "João (menor)",
            DataNascimento = new DateTime(2010, 1, 1)
        };

        // DTO de transação de receita para o menor
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 1000.00m,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        // Mock retorna a categoria válida e a pessoa menor de idade
        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoaMenor);

        // espera uma InvalidOperationException por regra de idade
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        Assert.Contains("Menores de 18 anos não podem registrar receitas", exception.Message);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve aceitar receita para maior de idade")]
    public async Task DeveAceitarReceitaParaMaiorDeIdade()
    {
        //cria categoria e pessoa para um cenário válido de receita
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Categoria de receita válida
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Pessoa maior de idade, válida para registrar receita
        var pessoaMaior = new Pessoa
        {
            Id = pessoaId,
            Nome = "João (adulto)",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // DTO de transação de receita válida
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 2000.00m,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        var transacaoId = Guid.NewGuid();

        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoaMaior);

        // Simula persistência da transação e atribui um ID gerado
        _transacaoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transacao>()))
            .Callback<Transacao>(t => t.Id = transacaoId)
            .Returns(Task.CompletedTask);

        // Simula commit bem-sucedido no banco de dados
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // chama o serviço para criar a transação
        var result = await _service.CreateAsync(dto);

        // confirma que a transação foi criada com tipo correto
        Assert.NotNull(result);
        Assert.Equal(Transacao.ETipo.Receita, result.Tipo);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve rejeitar despesa em categoria de receita")]
    public async Task DeveRejeitarDespesaEmCategoriaDeReceita()
    {
        // configuração de cenário inválido
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Categoria configurada para tipo Receita
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Pessoa válida para o cenário
        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "João",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // DTO tenta criar uma despesa em categoria de receita
        var dto = new CreateTransacaoDto
        {
            Descricao = "Erro - despesa em categoria de receita",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        // Mocks retornam categoria e pessoa válidas
        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoa);

        // espera InvalidOperationException pela regra de finalidade
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        Assert.Contains("Não é possível registrar despesa em categoria de receita", exception.Message);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve rejeitar receita em categoria de despesa")]
    public async Task DeveRejeitarReceitaEmCategoriaDeDespesa()
    {
        // configuração para categoria de despesa
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Categoria com finalidade Despesa
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Pessoa válida no cenário
        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "João",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // DTO tenta criar receita em categoria de despesa
        var dto = new CreateTransacaoDto
        {
            Descricao = "Erro - receita em categoria de despesa",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        // Mocks retornam categoria e pessoa válidas
        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoa);

        // espera exception pela regra de finalidade
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        Assert.Contains("Não é possível registrar receita em categoria de despesa", exception.Message);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve aceitar despesa em categoria de despesa")]
    public async Task DeveAceitarDespesaEmCategoriaDeDespesa()
    {
        // cenário válido para despesa
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        // Categoria com finalidade Despesa
        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Pessoa válida
        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "João",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // DTO de despesa válida
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra no mercado",
            Valor = 150.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        var transacaoId = Guid.NewGuid();

        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoa);

        // Simula a adição da transação e define o ID de retorno
        _transacaoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transacao>()))
            .Callback<Transacao>(t => t.Id = transacaoId)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // chama o serviço para criar a despesa
        var result = await _service.CreateAsync(dto);

        // verifica que a transação foi criada com sucesso e é do tipo Despesa
        Assert.NotNull(result);
        Assert.Equal(Transacao.ETipo.Despesa, result.Tipo);
    }

    [Fact(DisplayName = "REGRA DE NEGÓCIO: Deve aceitar ambos tipos em categoria 'Ambas'")]
    public async Task DeveAceitarAmbosTiposEmCategoriaAmbas()
    {
        // cenário onde a categoria aceita ambos os tipos de transação
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Diversos",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "João",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // DTO para despesa
        var dtoDespesa = new CreateTransacaoDto
        {
            Descricao = "Compra diversos",
            Valor = 50.00m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        // DTO para receita
        var dtoReceita = new CreateTransacaoDto
        {
            Descricao = "Receita diversos",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Data = DateTime.Today
        };

        _categoriaRepositoryMock.Setup(r => r.GetByIdAsync(categoriaId)).ReturnsAsync(categoria);
        _pessoaRepositoryMock.Setup(r => r.GetByIdAsync(pessoaId)).ReturnsAsync(pessoa);

        // Simula a adição de transações sem lançar erro
        _transacaoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transacao>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act & Assert - Despesa
        var resultDespesa = await _service.CreateAsync(dtoDespesa);
        Assert.NotNull(resultDespesa);
        Assert.Equal(Transacao.ETipo.Despesa, resultDespesa.Tipo);

        // Act & Assert - Receita
        var resultReceita = await _service.CreateAsync(dtoReceita);
        Assert.NotNull(resultReceita);
        Assert.Equal(Transacao.ETipo.Receita, resultReceita.Tipo);
    }

    [Fact(DisplayName = "Deve buscar transação por ID existente")]
    public async Task DeveBuscarTransacaoPorIdExistente()
    {
        // prepara os dados para a busca
        var transacaoId = Guid.NewGuid();
        var categoria = new Categoria { Id = Guid.NewGuid(), Descricao = "Alimentação" };
        var pessoa = new Pessoa { Id = Guid.NewGuid(), Nome = "João" };

        // Cria uma transação com informações básicas
        var transacao = new Transacao
        {
            Id = transacaoId,
            Descricao = "Compra",
            Valor = 100.00m,
            Tipo = Transacao.ETipo.Despesa,
            Data = DateTime.Today
        };

        // Usa reflexão para preencher propriedades que podem não ter setter público
        typeof(Transacao).GetProperty("Categoria")?.SetValue(transacao, categoria);
        typeof(Transacao).GetProperty("Pessoa")?.SetValue(transacao, pessoa);

        // Mock do repositório retorna a transação quando solicitado pelo ID
        _transacaoRepositoryMock.Setup(r => r.GetByIdAsync(transacaoId)).ReturnsAsync(transacao);

        // busca a transação por ID
        var result = await _service.GetByIdAsync(transacaoId);

        // valida que a transação foi retornada corretamente
        Assert.NotNull(result);
        Assert.Equal(transacaoId, result!.Id);
        Assert.Equal("Compra", result.Descricao);
        Assert.Equal(100.00m, result.Valor);
    }

    [Fact(DisplayName = "Deve retornar null quando transação não existe")]
    public async Task DeveRetornarNullQuandoTransacaoNaoExiste()
    {
        // agenda um ID inexistente
        var transacaoId = Guid.NewGuid();

        // Mock do repositório retorna null quando a transação não é encontrada
        _transacaoRepositoryMock.Setup(r => r.GetByIdAsync(transacaoId)).ReturnsAsync((Transacao?)null);

        // chama o serviço para buscar transação inexistente
        var result = await _service.GetByIdAsync(transacaoId);

        // valida que o resultado é null
        Assert.Null(result);
    }

    [Fact(DisplayName = "Deve listar transações com paginação")]
    public async Task DeveListarTransacoesComPaginacao()
    {
        var pageRequest = new PagedRequest { Page = 1, PageSize = 10 };
        var expectedResult = new PagedResult<TransacaoDto>
        {
            Items = new[]
            {
                new TransacaoDto
                {
                    Id = Guid.NewGuid(),
                    Descricao = "Transação 1",
                    Valor = 100.00m,
                    Tipo = Transacao.ETipo.Despesa,
                    Data = DateTime.Today,
                    CategoriaId = Guid.NewGuid(),
                    CategoriaDescricao = "Alimentação",
                    PessoaId = Guid.NewGuid(),
                    PessoaNome = "João"
                }
            },
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _transacaoRepositoryMock
            .Setup(r => r.GetPagedAsync(pageRequest, It.IsAny<MinhasFinancas.Application.Specifications.LambdaSpecification<Transacao, TransacaoDto>>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetAllAsync(pageRequest);

        // Assert
        Assert.Equal(expectedResult.TotalCount, result.TotalCount);
        Assert.Single(result.Items);
        Assert.Equal("Transação 1", result.Items.First().Descricao);
    }
}
