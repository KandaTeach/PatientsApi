using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Behaviors;

namespace Application
{
    /// <summary>
    /// Application dependency injection configuration.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add Mediator and Validation services to the service collection
            services
                .AddMediator()
                .AddValidation();

            return services;
        }

        /// <summary>
        /// Adds MediatR services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            // Add query and command mediator dynamically
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly)
            );

            return services;
        }

        /// <summary>
        /// Adds FluentValidation services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            // Add validator behavior dynamically
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            // Add query and command validators dynamically
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
