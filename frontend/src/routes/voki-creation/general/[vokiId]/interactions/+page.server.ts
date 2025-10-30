import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiInteractionSettings } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    return {
        vokiId: params.vokiId,
        response: await ApiVokiCreationGeneral.serverFetchJsonResponse<GeneralVokiInteractionSettings>(
            fetch, `/vokis/${params.vokiId}/interaction-settings`, { method: 'GET' }
        )
    };
}
