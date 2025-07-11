import { ApiVokiCreationGeneral } from "$lib/ts/backend-services";
import type { ServerLoad } from "@sveltejs/kit";
import type { VokiMainInfo } from "../../../voki-creation-page-types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<VokiMainInfo>(
        fetch, `/vokis/${params.vokiId}/main-info`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
