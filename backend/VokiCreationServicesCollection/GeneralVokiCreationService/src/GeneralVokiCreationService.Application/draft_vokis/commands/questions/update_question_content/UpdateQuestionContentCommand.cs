using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;

public sealed record UpdateQuestionContentCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    BaseUnsavedQuestionContentDto NewContent
) :
    ICommand<BaseQuestionTypeSpecificContent>,
    IWithAuthCheckStep;

internal sealed class UpdateQuestionContentCommandHandler
    : ICommandHandler<UpdateQuestionContentCommand, BaseQuestionTypeSpecificContent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public UpdateQuestionContentCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<BaseQuestionTypeSpecificContent>> Handle(UpdateQuestionContentCommand command, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOr<BaseQuestionTypeSpecificContent> savedContent = await SaveContent(
            command.VokiId, command.QuestionId, command.NewContent
        );
        if (savedContent.IsErr(out var err)) {
            return err;
        }

        ErrOr<BaseQuestionTypeSpecificContent> res = voki.UpdateQuestionTypeSpecificContent(
            command.UserCtx(_userCtxProvider),
            command.QuestionId,
            savedContent.AsSuccess()
        );
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> SaveContent(
        VokiId vokiId, GeneralVokiQuestionId questionId, BaseUnsavedQuestionContentDto unsavedContent, CancellationToken ct
    ) => unsavedContent.Match<Task<ErrOr<BaseQuestionTypeSpecificContent>>>(
        textOnly: (dto) => Task.FromResult(dto.ToSavedContent()
            .Bind<BaseQuestionTypeSpecificContent>(s => s)),
        imageOnly: (dto) => CreateImageOnlyContent(vokiId, questionId, dto, ct),
        imageAndText: () => CreateImageAndTextAnswerData(vokiId, questionId, d, ct),
        colorOnly: (dto) => Task.FromResult(dto.ToSavedContent()
            .Bind<BaseQuestionTypeSpecificContent>(s => s)),
        (dto) => Task.FromResult(dto.ToSavedContent()
            .Bind<BaseQuestionTypeSpecificContent>(s => s)),
        audioOnly: () => CreateAudioOnlyAnswerData(vokiId, questionId, d, ct),
        audioAndText: () => CreateAudioAndTextAnswerData(vokiId, questionId, d, ct)
    );

    private async Task<ErrOr<BaseQuestionTypeSpecificContent>> CreateImageOnlyContent(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        ImageOnlyUnsavedQuestionContentDto answer,
        CancellationToken ct
    ) {
        if(answer.Answers.)
         await HandleAnswerImageKey(vokiId, questionId, answer., ct)
        
    }

    


     private async Task<ErrOr<GeneralVokiAnswerImageKey>> HandleAnswerImageKey(
         VokiId vokiId,
         GeneralVokiQuestionId questionId,
         string? image,
         CancellationToken ct
     ) {
         if (image is null) {
             return ErrFactory.NoValue.Common("Image value is not provided");
         }

         if (GeneralVokiAnswerImageKey.FromString(image).IsSuccess(out var savedKey)) {
             return savedKey;
         }

         ErrOr<TempImageKey> tempKeyCreationRes = TempImageKey.FromString(image);
         if (tempKeyCreationRes.IsErr(out var err)) {
             return err;
         }

         TempImageKey tempKey = tempKeyCreationRes.AsSuccess();
         ImageFileExtension ext = tempKey.Extension;
         var destination = GeneralVokiAnswerImageKey.CreateForAnswer(vokiId, questionId, ext);
         ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiAnswerImageFromTempToStandard(tempKey, destination, ct);
         if (copyingRes.IsErr(out err)) {
             return ErrFactory.Unspecified("Couldn't save answer image", details: err.Message);
         }

         return destination;
     }
}