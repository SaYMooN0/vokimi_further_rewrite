import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import { redirect, type Cookies, type ServerLoad } from "@sveltejs/kit";
import type { BaseVokiTakingSessionData, GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from "./types";
import { VokiCatalogVisitMarkerCookie } from "$lib/ts/cookies/voki-catalog-visit-marker-cookie";
import { ContinueVokiTakingSessionMarkerCookie } from "$lib/ts/cookies/continue-voki-taking-session-marker";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { VokiType } from "$lib/ts/voki-type";

export const load: ServerLoad = async ({ cookies, params, fetch, url }): ServerFullReturtnType => {
    const vokiId = params.vokiId;
    if (!vokiId) {
        throw redirect(302, `/`);
    }
    if (url.searchParams.get('terminateExistingUnfinishedSession') === 'true') {
        return startNewSessionResult(fetch, vokiId, true);
    }
    if (url.searchParams.get('continueExistingUnfinishedSession') === 'true') {
        return continueExistingUnfinishedSessionResult(fetch, vokiId, cookies);
    }
    if (!VokiCatalogVisitMarkerCookie.checkIfSeen(cookies, vokiId)) {
        throw redirect(302, `/catalog/${vokiId}`);
    }
    return startNewSessionResult(fetch, vokiId, false);
}

async function continueExistingUnfinishedSessionResult(
    fetchFunc: typeof fetch,
    vokiId: string,
    cookies: Cookies
): ServerFullReturtnType {
    const continueSessionId = ContinueVokiTakingSessionMarkerCookie.get(cookies, vokiId);
    if (!continueSessionId) {
        return {
            vokiId: vokiId,
            vokiType: "General",
            isSuccess: true,
            data: {
                serverResultType: "ContinueErr:NoSessionId"
            }
        }
    }
    ContinueVokiTakingSessionMarkerCookie.clear(vokiId);
    const response = await ApiVokiTakingGeneral.serverFetchJsonResponse<ContinueTakingServerSuccessResponse>(
        fetchFunc, `/vokis/${vokiId}/continue-taking`, RJO.POST({})
    );
    if (!response.isSuccess) {
        return {
            vokiId: vokiId,
            vokiType: "General",
            isSuccess: false,
            errs: response.errs
        }
    }
    return {
        vokiId: vokiId,
        vokiType: "General",
        isSuccess: true,
        data: {
            serverResultType: "Success",
            sessionData: response.data,
            savedData: {
                anySave: true,
                savedChosenAnswers: response.data.savedChosenAnswers,
                currentQuestionId: response.data.currentQuestionId

            }
        }
    };
}
async function startNewSessionResult(
    fetchFunc: typeof fetch,
    vokiId: string,
    terminateExistingUnfinishedSession: boolean
): ServerFullReturtnType {

    const response = await ApiVokiTakingGeneral.serverFetchJsonResponse<StartTakingServerSuccessResponse>(
        fetchFunc, `/vokis/${vokiId}/start-taking`, RJO.POST({
            terminateExistingUnfinishedSession
        })
    );
    if (!response.isSuccess) {
        return {
            vokiId: vokiId,
            vokiType: "General",
            isSuccess: false,
            errs: response.errs
        }
    }
    if (response.data.newSessionStarted) {
        return {
            vokiId: vokiId,
            vokiType: "General",
            isSuccess: true,
            data: {
                serverResultType: "Success",
                sessionData: response.data,
                savedData: { anySave: false }
            },
        }
    }
    else {
        return {
            vokiId: vokiId,
            vokiType: "General",
            isSuccess: true,
            data: {
                serverResultType: "StartNewErr:UnfinishedSessionExists",
                sessionData: response.data
            },
        }
    }

}
type ServerFullReturtnType = Promise<
    ResponseResult<ServerSuccessType>
    & { vokiId: string; vokiType: VokiType }
>
type ServerSuccessType =
    (
        | { serverResultType: "ContinueErr:NoSessionId" }
        | {
            serverResultType: "StartNewErr:UnfinishedSessionExists",
            sessionData: { questionsWithSavedAnswersCount: number; } & BaseVokiTakingSessionData
        }
        | { serverResultType: "Success", sessionData: GeneralVokiTakingData, savedData: PosssibleGeneralVokiTakingDataSaveData }
    )
type ContinueTakingServerSuccessResponse = GeneralVokiTakingData & {
    savedChosenAnswers: Record<string, string[]>;
    currentQuestionId: string;
}
type StartTakingServerSuccessResponse =
    | ({ newSessionStarted: true } & GeneralVokiTakingData)
    | { newSessionStarted: false; questionsWithSavedAnswersCount: number } & BaseVokiTakingSessionData;