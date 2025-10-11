import { ApiVokiComments } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "../../$types";
import type { VokiIdToDateDict } from "../../types";

export const load: PageServerLoad<{
    albumName: string;
    howToAddVokis: string;
    response: ResponseResult<{ vokiIdWithCommentDate: VokiIdToDateDict }>;
}> = async ({ fetch }) => {
    return {
        albumName: "commented",
        howToAddVokis: "Vokis will be added automatically to this album after you comment them for the first time",
        response: await ApiVokiComments.serverFetchJsonResponse<{ vokiIdWithCommentDate: VokiIdToDateDict }>(
            fetch, `/commented-vokis`, { method: "GET" }
        )
    };
};
