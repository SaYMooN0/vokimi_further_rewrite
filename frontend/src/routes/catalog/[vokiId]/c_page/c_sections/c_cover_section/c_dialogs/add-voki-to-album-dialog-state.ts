import { ApiAlbums } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";

export type AlbumViewData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondaryColor: string;

};

type AlbumsLoadingState =
    | { name: "loading" }
    | {
        name: "ok";
        albums: AlbumViewData[];
    }
    | { name: "errs"; errs: Err[] };

export class AddVokiToAlbumsDialogState {
    readonly vokiId: string;
    albumsState: AlbumsLoadingState = $state({ name: "loading" });

    private static readonly CACHE_MS = 5 * 60 * 1000;
    private _lastLoadedAt = 0;
    private _inflight: Promise<void> | null = null;

    private _albumToIsChosen: Record<string, boolean> = {};
    private _albumToInitialChosen: Record<string, boolean> = {};

    constructor(vokiId: string) {
        this.vokiId = vokiId;
    }


    async ensureFresh(): Promise<void> {
        if (this._inflight) return this._inflight;

        const fresh =
            Date.now() - this._lastLoadedAt <= AddVokiToAlbumsDialogState.CACHE_MS &&
            this.albumsState.name === "ok";

        if (fresh) return;

        this._inflight = this.updateForce().finally(() => (this._inflight = null));
        return this._inflight;
    }

    async updateForce(): Promise<void> {
        if (this._inflight) {
            return this._inflight;
        }

        this._albumToIsChosen = {};
        this._albumToInitialChosen = {};
        this.albumsState = { name: "loading" };

        const task = (async () => {
            type AlbumFetchType = AlbumViewData & { isVokiInAlbum: boolean; };

            const response = await ApiAlbums.fetchJsonResponse<{ albums: AlbumFetchType[] }>(
                `/vokis/${this.vokiId}/albums-data`, { method: "GET" }
            );

            if (response.isSuccess) {
                const albums = response.data.albums;
                for (const a of albums) {
                    this._albumToIsChosen[a.id] = a.isVokiInAlbum;
                    this._albumToInitialChosen[a.id] = a.isVokiInAlbum;
                }

                this.albumsState = { name: "ok", albums };
                this._lastLoadedAt = Date.now();
            } else {
                this.albumsState = { name: "errs", errs: response.errs };
                this._lastLoadedAt = 0;
            }
        })();

        this._inflight = task.finally(() => (this._inflight = null));
        return this._inflight;
    }


    isAlbumChosen(albumId: string): boolean {
        return this._albumToIsChosen[albumId] ?? false;
    }

    isAlbumChosenChanged(albumId: string): boolean {
        const initial = this._albumToInitialChosen[albumId] ?? false;
        return this._albumToIsChosen[albumId] !== initial;
    }

    toggleAlbumChosen(albumId: string): void {
        const current = this._albumToIsChosen[albumId] ?? false;
        this._albumToIsChosen[albumId] = !current;
    }
}
