using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record InviteCoAuthorCommand(VokiId VokiId, AppUserId NewCoAuthorId) :
    ICommand<DraftVoki>,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class InviteCoAuthorCommandHandler :
    ICommandHandler<InviteCoAuthorCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public InviteCoAuthorCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<DraftVoki>> Handle(InviteCoAuthorCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId))!;
        ErrOrNothing result = voki.InviteNewCoAuthor(command.NewCoAuthorId);
        if (result.IsErr(out var err))
        {
            return err;
        }

        await _draftVokiRepository.Update(voki);
        return voki;
    }
}