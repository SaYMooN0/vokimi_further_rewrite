import { error } from '@sveltejs/kit';
import type { LayoutServerLoad } from './$types';
import { MyVokisPageTabMarker } from './tab-marker';
export const ssr = true;
export const prerender = true;
export const load: LayoutServerLoad = async ({ url, cookies }): Promise<{ currentTab: MyVokisPageTabMarker.Tab }> => {
	console.log(url);

	const currentTab = extractTabFromPathname(url.pathname);

	if (!currentTab) {
		error(404, 'Not found');
	}
	else {

		cookies.set(MyVokisPageTabMarker.cookieName, currentTab, {
			path: "/",
			maxAge: 24 * 3600 // 1 day
		});

		return {
			currentTab: currentTab
		};
	}
};

function extractTabFromPathname(
	pathname: string
): MyVokisPageTabMarker.Tab | null {
	for (const segment of pathname.split('/')) {
		if (MyVokisPageTabMarker.isTab(segment)) {
			return segment;
		}
	}
	return null;
}