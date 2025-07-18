using System.Text.Json;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Infrastructure.parsers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class VokiAnswerTypeDataConverter : ValueConverter<BaseVokiAnswerTypeData, string>
{
    public VokiAnswerTypeDataConverter() : base(
        data => ToString(data),
        str => FromString(str)
    ) { }

    private const string Divider = ": ";

    private static string ToString(BaseVokiAnswerTypeData value) =>
        value.MatchingEnum + Divider + JsonSerializer.Serialize(VokiAnswerTypeDataParser.ToDictionary(value));

    private static BaseVokiAnswerTypeData FromString(string str) {
        string[] parts = str.Split(Divider, 2);
        GeneralVokiAnswerType type = Enum.Parse<GeneralVokiAnswerType>(parts[0]);
        Dictionary<string, string> data = JsonSerializer.Deserialize<Dictionary<string, string>>(parts[1])!;
        return VokiAnswerTypeDataParser.CreateFromDictionary(type, data).AsSuccess();
    }
}