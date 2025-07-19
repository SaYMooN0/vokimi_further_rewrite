import { redirect, type ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ cookies }) => {
   	const lastTab = cookies.get('my-vokis-last-tab');
	if (lastTab === 'published') {
        throw redirect(301, '/my-vokis/published');
    }
    throw redirect(301, '/my-vokis/draft');
}
