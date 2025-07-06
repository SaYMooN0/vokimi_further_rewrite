namespace CoreVokiCreationService.Application.pipeline_behaviors;

public interface IWithMultipleVokiAccessValidationStep
{
    VokiId[] VokiIds { get; }

    Err NoAccessErr => ErrFactory.NoAccess("You do not have access to one or more of the requested vokis");
}