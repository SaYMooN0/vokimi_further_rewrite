import { ApiVokiCreationCore } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { VokiType } from "$lib/ts/voki-type";

type InvitesState =
    | { state: 'loading' }
    | { state: 'errs'; errs: Err[] }
    | { state: 'loaded'; invites: InviteForVokiCoAuthorData[] };

export type InviteForVokiCoAuthorData = {
    vokiId: string,
    vokiName: string,
    vokiCover: string,
    vokiType: VokiType,
    primaryAuthorId: string,
    coAuthorIds: string[],
    invitedForCoAuthorUserIds: string[],
    creationDate: Date
}

export class MyVokiInvitesPageState {
    loadingState: InvitesState = $state({ state: 'loading' });

    constructor() {
        this.loadInvites();
    }

    async forceRefetch() {
        await this.loadInvites();
    }

    async loadInvites() {
        this.loadingState = { state: 'loading' };

        const response = await ApiVokiCreationCore.fetchJsonResponse<{ invites: InviteForVokiCoAuthorData[] }>(
            '/list-invites',
            { method: 'GET' }
        );

        if (response.isSuccess) {
            this.loadingState = { state: 'loaded', invites: response.data.invites };
        } else {
            this.loadingState = { state: 'errs', errs: response.errs };
        }
    }

    
    updateByInviteIds(inviteIds: string[]) {
        if (this.loadingState.state !== 'loaded') {
            this.loadInvites();
            return;
        }

        const current = this.loadingState.invites;

        const incomingSet = new Set(inviteIds);
        const currentSet = new Set(current.map(i => i.vokiId));

        let hasNew = false;
        for (const id of inviteIds) {
            if (!currentSet.has(id)) {
                hasNew = true;
                break;
            }
        }

        if (hasNew) {
            this.loadInvites();
            return;
        }

        const filtered = current.filter(inv => incomingSet.has(inv.vokiId));
        if (filtered.length === current.length) {
            return;
        }

        this.loadingState = {
            state: 'loaded',
            invites: filtered
        };
    }

}