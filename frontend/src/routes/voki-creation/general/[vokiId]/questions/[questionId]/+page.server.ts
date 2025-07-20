import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { ServerLoad } from "@sveltejs/kit";
import type { QuestionFullData } from "../types";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<QuestionFullData>(
        fetch, `/vokis/${params.vokiId}/questions/${params.questionId}`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId, questionId: params.questionId };
}

