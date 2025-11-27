using System.Collections.Immutable;
using System.Reflection;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedKernel.common;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.domain.ids;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace DbSeeder.seeding.newtonsoft;

public class DraftGeneralVokiConverter : JsonConverter<DraftGeneralVoki>
{
    public override DraftGeneralVoki ReadJson(
        JsonReader reader, Type type, DraftGeneralVoki? existing,
        bool hasExisting, JsonSerializer serializer
    ) {
        JObject jo = JObject.Load(reader);
        DraftGeneralVoki obj = (DraftGeneralVoki)JsonUtil.CreateUninitialized(typeof(DraftGeneralVoki));

        if (jo.TryGetValue("takingProcessSettings", out var jTake)) {
            var tps = jTake.ToObject<VokiTakingProcessSettings>(serializer);
            JsonUtil.SetProperty(obj, "TakingProcessSettings", tps);
        }

        JObject jIntSet = (JObject)jo["interactionSettings"]!;
        bool signedInOnly = jIntSet["signedInOnlyTaking"]!.Value<bool>();
        var visibility = (GeneralVokiResultsVisibility)jIntSet["resultsVisibility"]!.Value<int>();
        bool showDistribution = jIntSet["showResultsDistribution"]!.Value<bool>();

        JsonUtil.SetProperty(obj, "InteractionSettings", GeneralVokiInteractionSettings
            .Create(signedInOnly, visibility, showDistribution).AsSuccess());

        FieldInfo questionsField = obj.GetType().GetField("_questions", BindingFlags.Instance | BindingFlags.NonPublic)!;
        List<VokiQuestion> questionsList = [];

        foreach (var jQ in (jo["questions"] as JArray)!) {
            var q = DeserializeQuestion((jQ as JObject)!, serializer);
            questionsList.Add(q);
        }

        questionsField.SetValue(obj, questionsList);

        FieldInfo? resultsField = obj.GetType().GetField("_results", BindingFlags.Instance | BindingFlags.NonPublic)!;

        List<VokiResult> resultsList = [];
        foreach (var jR in (jo["results"] as JArray)!) {
            var r = DeserializeResult((jR as JObject)!);
            resultsList.Add(r);
        }

        resultsField.SetValue(obj, resultsList);

        return obj;
    }

    private VokiQuestion DeserializeQuestion(JObject jo, JsonSerializer serializer) {
        var q = (VokiQuestion)JsonUtil.CreateUninitialized(typeof(VokiQuestion));

        JsonUtil.SetProperty(q, "Text", VokiQuestionText.Create(jo["text"]!.Value<string>()!).AsSuccess());
        JsonUtil.SetProperty(q, "OrderInVoki", jo["orderInVoki"]!.Value<ushort>());
        JsonUtil.SetProperty(q, "ShuffleAnswers", jo["shuffleAnswers"]!.Value<bool>());

        JToken ar = jo["imageSet"]!["aspectRatio"]!;
        double width = ar["width"]!.Value<double>();
        double height = ar["height"]!.Value<double>();
        VokiQuestionImagesAspectRatio finalAspect = VokiQuestionImagesAspectRatio.Create(width, height).AsSuccess();
        JsonUtil.SetProperty(q, "ImageSet", VokiQuestionImagesSet.Create([], finalAspect).AsSuccess());

        JsonUtil.SetProperty(q, "AnswersType",
            Enum.Parse<GeneralVokiAnswerType>(jo["answersType"]!.Value<string>()!, true));

        List<VokiQuestionAnswer> answersList = new List<VokiQuestionAnswer>();
        ushort ansOrder = 1;

        foreach (var jA in (jo["answers"] as JArray)!) {
            answersList.Add(DeserializeAnswer((JObject)jA, ansOrder));
            ansOrder++;
        }

        q.GetType().GetField("_answers", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(q, answersList);

        return q;
    }

    private VokiQuestionAnswer DeserializeAnswer(JObject jo, ushort order) {
        var a = (VokiQuestionAnswer)JsonUtil.CreateUninitialized(typeof(VokiQuestionAnswer));

        JsonUtil.SetProperty(a, "OrderInQuestion", order);

        if (jo.TryGetValue("text", out var jText)) {
            JsonUtil.SetProperty(a, "TypeData",
                new BaseVokiAnswerTypeData.TextOnly(GeneralVokiAnswerText.Create(jText.Value<string>()!).AsSuccess()));
        }
        else if (jo.TryGetValue("color", out var jColor)) {
            JsonUtil.SetProperty(a, "TypeData",
                new BaseVokiAnswerTypeData.ColorOnly(new HexColor(jColor.Value<string>()!)));
        }
        else {
            throw new Exception($"Unknown text type: {jText}");
        }

        List<string> res = jo["relatedResults"]!.ToObject<List<string>>()!;
        var resIds = res.Select(s => new GeneralVokiResultId(Guid.Parse(s))).ToImmutableHashSet();
        JsonUtil.SetProperty(a, "RelatedResultIds", resIds);

        return a;
    }

    private VokiResult DeserializeResult(JObject jo) {
        var r = (VokiResult)JsonUtil.CreateUninitialized(typeof(VokiResult));
        JsonUtil.SetProperty(r, "Id", new GeneralVokiResultId(new(jo["id"]!.Value<string>()!)));
        JsonUtil.SetProperty(r, "Name", VokiResultName.Create(jo["name"]!.Value<string>()!).AsSuccess());
        JsonUtil.SetProperty(r, "Text", VokiResultText.Create(jo["text"]!.Value<string>()!).AsSuccess());

        return r;
    }

    public override void WriteJson(JsonWriter writer, DraftGeneralVoki? value, JsonSerializer serializer)
        => throw new NotImplementedException();
}