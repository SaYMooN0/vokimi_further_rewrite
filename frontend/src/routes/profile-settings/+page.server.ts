import { ApiUserProfiles } from "$lib/ts/backend-communication/backend-services";
import type { PageServerLoad } from "./$types";


export const load: PageServerLoad = async ({ fetch }) => {

    return {
        response: await ApiUserProfiles.serverFetchJsonResponse<UserProfileSettingsData>(
            fetch, `/profile-settings`, { method: 'GET' }
        ),
    };
};

