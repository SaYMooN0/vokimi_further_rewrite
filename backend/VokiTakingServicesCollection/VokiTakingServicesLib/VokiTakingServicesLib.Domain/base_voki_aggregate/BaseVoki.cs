using SharedKernel.auth;
using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace VokiTakingServicesLib.Domain.base_voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public VokiName Name { get; }
    protected abstract IVokiInteractionSettings BaseInteractionSettings { get; }


    public ErrOrNothing CheckUserAccessToTake(IUserContext userContext) {
        if (BaseInteractionSettings.AuthenticatedOnlyTaking && userContext.UserIdFromToken().IsErr()) {
            return ErrFactory.NoAccess("To take this Voki you need to be signed in");
        }

        return ErrOrNothing.Nothing;
    }

    protected BaseVoki(VokiName name) {
        Name = name;
    }
}