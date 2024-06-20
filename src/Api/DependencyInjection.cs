using System.Reflection;
using Api.Common.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Api;

/// <summary>
/// Presentation dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the presentation services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    public static IServiceCollection AddPresentation(
        this IServiceCollection services
    )
    {
        services.AddControllers();

        services
            .AddProblemDetailsFactory()
            .AddMappings()
            .AddSwaggerUiGenInfo();

        return services;
    }

    /// <summary>
    /// Use the presentation application builder to the specified <see cref="IApplicationBuilder"/>.
    /// </summary>
    /// <param name="app">The app collection to use the application builder to.</param>
    public static IApplicationBuilder UsePresentation(
        this IApplicationBuilder app
    )
    {
        app
            .UseSwaggerUIBuilder();

        app
            .UseExceptionHandler("/error");

        return app;
    }

    /// <summary>
    /// Adds a custom problem details factory to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    private static IServiceCollection AddProblemDetailsFactory(
        this IServiceCollection services
    )
    {
        // add the custom problem details factory
        services.AddSingleton<ProblemDetailsFactory, CustomlyProblemDetailsFactory>();

        return services;
    }

    /// <summary>
    /// Adds mapping configurations using Mapster to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    private static IServiceCollection AddMappings(
        this IServiceCollection services
    )
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    /// <summary>
    /// Adds Swagger/OpenAPI configuration to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    private static IServiceCollection AddSwaggerUiGenInfo(
        this IServiceCollection services
    )
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PatientsApi",
                    Description = "A take home assignment from tensova AI Business Solutions Inc. & MedMinder",
                    Version = "v1"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

        return services;
    }

    /// <summary>
    /// Use SwaggerUI application builder to the <see cref="IApplicationBuilder"/>.
    /// </summary>
    /// <param name="app">The app collection to use the application builder to.</param>
    private static IApplicationBuilder UseSwaggerUIBuilder(
        this IApplicationBuilder app
    )
    {
        // Configure the HTTP request pipeline.
        app
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }
}