import { ApiVokiCreationGeneral, type VokiMainInfo } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<VokiMainInfo>(
        fetch, `/vokis/${params.vokiId}/main-info`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
