import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ cookies }) => {
    cookies.set('my-vokis-last-tab', 'published', {
        path: '/',
        maxAge: 3 * 24 * 3600 // 3 days
    });
    return {
        currentTab: 'published'
    };
};