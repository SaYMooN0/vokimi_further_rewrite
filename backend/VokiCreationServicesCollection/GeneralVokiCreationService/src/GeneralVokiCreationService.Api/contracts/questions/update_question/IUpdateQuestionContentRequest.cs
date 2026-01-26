using System.Text.Json.Serialization;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

[JsonDerivedType(typeof(UpdateTextOnlyQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiAnswerType.TextOnly))]
public interface IUpdateQuestionContentRequest : IRequestWithValidationNeeded
{
    public UnsavedQuestionContentDto ValidatedContent { get; protected set; }
}

public class UpdateTextOnlyQuestionContentRequest : IUpdateQuestionContentRequest { }