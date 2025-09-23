import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<any>(
            fetch, `/vokis/${params.vokiId}/results/received`, {
            method: "GET",
        }
        ),
        vokiId: params.vokiId
    }
}
