using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record LeaveVokiCreationCommand(VokiId VokiId) :
    ICommand,
    IWithAuthCheckStep;

internal sealed class LeaveVokiCreationCommandHandler : ICommandHandler<LeaveVokiCreationCommand>
{
    private readonly IUserContext _userContext;
    private readonly IDraftVokiRepository _draftVokiRepository;

    public LeaveVokiCreationCommandHandler(IUserContext userContext, IDraftVokiRepository draftVokiRepository) {
        _userContext = userContext;
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOrNothing> Handle(LeaveVokiCreationCommand command, CancellationToken ct) {
        DraftVoki? voki = await _draftVokiRepository.GetById(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Could not leave Voki creation because Voki was not found");
        }

        voki.LeaveVokiCreation(_userContext.AuthenticatedUser);
        await _draftVokiRepository.Update(voki, ct);
        return ErrOrNothing.Nothing;
    }
}