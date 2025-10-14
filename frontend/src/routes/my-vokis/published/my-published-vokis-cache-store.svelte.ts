import { ApiVokisCatalog, RJO } from "$lib/ts/backend-communication/backend-services";
import type { PublishedVokiBriefInfo } from "$lib/ts/voki";





type CacheEntry = {
    info: PublishedVokiBriefInfo;
    lastFetched: Date;
};
const CACHE_SIZE = 100;
const TTL_MS = 5 * 60 * 1000;

export namespace MyPublishedVokisCacheStore {
    const cache: Record<string, CacheEntry> = $state({});
    const ongoingRequests: Record<string, Promise<PublishedVokiBriefInfo | null>> = {};

    export async function Get(id: string): Promise<PublishedVokiBriefInfo | null> {
        const cached = cache[id];
        const now = new Date();
        if (cached && now.getTime() - cached.lastFetched.getTime() <= TTL_MS) {
            return cached.info;
        }
        if (ongoingRequests[id] != null) {
            return await ongoingRequests[id];
        }

        const request = RefreshAndGet(id);
        ongoingRequests[id] = request;

        try {
            return await request;
        } finally {
            delete ongoingRequests[id];
        }
    }

    export async function RefreshAndGet(id: string): Promise<PublishedVokiBriefInfo | null> {
      
        const response = await ApiVokisCatalog.fetchJsonResponse<{ vokis: PublishedVokiBriefInfo[] }>(
            `/vokis/brief-info`,
            RJO.POST({ ids: [id] })
        );
        if (response.isSuccess && response.data && response.data.vokis.length > 0) {
            insertOrReplace(response.data.vokis[0]);
            return response.data.vokis[0];
        }

        return null;
    }

    export async function EnsureExist(ids: string[]): Promise<void> {
        if (ids.length === 0) { return; }
        const now = new Date();

        const needsFetch = ids.filter(id =>
            (!cache[id] || now.getTime() - cache[id].lastFetched.getTime() > TTL_MS)
        );

        if (needsFetch.length === 0) { return; }

        const limitedIds = needsFetch.slice(0, CACHE_SIZE);

        const response = await ApiVokisCatalog.fetchJsonResponse<{ vokis: PublishedVokiBriefInfo[] }>(
            `/vokis/brief-info`,
            RJO.POST({ ids: limitedIds })
        );

        if (response.isSuccess && response.data) {
            for (const info of response.data.vokis) {
                insertOrReplace(info);
            }
        }
    }


    export function Clear(): void {
        for (const key in cache) {
            delete cache[key];
        }
    }

    function insertOrReplace(info: PublishedVokiBriefInfo): void {
        const now = new Date();
        if (Object.keys(cache).length >= CACHE_SIZE) {
            evictOldest();
        }

        cache[info.id] = {
            info,
            lastFetched: now
        };
    }

    function evictOldest(): void {
        const entries = Object.entries(cache);
        if (entries.length === 0) return;

        const [oldestKey] = entries.reduce(
            (min, curr) => curr[1].lastFetched < min[1].lastFetched ? curr : min,
            entries[0]
        );

        delete cache[oldestKey];
    }
}