namespace NoteFlow.Infrastructure;

using System;
using Data;
using Constants;
using Domain.Common;
using Domain.Interfaces;
using Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructuresService(this IServiceCollection services,
        AppSettings appSettings)
    {
        var environment = Environment.GetEnvironmentVariable(AppConstants.EnvironmentVariable);
        services.AddScoped<IUserRepository, UserRepository>();

        if (environment == AppConstants.TestEnvironmentName)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(AppConstants.TestDatabaseName));
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));


        return services;
    }
}