import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "./$types";
import type { VokiOverviewInfo } from "./ts_page/types";


const TABS = ['about', 'comments', 'ratings'] as const;
export type VokiPageTab = typeof TABS[number];

export const load: PageServerLoad = async ({ url, params, fetch }) => {
    const raw = (url.searchParams.get('tab') ?? '').toLowerCase();
    const tab = (TABS as readonly string[]).includes(raw) ? raw : 'about' as VokiPageTab;

    return {
        response: await ApiVokisCatalog.serverFetchJsonResponse<VokiOverviewInfo>(
            fetch, `/vokis/${params.vokiId}/overview`, { method: 'GET' }
        ),
        vokiId: params.vokiId,
        currentTab: tab
    };
};