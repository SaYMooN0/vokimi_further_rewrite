import type { VokiItemViewState } from "$lib/components/voki_item/VokiItemView.svelte";

export type VokiAlbumPreviewData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondaryColor: string;
    vokisCount: number;
};
export type VokiIdToDateDict = Record<string, Date>;

export type AutoAlbumsAppearance = {
    takenVokisAlbums: AutoAlbumsColorsPair,
    ratedVokisAlbums: AutoAlbumsColorsPair,
    commentedVokisAlbums: AutoAlbumsColorsPair
}
export type AutoAlbumsColorsPair = { mainColor: string, secondaryColor: string };
