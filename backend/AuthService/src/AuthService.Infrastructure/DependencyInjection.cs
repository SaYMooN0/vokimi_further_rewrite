﻿using ApplicationShared;
using AuthService.Application.abstractions;
using AuthService.Application.common.repositories;
using AuthService.Infrastructure.auth;
using AuthService.Infrastructure.background_services;
using AuthService.Infrastructure.email_service;
using AuthService.Infrastructure.persistence;
using AuthService.Infrastructure.persistence.repositories;
using Infrastructure.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.auth;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        return services
            .AddDefaultServices()
            .AddPersistence(configuration, env)
            .AddAuth(configuration)
            .AddPasswordHasherAndTokenGenerator()
            .AddEmailService(configuration)
            .AddFrontendConfig(configuration)
            .AddMassTransitWithIntegrationEventHandlers(configuration, typeof(Application.DependencyInjection).Assembly)
            .AddIntegrationEventsPublisher()
            .AddBackgroundServices();
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddDateTimeProvider()
        .AddDomainEventsPublisher()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

    private static IServiceCollection AddPasswordHasherAndTokenGenerator(this IServiceCollection services) => services
        .AddScoped<IPasswordHasher, PasswordHasher>()
        .AddScoped<ITokenGenerator, TokenGenerator>();

    private static IServiceCollection AddIntegrationEventsPublisher(this IServiceCollection services) {
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services
            .Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        string dbConnectionString = configuration.GetConnectionString("AuthServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");

        services.AddDbContext<AuthDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                options.ConfigureDevelopmentExclusive(env);
            }
        );

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IUnconfirmedUsersRepository, UnconfirmedUsersRepository>();

        return services;
    }

    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration) {
        var emailServiceConfig = configuration.GetSection("EmailServiceConfig").Get<EmailServiceConfig>();
        if (emailServiceConfig is null) {
            throw new Exception("Email service is not configured");
        }

        services.AddSingleton(emailServiceConfig);
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    private static IServiceCollection AddFrontendConfig(
        this IServiceCollection services, IConfiguration configuration
    ) {
        var frontendConfig = configuration.GetSection("FrontendConfig").Get<FrontendConfig>();
        if (frontendConfig is null) {
            throw new Exception("Email service is not configured");
        }

        services.AddSingleton(frontendConfig);
        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services) {
        services.AddHostedService<UnconfirmedUsersCleanupBackgroundService>();
        return services;
    }
}