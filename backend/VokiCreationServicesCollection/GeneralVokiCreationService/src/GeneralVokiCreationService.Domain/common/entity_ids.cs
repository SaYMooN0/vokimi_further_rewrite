namespace GeneralVokiCreationService.Domain.common;

public class VokiQuestionId(Guid value) : GuidBasedId(value)
{
    public static VokiQuestionId CreateNew() => new(Guid.CreateVersion7());
}
public class VokiQuestionAnswerId(Guid value) : GuidBasedId(value)
{
    public static VokiQuestionAnswerId CreateNew() => new(Guid.CreateVersion7());
}
