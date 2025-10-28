import { ApiVokisCatalog, RJO } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { PublishedVokiBriefInfo } from "$lib/ts/voki";


type VokiViewState = {
    state:
    | { name: 'ok'; data: PublishedVokiBriefInfo }
    | { name: 'loading' }
    | { name: 'err'; errs: Err[] };
};
type CacheEntry = {
    info: PublishedVokiBriefInfo;
    lastFetched: Date;
};
const CACHE_SIZE = 100;
const TTL_MS = 5 * 60 * 1000;

export namespace MyPublishedVokisCacheStore {
    const cache: Record<string, CacheEntry> = $state({});
    const ongoingRequests: Record<string, Promise<PublishedVokiBriefInfo | null>> = {};

    export function Get(id: string): VokiViewState {
        const cached = cache[id];
        const now = new Date();
        return {
            state: { name: 'loading' }
        }
    }

}