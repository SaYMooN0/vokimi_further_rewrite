import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "../../$types";


export const load: PageServerLoad = async ({ fetch }) => {

    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<{ vokiIds: string[] }>(
            fetch, `/rated-voki-ids`, { method: 'GET' }
        ),
    };
};