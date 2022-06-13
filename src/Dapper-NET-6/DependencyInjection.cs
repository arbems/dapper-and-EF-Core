using DapperAndEFCore.Interfaces;
using DapperAndEFCore.Persistence;
using DapperAndEFCore.Persistence.Connections;
using DapperAndEFCore.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<ApplicationContext>());

        services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
        services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();

        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient(typeof(IReadRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IPostRepository, PostRepository>();

        return services;
    }
}
