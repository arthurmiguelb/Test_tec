using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;

namespace MinhasFinancas.Application.Services;

/// <summary>
/// Implementação do serviço de Transações.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Construtor.
    /// </summary>
    public TransacaoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtém todas as transações com paginação.
    /// </summary>
    public async Task<MinhasFinancas.Domain.ValueObjects.PagedResult<TransacaoDto>> GetAllAsync(MinhasFinancas.Domain.ValueObjects.PagedRequest? pageRequest = null)
    {
        var spec = new MinhasFinancas.Application.Specifications.LambdaSpecification<Transacao, TransacaoDto>(
            t => new TransacaoDto
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Data = t.Data,
                Tipo = t.Tipo,
                CategoriaId = t.CategoriaId,
                CategoriaDescricao = t.Categoria != null ? t.Categoria.Descricao : string.Empty,
                PessoaId = t.PessoaId,
                PessoaNome = t.Pessoa != null ? t.Pessoa.Nome : string.Empty
            },
            t => t.Data,
            orderByDescending: true
        );

        var paged = await _unitOfWork.Transacoes.GetPagedAsync(pageRequest, spec);
        return paged;
    }

    /// <summary>
    /// Obtém uma transação pelo ID.
    /// </summary>
    public async Task<TransacaoDto?> GetByIdAsync(Guid id)
    {
        var transacao = await _unitOfWork.Transacoes.GetByIdAsync(id);
        if (transacao == null) return null;

        return new TransacaoDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
                Data = transacao.Data,
            Tipo = transacao.Tipo,
            CategoriaId = transacao.CategoriaId,
            CategoriaDescricao = transacao.Categoria?.Descricao ?? string.Empty,
            PessoaId = transacao.PessoaId,
            PessoaNome = transacao.Pessoa?.Nome ?? string.Empty
        };
    }

    /// <summary>
    /// Cria uma nova transação.
    /// </summary>
    public async Task<TransacaoDto> CreateAsync(CreateTransacaoDto dto)
    {
        // Validar se as entidades relacionadas existem
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(dto.CategoriaId);
        if (categoria == null)
            throw new ArgumentException("Categoria não encontrada.", nameof(dto.CategoriaId));

        var pessoa = await _unitOfWork.Pessoas.GetByIdAsync(dto.PessoaId);
        if (pessoa == null)
            throw new ArgumentException("Pessoa não encontrada.", nameof(dto.PessoaId));

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Data = dto.Data,
            Tipo = dto.Tipo,
            Categoria = categoria,
            Pessoa = pessoa
        };

        await _unitOfWork.Transacoes.AddAsync(transacao);
        await _unitOfWork.SaveChangesAsync();

        return new TransacaoDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            CategoriaId = transacao.CategoriaId,
            CategoriaDescricao = transacao.Categoria?.Descricao ?? string.Empty,
            PessoaId = transacao.PessoaId,
            PessoaNome = transacao.Pessoa?.Nome ?? string.Empty
        };
    }
}