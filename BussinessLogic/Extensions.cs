using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BussinessLogic;

public static class Extensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INoteService, NoteService>();
        return serviceCollection;
    }
}