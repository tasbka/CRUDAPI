using DataAccess.Category;
using DataAccess.Notes;
using DataAccess.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection,  string connectionString)
    {
        serviceCollection.AddScoped<INoteRepository, NoteRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<IPostLikeRepository, PostLikeRepository>(); 
        
        serviceCollection.AddDbContext<AppContext>(x =>
        {
            x.UseNpgsql(connectionString);  
        });
        
        return serviceCollection;
    }
}