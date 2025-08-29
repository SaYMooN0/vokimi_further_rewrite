using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel;
using SharedKernel.auth;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record InitializeNewVokiCommand(VokiType VokiType, VokiName VokiName)
    : ICommand<DraftVoki>;

internal sealed class InitializeNewVokiCommandHandler :
    ICommandHandler<InitializeNewVokiCommand, DraftVoki>
{
    private readonly IUserContext _userContext;
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMainStorageBucket _mainStorageBucket;

    public InitializeNewVokiCommandHandler(
        IUserContext userContext,
        IDraftVokiRepository draftVokiRepository,
        IDateTimeProvider dateTimeProvider,
        IMainStorageBucket mainStorageBucket
    ) {
        _userContext = userContext;
        _draftVokiRepository = draftVokiRepository;
        _dateTimeProvider = dateTimeProvider;
        _mainStorageBucket = mainStorageBucket;
    }

    private static readonly VokiType[] ImplementedTypes = [VokiType.General];

    public async Task<ErrOr<DraftVoki>> Handle(InitializeNewVokiCommand command, CancellationToken ct) {
        if (!ImplementedTypes.Contains(command.VokiType)) {
            return ErrFactory.NotImplemented("Specified voki type is not implemented");
        }

        AppUserId authorId = _userContext.AuthenticatedUserId;
        DraftVoki voki = DraftVoki.Create(command.VokiName, command.VokiType, authorId, _dateTimeProvider.UtcNow);

        var copyRes = await _mainStorageBucket.CopyDefaultVokiCoverForNewVoki(voki.Cover);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Add(voki);
        return voki;
    }
}