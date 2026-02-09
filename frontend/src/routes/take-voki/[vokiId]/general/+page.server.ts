import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import { redirect, type ServerLoad } from "@sveltejs/kit";
import type { BaseVokiTakingSessionData, ContinueGeneralVokiTakingData, StartGeneralVokiTakingData } from "./types";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";
import { ContinueVokiTakingSessionMarkerCookie } from "$lib/ts/cookies/continue-voki-taking-session-marker";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { VokiType } from "$lib/ts/voki-type";

export const load: ServerLoad = async ({ cookies, params, fetch, url }): Promise<ServerReturnType> => {
    const vokiId = params.vokiId;
    if (!vokiId) {
        throw redirect(302, `/`);
    }
    if (url.searchParams.get('terminateExistingUnfinishedSession') === 'true') {
        return await startNewSessionResult(fetch, vokiId, true);
    }
    if (url.searchParams.get('continueExistingActiveSession') === 'true') {
        const continueSessionId = ContinueVokiTakingSessionMarkerCookie.get(cookies, vokiId);
        if (!continueSessionId) {
            return {
                vokiId: vokiId,
                vokiType: "General",
                sessionActionResult: "ContinueErr:NoSessionId"
            }
        }
        ContinueVokiTakingSessionMarkerCookie.clear(vokiId);
        return {
            vokiId: vokiId,
            vokiType: "General",
            sessionActionResult: "ContinuedExisting",
            response: await ApiVokiTakingGeneral.serverFetchJsonResponse<ContinueGeneralVokiTakingData>(
                fetch, `/vokis/${params.vokiId}/continue-taking`, RJO.POST({})
            )
        };

    }
    if (!VokiCatalogVisitMarkerCookie.checkIfSeen(cookies, vokiId)) {
        throw redirect(302, `/catalog/${vokiId}`);
    }
    return await startNewSessionResult(fetch, vokiId, false);
}
async function startNewSessionResult(
    fetchFunc: typeof fetch,
    vokiId: string,
    terminateExistingUnfinishedSession: boolean
): Promise<Extract<ServerReturnType, { sessionActionResult: "NewStarted" }>> {
    const response = await ApiVokiTakingGeneral.serverFetchJsonResponse<StartVokiTakingResponseType>(
        fetchFunc, `/vokis/${vokiId}/start-taking`, RJO.POST({
            terminateExistingUnfinishedSession
        })
    );
    return {
        response: response,
        vokiId: vokiId,
        vokiType: "General",
        sessionActionResult: "NewStarted"
    }

}
type ServerReturnType =
    {
        vokiId: string,
        vokiType: VokiType,
    }
    & (
        | { sessionActionResult: "ContinueErr:NoSessionId" }
        | { sessionActionResult: "ContinuedExisting", response: ResponseResult<ContinueGeneralVokiTakingData> }
        | { sessionActionResult: "NewStarted", response: ResponseResult<StartVokiTakingResponseType> }
    )

type StartVokiTakingResponseType =
    | ({ newSessionStarted: true } & StartGeneralVokiTakingData)
    | ({ newSessionStarted: false, questionsWithSavedAnswersCount: number; } & BaseVokiTakingSessionData);