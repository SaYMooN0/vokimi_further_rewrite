import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import { redirect, type ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingData } from "./types";
import { RequestJsonOptions } from "$lib/ts/request-json-options";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";

export const load: ServerLoad = async ({ cookies, params, fetch }) => {
    const vokiId = params.vokiId;
    if (!vokiId) {
        throw redirect(302, `/`);
    }
    if (!VokiCatalogVisitMarkerCookie.checkIfSeen(cookies, vokiId)) {
        throw redirect(302, `/catalog/${vokiId}`);
    }
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<GeneralVokiTakingData>(
            fetch, `/vokis/${params.vokiId}/start-taking`, RequestJsonOptions.POST({})
        ),
        vokiId: vokiId,
        vokiTypeName: "General"
    }
}
