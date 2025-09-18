import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingData } from "../../types";

export const load: ServerLoad = async ({  params, fetch }) => {
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<GeneralVokiTakingData>(
            fetch, `/vokis/${params.vokiId}/results/${params.vokiId}`, {
            method: "GET",
        }
        ),
        vokiId: params.vokiId,
        resultId: params.vokiId
    }
}
