using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace InfrastructureShared.EfCore;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureDevelopmentExclusive(this DbContextOptionsBuilder options, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.ConfigureWarnings(warning => {
                warning.Log(
                    CoreEventId.FirstWithoutOrderByAndFilterWarning,
                    CoreEventId.RowLimitingOperationWithoutOrderByWarning
                );
            });
        }
    }
}