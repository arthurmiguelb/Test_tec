// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using MinhasFinancas.Domain.Interfaces;
// using MinhasFinancas.Application.Services;
// using MinhasFinancas.Infrastructure.Data;
// using MinhasFinancas.Infrastructure.Queries;

// namespace MinhasFinancas.IntegrationTests;

// /// <summary>
// /// Factory customizada para testes de integração que configura o banco em memória.
// /// Cada teste terá seu próprio banco isolado usando GUID único.
// /// </summary>
// public class CustomWebApplicationFactory
// {
//     private readonly IServiceProvider _serviceProvider;
//     private readonly string _databaseName = Guid.NewGuid().ToString();

//     public CustomWebApplicationFactory()
//     {
//         var services = new ServiceCollection();

//         // Configurar DbContext com banco em memória
//         services.AddDbContext<MinhasFinancasDbContext>(options =>
//             options.UseInMemoryDatabase(_databaseName));

//         // Registrar serviços da aplicação
//         services.AddScoped<IPessoaService, PessoaService>();
//         services.AddScoped<ICategoriaService, CategoriaService>();
//         services.AddScoped<ITransacaoService, TransacaoService>();
//         services.AddScoped<ITotalService, TotalService>();
//         services.AddScoped<IUnitOfWork, MinhasFinancas.Infrastructure.UnitOfWork.UnitOfWork>();
//         services.AddScoped<ITotaisQuery, TotaisQuery>();
//         services.AddMemoryCache();

//         _serviceProvider = services.BuildServiceProvider();
//     }

//     /// <summary>
//     /// Obtém uma instância do serviço solicitado.
//     /// </summary>
//     public T GetService<T>() where T : notnull
//     {
//         return _serviceProvider.GetRequiredService<T>();
//     }

//     /// <summary>
//     /// Obtém o DbContext para operações diretas no banco.
//     /// </summary>
//     public MinhasFinancasDbContext GetDbContext()
//     {
//         return GetService<MinhasFinancasDbContext>();
//     }
// }

