import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import { redirect, type ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingData } from "./types";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";
import { ContinueVokiTakingSessionMarkerCookie } from "$lib/ts/cookies/continue-voki-taking-session-marker";

export const load: ServerLoad = async ({ cookies, params, fetch, url }) => {

    const vokiId = params.vokiId;
    if (!vokiId) {
        throw redirect(302, `/`);
    }
    if (url.searchParams.get('continueExistingActiveSession')) {

        const continueSessionId = ContinueVokiTakingSessionMarkerCookie.get(cookies, vokiId);
        if (continueSessionId) {
            ContinueVokiTakingSessionMarkerCookie.clear(vokiId);
        }
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
