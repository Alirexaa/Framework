
using Framework.Abstraction.Persistence;
using Framework.Persistence.EFCore.SqlServer.Builder;
using Framework.Persistence.EFCore.SqlServer.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Persistence.EFCore.SqlServer.DependencyInjection;
public static class Extentions
{
 
    public static IServiceCollection AddSqlServerDbContext<TApplicationDbContext>(this IServiceCollection services,
        Func<ISqlServerOptionsBuilder, ISqlServerOptionsBuilder> buildOptions) where TApplicationDbContext : DbContext
    {
        var options = buildOptions(new SqlServerOptionsBuilder()).Build();
        return services.AddSqlServerDbContext<TApplicationDbContext>(options);
    }

    public static IServiceCollection AddSqlServerDbContext<TApplicationDbContext>(this IServiceCollection services, SqlServerOptions options) where TApplicationDbContext : DbContext
    {
        services
            .AddSingleton(options)
            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
            .AddScoped(typeof(IRepository<>), typeof(Repository<,>))
            //.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork<>))    
            .AddDbContext<TApplicationDbContext>(opt =>
            {
                if (options.UseInMemoryDatabase)
                {
                    opt.UseInMemoryDatabase(options.InMemoryDatabaseName);
                }
                else
                {
                    opt.UseSqlServer(options.ConnectionString);
                }
            });
        return services;
    }
}

