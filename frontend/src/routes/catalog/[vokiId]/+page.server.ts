import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokisCatalog.serverFetchJsonResponse<{
        vokis: any[]
    }>(
        fetch, `/vokis/${params.vokiId}`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
