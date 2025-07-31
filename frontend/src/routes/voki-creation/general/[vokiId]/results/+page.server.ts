import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<{
        results: ResultOverViewData[]
    }>(
        fetch, `/vokis/${params.vokiId}/results/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}

type ResultOverViewData = {
    id: string;
    name: string;
    text: string;
    image: string | null;
};