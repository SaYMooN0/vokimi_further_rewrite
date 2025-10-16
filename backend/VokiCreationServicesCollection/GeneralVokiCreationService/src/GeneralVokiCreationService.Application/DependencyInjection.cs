using ApplicationShared;
using GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;
using Microsoft.Extensions.DependencyInjection;
using VokiCreationServicesLib.Application;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        services.AddLibStepHandlers();
        services.AddSingleton<DraftVokiAnswerDataSavingService>();

        return services;
    }
}