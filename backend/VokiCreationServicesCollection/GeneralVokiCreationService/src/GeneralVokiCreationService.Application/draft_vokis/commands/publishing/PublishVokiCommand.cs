
using SharedKernel;
using VokiCreationServicesLib.Application;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.publishing;

public record class PublishVokiWithNoIssuesCommand(VokiId VokiId) :
    ICommand<VokiSuccessfullyPublishedResult>,
    IWithAuthCheckStep;

internal sealed class PublishVokiWithNoIssuesCommandHandler :
    ICommandHandler<PublishVokiWithNoIssuesCommand, VokiSuccessfullyPublishedResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserCtxProvider _userCtxProvider;

    public PublishVokiWithNoIssuesCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IDateTimeProvider dateTimeProvider,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _dateTimeProvider = dateTimeProvider;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiSuccessfullyPublishedResult>> Handle(
        PublishVokiWithNoIssuesCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsAndResultsForUpdate(command.VokiId, ct))!;

        var publishingRes = voki.PublishWithNoIssues(command.UserCtx(_userCtxProvider), _dateTimeProvider.UtcNow);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Delete(voki, ct);
        return new VokiSuccessfullyPublishedResult(voki.Id, voki.Cover, voki.Name);
    }
}