using AbstractTasksDal;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class AddRepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ITaskContext, TaskContext>(options => { options.UseNpgsql(connectionString); });
        using (var provider = services.BuildServiceProvider())
        {
            var userContext = provider.GetRequiredService<TaskContext>();
            if (userContext.Database.GetPendingMigrations().Any())
                userContext.Database.Migrate();
        }
    }
}