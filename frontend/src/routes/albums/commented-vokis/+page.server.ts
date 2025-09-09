import { ApiVokiComments } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "../../$types";


export const load: PageServerLoad = async ({ fetch }) => {

    return {
        response: await ApiVokiComments.serverFetchJsonResponse<{ vokiIds: string[] }>(
            fetch, `/commented-voki-ids`, { method: 'GET' }
        ),
    };
};