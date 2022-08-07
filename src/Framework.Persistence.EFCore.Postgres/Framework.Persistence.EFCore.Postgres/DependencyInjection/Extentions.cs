
using Framework.Abstraction.Persistence;
using Framework.Persistence.EFCore.Internals;
using Framework.Persistence.EFCore.Postgres.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Persistence.EFCore.Postgres.DependencyInjection;
public static class Extentions
{
 
    public static IServiceCollection AddPostgresDbContext<TApplicationDbContext>(this IServiceCollection services,
        Func<IPostgresOptionsBuilder, IPostgresOptionsBuilder> buildOptions) where TApplicationDbContext : DbContext
    {
        var options = buildOptions(new PostgresOptionsBuilder()).Build();
        return services.AddPostgresDbContext<TApplicationDbContext>(options);
    }

    public static IServiceCollection AddPostgresDbContext<TApplicationDbContext>(this IServiceCollection services, PostgresOptions options) where TApplicationDbContext : DbContext
    {
        services
            .AddSingleton(options)
            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork<>))    
            .AddDbContext<TApplicationDbContext>(opt =>
            {
                if (options.UseInMemoryDatabase)
                {
                    opt.UseInMemoryDatabase(options.InMemoryDatabaseName);
                }
                else
                {
                    opt.UseNpgsql(options.ConnectionString);
                }
            });
        return services;
    }
}

