import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import { redirect, type ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingData } from "./types";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";

export const load: ServerLoad = async ({ cookies, params, fetch }) => {
    params.terminateCurrentActive
    const vokiId = params.vokiId;
    if (!vokiId) {
        throw redirect(302, `/`);
    }
    if (!VokiCatalogVisitMarkerCookie.checkIfSeen(cookies, vokiId)) {
        throw redirect(302, `/catalog/${vokiId}`);
    }
    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<GeneralVokiTakingData>(
            fetch, `/vokis/${params.vokiId}/start-taking`, RJO.POST({})
        ),
        vokiId: vokiId,
        vokiTypeName: "General"
    }
}
