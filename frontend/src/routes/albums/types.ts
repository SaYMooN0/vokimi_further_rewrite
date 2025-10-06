export type AutoAlbumsColorsPair = { mainColor: string, secondaryColor: string };

export type VokiAlbumPreviewData = {
    id: string;
    name: string;
    icon: string;
    mainColor: string;
    secondaryColor: string;
    vokisCount: number;
};
export type VokiIdToDate = Record<string, Date>; 