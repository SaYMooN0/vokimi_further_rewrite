import { ApiAlbums, RJO } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import { SvelteSet } from "svelte/reactivity";

export type AlbumViewData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondaryColor: string;
    creationDate: Date;
};

type AlbumsLoadingState =
    | { name: "loading" }
    | { name: "ok"; albums: AlbumViewData[] }
    | { name: "errs"; errs: Err[] };

export class AddVokiToAlbumsDialogState {
    readonly vokiId: string;
    albumsState: AlbumsLoadingState = $state({ name: "loading" });
    albumToIsChosen: Record<string, boolean> = $state({});

    private static readonly CACHE_MS = 5 * 60 * 1000;
    private _lastLoadedAt = 0;
    private _inflight: Promise<void> | null = null;

    private _initialChosenAlbumIds = new SvelteSet<string>();

    constructor(vokiId: string) {
        this.vokiId = vokiId;
    }
    isAlbumChosenChanged(albumId: string): boolean {
        const initial = this._initialChosenAlbumIds.has(albumId);
        const now = this.albumToIsChosen[albumId];
        return now !== initial;
    }
    async ensureFresh(): Promise<void> {
        if (this._inflight) {
            return this._inflight;
        }

        const fresh =
            Date.now() - this._lastLoadedAt <= AddVokiToAlbumsDialogState.CACHE_MS &&
            this.albumsState.name === "ok";

        if (fresh) {
            return;
        }

        this._inflight = this.updateForce().finally(() => (this._inflight = null));
        return this._inflight;
    }

    async updateForce(): Promise<void> {
        if (this._inflight) {
            return this._inflight;
        }


        const task = (async () => {
            this.albumsState = { name: "loading" };

            const response = await ApiAlbums.fetchJsonResponse<{ albums: AlbumDataWithVokiPresence[] }>(
                `/vokis/${this.vokiId}/albums-data`,
                { method: "GET" }
            );

            this.handleFetchAlbumsWithVokiPresence(response);
        })();

        this._inflight = task.finally(() => (this._inflight = null));
        return this._inflight;
    }

    async updateVokiPresenceInAlbums() {
        this.albumsState = { name: "loading" };
        const response = await ApiAlbums.fetchJsonResponse<{ albums: AlbumDataWithVokiPresence[] }>(
            `/vokis/${this.vokiId}/update-presence-in-albums`,
            RJO.PATCH({ albumIdToIsChosen: this.albumToIsChosen })
        );
        this.handleFetchAlbumsWithVokiPresence(response);

    }

    handleFetchAlbumsWithVokiPresence(response: ResponseResult<{ albums: AlbumDataWithVokiPresence[]; }>) {
        this.albumToIsChosen = {};
        this._initialChosenAlbumIds.clear();
        if (response.isSuccess) {
            const albums: AlbumViewData[] = response.data.albums
                .map((a) => ({
                    id: a.id,
                    name: a.name,
                    icon: a.icon,
                    mainColor: a.mainColor,
                    secondaryColor: a.secondaryColor,
                    creationDate: a.creationDate instanceof Date
                        ? a.creationDate
                        : new Date(a.creationDate as unknown as string),
                }))
                .sort((a, b) => b.creationDate.getTime() - a.creationDate.getTime());

            for (const a of response.data.albums) {
                this.albumToIsChosen[a.id] = a.isVokiInAlbum;
                if (a.isVokiInAlbum) {
                    this._initialChosenAlbumIds.add(a.id);
                }
            }

            this.albumsState = { name: "ok", albums };
            this._lastLoadedAt = Date.now();
        } else {
            this.albumsState = { name: "errs", errs: response.errs };
            this._lastLoadedAt = 0;
        }
    }

}
type AlbumDataWithVokiPresence = AlbumViewData & { isVokiInAlbum: boolean };
