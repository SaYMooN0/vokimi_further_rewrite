import { redirect, type ServerLoad } from "@sveltejs/kit";
import { MyVokisPageTabMarker } from "./tab-marker";
export const ssr = true;
export const prerender = true;
export const load: ServerLoad = async ({ cookies }) => {
    const raw = cookies.get(MyVokisPageTabMarker.cookieName);
    const tab = raw && MyVokisPageTabMarker.isTab(raw) ? raw : "draft-vokis";
    throw redirect(307, `/my-vokis/${tab}`);
}
