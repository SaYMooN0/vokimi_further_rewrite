using SharedKernel.auth;
using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace VokiTakingServicesLib.Domain.general_voki_aggregate;

public class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public VokiName Name { get; }
    private bool AuthenticatedOnlyTaking { get; }

    public ErrOrNothing CheckUserAccessToTake(IUserContext userContext) {
        if (AuthenticatedOnlyTaking && userContext.UserIdFromToken().IsErr()) {
            return ErrFactory.NoAccess("To take this Voki you need to be signed in");
        }

        return ErrOrNothing.Nothing;
    }

    protected BaseVoki(VokiName name, bool authenticatedOnlyTaking) {
        Name = name;
        AuthenticatedOnlyTaking = authenticatedOnlyTaking;
    }
}