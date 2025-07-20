using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel;
using SharedKernel.auth;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record InitializeNewVokiCommand(VokiType VokiType, VokiName VokiName)
    : ICommand<(VokiId Id, VokiType Type)>;

internal sealed class InitializeNewVokiCommandHandler :
    ICommandHandler<InitializeNewVokiCommand, (VokiId Id, VokiType Type)>
{
    private readonly IUserContext _userContext;
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public InitializeNewVokiCommandHandler(
        IUserContext userContext,
        IDraftVokiRepository draftVokiRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _userContext = userContext;
        _draftVokiRepository = draftVokiRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<(VokiId Id, VokiType Type)>> Handle(InitializeNewVokiCommand command, CancellationToken ct) {
        AppUserId authorId = _userContext.AuthenticatedUserId;
        DraftVoki voki = DraftVoki.Create(command.VokiName, command.VokiType, authorId, _dateTimeProvider.UtcNow);
        await _draftVokiRepository.Add(voki);
        return (voki.Id, voki.Type);
    }
}