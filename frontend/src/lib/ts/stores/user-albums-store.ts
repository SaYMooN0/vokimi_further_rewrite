//auto inv in 10 min
// invalidate
// get null or list
// refresh method

const TTL_MS = 10 * 60 * 1000;

type VokisAlbumData = {
    id: string;
    name: string;
    description: string;
    icon: string;
    mainColor: string;
    secondColor: string;
    vokiIds: string[];

};

type CacheEntry = {
    info: VokisAlbumData;
    lastFetched: Date;
};
