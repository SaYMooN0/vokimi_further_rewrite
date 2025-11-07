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
        this.loadDraftVokis();
    }

    async forceRefetch() {
        await this.loadDraftVokis();
    }

    async loadDraftVokis() {
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
}