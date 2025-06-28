using System.Text.Json.Serialization;

namespace SharedKernel.integration_events;

[JsonDerivedType(typeof(NewAppUserCreatedIntegrationEvent),
    typeDiscriminator: nameof(NewAppUserCreatedIntegrationEvent))]
public interface IIntegrationEvent { }
