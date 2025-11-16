using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INoteRepository, NoteRepository>();
        serviceCollection.AddDbContext<AppContext>(x=>
        {
            x.UseNpgsql(connectionString: "Host=localhost;Database=NateDb;Username=postgres;Password=1234");
        });
        return serviceCollection;
    }
}