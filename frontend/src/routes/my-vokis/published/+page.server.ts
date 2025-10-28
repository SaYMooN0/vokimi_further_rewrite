import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ cookies, fetch }) => {
    cookies.set('my-vokis-last-tab', 'published', {
        path: '/',
        maxAge: 3 * 24 * 3600 // 3 days
    });


    return {
        currentTab: 'published',
        response: await ApiVokisCatalog.serverFetchJsonResponse<{ vokiIds: string[] }>(
            fetch, '/vokis/list-user-voki-ids', { method: 'GET' }
        )
    };
};

