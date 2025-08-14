using System.Text.Json.Serialization;
using SharedKernel.integration_events.draft_vokis;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;
using SharedKernel.integration_events.voki_publishing;

namespace SharedKernel.integration_events;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
// @formatter:off
[JsonDerivedType(typeof(NewAppUserCreatedIntegrationEvent),typeDiscriminator: nameof(NewAppUserCreatedIntegrationEvent))]

[JsonDerivedType(typeof(GeneralDraftVokiInitializedIntegrationEvent),typeDiscriminator: nameof(GeneralDraftVokiInitializedIntegrationEvent))]
[JsonDerivedType(typeof(ScoringDraftVokiInitializedIntegrationEvent),typeDiscriminator: nameof(ScoringDraftVokiInitializedIntegrationEvent))]
[JsonDerivedType(typeof(TierListDraftVokiInitializedIntegrationEvent),typeDiscriminator: nameof(TierListDraftVokiInitializedIntegrationEvent))]

[JsonDerivedType(typeof(DraftVokiCoverUpdatedIntegrationEvent),typeDiscriminator: nameof(DraftVokiCoverUpdatedIntegrationEvent))]
[JsonDerivedType(typeof(DraftVokiNameUpdatedIntegrationEvent),typeDiscriminator: nameof(DraftVokiNameUpdatedIntegrationEvent))]

[JsonDerivedType(typeof(BaseVokiPublishedIntegrationEvent),typeDiscriminator: nameof(BaseVokiPublishedIntegrationEvent))]
[JsonDerivedType(typeof(GeneralVokiPublishedIntegrationEvent),typeDiscriminator: nameof(GeneralVokiPublishedIntegrationEvent))]
// @formatter:on
public interface IIntegrationEvent { }