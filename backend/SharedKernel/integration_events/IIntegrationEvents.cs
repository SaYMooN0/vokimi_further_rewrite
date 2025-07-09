using System.Text.Json.Serialization;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;

namespace SharedKernel.integration_events;

[JsonDerivedType(typeof(NewAppUserCreatedIntegrationEvent), typeDiscriminator: nameof(NewAppUserCreatedIntegrationEvent))]

[JsonDerivedType(typeof(GeneralDraftVokiInitializedIntegrationEvent),
    typeDiscriminator: nameof(GeneralDraftVokiInitializedIntegrationEvent))]
[JsonDerivedType(typeof(ScoringDraftVokiInitializedIntegrationEvent),
    typeDiscriminator: nameof(ScoringDraftVokiInitializedIntegrationEvent))]
[JsonDerivedType(typeof(TierListDraftVokiInitializedIntegrationEvent),
    typeDiscriminator: nameof(TierListDraftVokiInitializedIntegrationEvent))]
public interface IIntegrationEvent { }