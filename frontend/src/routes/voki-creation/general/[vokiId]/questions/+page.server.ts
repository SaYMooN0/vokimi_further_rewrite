import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingProcessSettings, QuestionBriefData } from "./types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<{
        questions: QuestionBriefData[],
        settings: GeneralVokiTakingProcessSettings
    }>(
        fetch, `/vokis/${params.vokiId}/questions/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
