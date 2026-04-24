using System.Linq.Expressions;
using MinhasFinancas.Domain.ValueObjects;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Implementação do serviço de Pessoas.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Construtor.
    /// </summary>
    public PessoaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtém todas as pessoas.
    /// </summary>
    public async Task<PagedResult<PessoaDto>> GetAllAsync(PagedRequest? pageRequest = null)
    {
        Expression<Func<Pessoa, bool>>? where = null;
        if (!string.IsNullOrWhiteSpace(pageRequest?.Search))
        {
            var search = pageRequest!.Search!;
            where = p => p.Nome.Contains(search);
        }

        var spec = new MinhasFinancas.Application.Specifications.LambdaSpecification<Pessoa, PessoaDto>(
            p => new PessoaDto
            {
                Id = p.Id,
                Nome = p.Nome,
                DataNascimento = p.DataNascimento,
                Idade = p.Idade
            },
            p => p.Nome,
            false,
            where
        );

        var paged = await _unitOfWork.Pessoas.GetPagedAsync(pageRequest, spec).ConfigureAwait(false);
        return paged;
    }

    /// <summary>
    /// Obtém uma pessoa pelo ID.
    /// </summary>
    public async Task<PessoaDto?> GetByIdAsync(Guid id)
    {
        var pessoa = await _unitOfWork.Pessoas.GetByIdAsync(id).ConfigureAwait(false);
        if (pessoa == null) return null;
        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            DataNascimento = pessoa.DataNascimento,
            Idade = pessoa.Idade
        };
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    public async Task<PessoaDto> CreateAsync(CreatePessoaDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento
        };

        await _unitOfWork.Pessoas.AddAsync(pessoa).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            DataNascimento = pessoa.DataNascimento,
            Idade = pessoa.Idade
        };
    }

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    public async Task UpdateAsync(Guid id, UpdatePessoaDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var pessoa = await _unitOfWork.Pessoas.GetByIdAsync(id).ConfigureAwait(false);
        if (pessoa == null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        pessoa.Nome = dto.Nome;
        pessoa.DataNascimento = dto.DataNascimento;

        await _unitOfWork.Pessoas.UpdateAsync(pessoa).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Remove uma pessoa pelo ID.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.Pessoas.DeleteAsync(id).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
