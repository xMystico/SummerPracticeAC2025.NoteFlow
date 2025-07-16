namespace NoteFlow.Application;

using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INoteService, NoteService>();

        return services;
    }
}