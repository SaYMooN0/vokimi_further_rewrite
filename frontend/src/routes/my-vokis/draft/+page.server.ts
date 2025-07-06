import { ApiVokiCreationCore } from "$lib/ts/backend-services";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ cookies, fetch }) => {
    cookies.set('my-vokis-last-tab', 'draft', {
        path: '/',
        maxAge: 3 * 24 * 3600 // 3 days
    });
    const response = await ApiVokiCreationCore.serverFetchJsonResponse<{ vokiIds: string[] }>(
        fetch, '/list-user-voki-ids', { method: 'GET' }
    );
    const pageData = response.isSuccess ? { draftVokiIds: response.data.vokiIds } : { errs: response.errs };
    return {
        currentTab: 'draft',
        ...pageData
    };
};