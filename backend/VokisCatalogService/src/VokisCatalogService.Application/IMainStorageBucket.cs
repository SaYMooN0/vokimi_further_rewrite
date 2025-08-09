namespace VokisCatalogService.Application;

public interface IMainStorageBucket
{
    Task<ErrOrNothing> DeleteDraftVokiContentAfterPublication(VokiId vokiId);
}