import { ApiAlbums } from '$lib/ts/backend-communication/backend-services';
import type { ResponseResult } from '../backend-communication/result-types';

const TTL_MS = 10 * 60 * 1000; // 10 Mins

export type VokisAlbumData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondColor: string;
    vokiIds: string[];
};
export class AlbumsStore {
    #cachedAlbums = $state<VokisAlbumData[] | null>(null);
    #lastFetched = 0;

    async #fetchAlbums(): Promise<ResponseResult<VokisAlbumData[]>> {
        return ApiAlbums.fetchJsonResponse<VokisAlbumData[]>(
            `/user-albums`,
            { method: 'GET' }
        );
    }
    async refresh(): Promise<VokisAlbumData[] | null> {
        const response = await this.#fetchAlbums();
        if (response.isSuccess) {
            this.#cachedAlbums = response.data;
            this.#lastFetched = Date.now();
            return this.#cachedAlbums;
        } else {
            return null;
        }
    }
    async getAlbums(): Promise<VokisAlbumData[] | null> {
        const now = Date.now();
        const isStale =
            !this.#cachedAlbums
            || now - this.#lastFetched > TTL_MS;

        if (isStale) {
            return this.refresh();
        }

        return this.#cachedAlbums;
    }

    invalidate() {
        this.#cachedAlbums = null;
        this.#lastFetched = 0;
    }
}

export const albumsStore = new AlbumsStore();
