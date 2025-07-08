import { ApiVokiCreationGeneral } from "$lib/ts/backend-services";
import type { Language } from "$lib/ts/language";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<VokiMainInfo>(
        fetch, `/vokis/${params.vokiId}/main-info`, { method: 'GET' }
    );
    return { ...response };
}
export type VokiMainInfo = {
    name: string;
    cover: string;
    details: VokiDetails;
    tags: string[];
}

export type VokiDetails = {
    description: string;
    isAgeRestricted: boolean;
    language: Language;
}