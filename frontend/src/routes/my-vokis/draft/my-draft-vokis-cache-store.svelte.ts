import { ApiVokiCreationCore, RJO } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { VokiType } from "$lib/ts/voki";


type DraftVokiBriefInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
}
type VokiViewState = {
    state:
    | { name: 'ok'; data: DraftVokiBriefInfo }
    | { name: 'loading' }
    | { name: 'err'; errs: Err[] };
};
type CacheEntry = {
    lastFetched: Date;
    data: VokiViewState;
};

const CACHE_SIZE = 100;
const TTL_MS = 5 * 60 * 1000;

export namespace MyDraftVokisCacheStore {
    const cache: Record<string, CacheEntry> = $state({});
    const ongoingRequests: Record<string, VokiViewState> = {};


    export function Get(id: string): VokiViewState {
        const cached = cache[id];
        const now = new Date();
        if (cached && now.getTime() - cached.lastFetched.getTime() <= TTL_MS) {
            return cached.data;
        }
        if (ongoingRequests[id] != null) {
            return ongoingRequests[id];
        }

        return {
            state: { name: 'loading' }
        }
    }

}