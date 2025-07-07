using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Domain.repositories;

public interface IDraftGeneralVokiRepository
{
    Task Add(DraftGeneralVoki generalVoki);

}