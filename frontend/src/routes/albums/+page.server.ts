import { ApiAlbums } from "$lib/ts/backend-communication/backend-services";
import { type ServerLoad } from "@sveltejs/kit";
import type { AutoAlbumsColorsPair, VokiAlbumPreviewData } from "./types";

export const load: ServerLoad = async ({ fetch }) => {
    return ApiAlbums.serverFetchJsonResponse<{
        albums: VokiAlbumPreviewData[],
        takenVokisAlbums: AutoAlbumsColorsPair,
        ratedVokisAlbums: AutoAlbumsColorsPair,
        commentedVokisAlbums: AutoAlbumsColorsPair
    }>(
        fetch, '/all-albums-preview', { method: 'GET' }
    );
};



