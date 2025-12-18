import { ApiVokiRatings, RJO } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import { toast } from "svelte-sonner";
import type { VokiPageTab } from "./+page.server";
import type { RatingsTabDataType, VokiRatingData, VokiRatingsWithAverage } from "./types";

export class VokiPageState {
    readonly vokiId: string;
    currentTab: VokiPageTab = $state('about');
    ratingsCount: number = $state(0);
    commentsCount: number = $state(0);
    ratingsTabData: RatingsTabDataType = $state({ state: 'empty' });
    constructor(
        vokiId: string,
        pageTab: VokiPageTab,
        ratingsCount: number,
        commentsCount: number
    ) {
        this.vokiId = vokiId;
        this.currentTab = pageTab;
        this.ratingsCount = ratingsCount;
        this.commentsCount = commentsCount;
    }

    public async fetchRatingsTabData(): Promise<void> {
        this.ratingsTabData = { state: 'loading' };
        const response = await ApiVokiRatings.fetchJsonResponse<{
            userHasTaken: boolean,
            ratingsWithAverage: VokiRatingsWithAverage
        }>(
            `/vokis/${this.vokiId}/ratings`,
            { method: 'GET' }
        );
        if (response.isSuccess) {
            this.ratingsTabData = {
                state: 'fetched',
                averageRating: response.data.ratingsWithAverage.averageRating,
                allRatings: response.data.ratingsWithAverage.ratings,
                userHasTaken: response.data.userHasTaken,
                isAverageOutdated: false
            };
        }
        else {
            this.ratingsTabData = { state: 'error', errs: response.errs }
        }
    }
    public saveNewUserRating(newRatingVal: number): Promise<ResponseResult<VokiRatingData>> {
        if (this.ratingsTabData.state === 'fetched') {
            this.ratingsTabData.isAverageOutdated = true;
        }
        return ApiVokiRatings.fetchJsonResponse<VokiRatingData>(
            `/vokis/${this.vokiId}/rate`,
            RJO.PATCH({ ratingValue: newRatingVal })
        );
    }
    public async reloadOutdatedRatings(): Promise<void> {
        if (this.ratingsTabData.state !== 'fetched') {
            toast.error('Cannot reload ratings. Please refresh the page');
            return;
        }
        const response = await ApiVokiRatings.fetchJsonResponse<VokiRatingsWithAverage>(
            `/vokis/${this.vokiId}/all-with-average`, { method: 'GET' }
        );
        if (response.isSuccess) {
            this.ratingsTabData.averageRating = response.data.averageRating;
            this.ratingsTabData.allRatings = response.data.ratings;
            this.ratingsTabData.isAverageOutdated = false;
        }
        else {
            toast.error('Something went wrong. Could not reload ratings');
        }
    }
}
