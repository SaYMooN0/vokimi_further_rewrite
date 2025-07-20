import type { ServerLoad } from "@sveltejs/kit";
import type { VokiAuthorsInfo } from "../../../c_shared/c_shared_pages/c_authors/types";
import { ApiVokiCreationCore } from "$lib/ts/backend-communication/backend-services";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationCore.serverFetchJsonResponse<VokiAuthorsInfo>(
        fetch, `/vokis/${params.vokiId}/authors-info`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}

