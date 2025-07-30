using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.results;

public class VokiResultImageConverter : ValueConverter<DraftGeneralVokiResultImageKey?, string?>
{
    public VokiResultImageConverter() : base(
        img => ToString(img),
        str =>FromString(str)
    ) { }
    private static string? ToString(DraftGeneralVokiResultImageKey? image) =>
        image?.ToString();
    private static DraftGeneralVokiResultImageKey? FromString(string? str) => 
        str is null ? null : new DraftGeneralVokiResultImageKey(str);
}