namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

public class ListVokisBriefInfoRequest : IRequestWithValidationNeeded
{
    public string[] Ids { get; init; }
    private const int MaxAllowedIds = 200;

    public ErrOrNothing Validate() {
        if (Ids == null || Ids.Length == 0) {
            return ErrFactory.NoValue.Common("No voki ids were provided");
        }

        if (Ids.Length > MaxAllowedIds) {
            return ErrFactory.ValueOutOfRange(
                $"Too many voki ids specified. Cannot handle more than {MaxAllowedIds} ids at a time",
                $"Provided ids count: {Ids.Length}"
            );
        }

        string[] incorrectIds = Ids.Where(id => !Guid.TryParse(id, out _)).ToArray();
        if (incorrectIds.Length > 0) {
            return ErrFactory.IncorrectFormat(
                $"Some ({incorrectIds.Length}) of provided ids were incorrect",
                $"Incorrect ids: {string.Join(", ", incorrectIds)}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public VokiId[] ParsedVokiIds => Ids.Select(i => new VokiId(new(i))).ToArray();
}