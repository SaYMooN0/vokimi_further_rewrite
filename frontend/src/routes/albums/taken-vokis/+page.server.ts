import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "../../$types";
import type { VokiOverviewInfo } from "../../catalog/[vokiId]/ts_page/types";
export const load: PageServerLoad = async ({ fetch }) => {

    return {
        response: await ApiVokisCatalog.serverFetchJsonResponse<{ vokis: VokiOverviewInfo[] }>(
            fetch, `/taken-vokis-overview`, { method: 'GET' }
        ),
    };
};