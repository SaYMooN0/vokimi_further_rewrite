import type { Err } from "$lib/ts/err";
import type { VokiDailyRatingsSnapshot } from "../../types";

export class RatingsHistoryState {
    readonly vokiId: string;
    readonly publicationDate: Date;
    allSnapshotsState: AllSnapshotsState = $state({ type: 'loading' });

    constructor(vokiId: string, publicationDate: Date) {
        this.vokiId = vokiId;
        this.publicationDate = publicationDate;
        this.loadAllRatingsSnapshots();
    }
    async loadAllRatingsSnapshots() {
        return allSnapshotsState =...
    }
    snapshotsToShow: VokiDailyRatingsSnapshot[] = $derived.by(() => { return [] });;

}


type AllSnapshotsState =
    | { type: ' ok', data: VokiDailyRatingsSnapshot[] }
    | { type: 'loading' }
    | { type: 'errs', errs: Err[] }