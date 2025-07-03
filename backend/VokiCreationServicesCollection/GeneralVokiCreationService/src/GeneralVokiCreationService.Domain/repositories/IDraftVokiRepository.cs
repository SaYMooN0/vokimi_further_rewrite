using GeneralVokiCreationService.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Domain.repositories;

public interface IDraftVokiRepository
{
    Task Add(DraftVoki voki);

}