using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using Microsoft.EntityFrameworkCore;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftGeneralVokisRepository : IDraftGeneralVokisRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftGeneralVokisRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }


    public async Task Add(DraftGeneralVoki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }


    public Task<DraftGeneralVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);


    public Task<DraftGeneralVoki?> GetWithQuestionsAsNoTracking(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .AsNoTracking()
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .AsNoTracking()
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithResultsAsNoTracking(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .AsNoTracking()
        .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId, CancellationToken ct) => _db.Vokis
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
        .AsSplitQuery()
        .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
            .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
            .AsSplitQuery()
            .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task Delete(DraftGeneralVoki voki, CancellationToken ct) {
        _db.Vokis.Remove(voki);
        return _db.SaveChangesAsync(ct);
    }


    public Task<DraftGeneralVoki?> GetById(VokiId generalVokiId, CancellationToken ct) => _db.Vokis
        .FirstOrDefaultAsync(v => v.Id == generalVokiId, cancellationToken: ct);

    async Task<BaseDraftVoki?> IDraftVokiRepository.GetById(VokiId vokiId, CancellationToken ct) =>
        await GetById(generalVokiId: vokiId, ct);

    public async Task Update(DraftGeneralVoki generalVoki, CancellationToken ct) {
        _db.Vokis.Update(generalVoki);
        await _db.SaveChangesAsync(ct);
    }

    async Task IDraftVokiRepository.Update(BaseDraftVoki voki, CancellationToken ct) {
        if (voki is not DraftGeneralVoki generalVoki) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Unexpected type of voki: {voki.GetType()}. Expected: {typeof(DraftGeneralVoki)}"
            ));
            throw new();
        }

        await Update(generalVoki: generalVoki, ct);
    }
}