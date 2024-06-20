using Application.Common.Interfaces.Persistence.Repository;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Infrastructure dependency injection configuration.
/// </summary>
public static class DependencyInjection
{

    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services
    )
    {
        services
            .AddRepositories()
            .AddInMemoryDataSource();

        return services;
    }

    /// <summary>
    /// Adds the repositories to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        services.AddScoped<IPatientRepository, PatientRepository>();

        return services;
    }

    /// <summary>
    /// Adds the in-memory datasource to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    private static IServiceCollection AddInMemoryDataSource(
        this IServiceCollection services
    )
    {
        services.AddSingleton<PatientInMemoryContext>(new PatientInMemoryContext());

        return services;
    }
}