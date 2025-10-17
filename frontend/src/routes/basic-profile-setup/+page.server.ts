import { ApiUserProfiles } from "$lib/ts/backend-communication/backend-services";
import type { Language } from "$lib/ts/language";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
    return ApiUserProfiles.serverFetchJsonResponse<BasicSetupInfo>(
        fetch, '/basic-setup-info', { method: 'GET' }
    );
};

type  BasicSetupInfo = {
    userUniqueName: string,
    displayName: string,
    preferredLanguages: Language[]
    favoriteTags: string[],
    profilePicture: string,
    maxDisplayNameLength:number
}