using SharedKernel.common;

namespace DraftVokisLib;

public record class VokiDetails(
    VokiDescription Description,
    bool IsAgeRestricted,
    Language Language
)
{
    public static VokiDetails Default => new(VokiDescription.Empty, false, Language.Other);
}