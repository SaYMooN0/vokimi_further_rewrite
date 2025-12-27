using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.commands;

public sealed record RateVokiCommand(
    VokiId VokiId,
    ushort NewRating
) : ICommand<VokiRating>,
    IWithAuthCheckStep;

internal sealed class RateVokiCommandHandler : ICommandHandler<RateVokiCommand, VokiRating>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;

    public RateVokiCommandHandler(
        IAppUsersRepository appUsersRepository,
        IRatingsRepository ratingsRepository,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext
    ) {
        _appUsersRepository = appUsersRepository;
        _ratingsRepository = ratingsRepository;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public async Task<ErrOr<VokiRating>> Handle(RateVokiCommand command, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        AppUser user = (await _appUsersRepository.GetById(userId, ct))!;
        if (!user.HasTaken(command.VokiId)) {
            return ErrFactory.NoAccess(
                "You cannot rate this Voki because you have not taken it yet",
                "To rate voki you need to take it"
            );
        }

        ErrOr<RatingValueWithDate> creationRes = RatingValueWithDate.Create(command.NewRating, _dateTimeProvider.UtcNow);

        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var ratingValue = creationRes.AsSuccess();
        VokiRating? rating = await _ratingsRepository.GetUserRatingForVokiWithHistory(
            new AuthenticatedUserContext(userId), command.VokiId, ct
        );


        if (rating is null) {
            rating = VokiRating.CreateNew(userId, command.VokiId, ratingValue);
            await _ratingsRepository.Add(rating, ct);
        }
        else {
            ErrOrNothing res = rating.Update(ratingValue);
            if (res.IsErr(out var updateRatingErr)) {
                return updateRatingErr;
            }

            await _ratingsRepository.Update(rating, ct);
        }

        return rating;
    }
}