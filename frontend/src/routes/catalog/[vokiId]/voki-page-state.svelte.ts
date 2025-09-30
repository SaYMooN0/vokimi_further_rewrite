import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import { RequestJsonOptions } from "$lib/ts/request-json-options";
import type { VokiPageTab } from "./+page.server";
import type { RatingsTabDataType, VokiRatingsWithAverage } from "./types";

export class VokiPageState {

    readonly vokiId: string;
    currentTab: VokiPageTab = $state('about');
    ratingsCount: number = $state(0);
    commentsCount: number = $state(0);
    ratingsTabData: RatingsTabDataType = $state({ state: 'empty' });
    constructor(vokiId: string, pageTab: VokiPageTab, ratingsCount: number, commentsCount: number) {
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
                userHasTaken: response.data.userHasTaken
            };
        }
        else {
            this.ratingsTabData = { state: 'error', errs: response.errs }
        }
    }
    public saveNewUserRating(newRatingVal: number): Promise<ResponseResult<{ value: number, dateTime: Date }>> {
        return ApiVokiRatings.fetchJsonResponse<{ value: number, dateTime: Date }>(
            `/vokis/${this.vokiId}/rate`,
            RequestJsonOptions.PATCH({ ratingValue: newRatingVal })
        );

    }
}
