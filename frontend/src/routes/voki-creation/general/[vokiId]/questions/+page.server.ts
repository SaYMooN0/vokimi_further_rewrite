import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { GeneralVokiAnswerType } from "$lib/ts/voki";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokiCreationGeneral.serverFetchJsonResponse<{
        questions: QuestionOverviewInfo[],
        settings: QuestionSettings
    }>(
        fetch, `/vokis/${params.vokiId}/questions/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
}
export type QuestionOverviewInfo = {
    id: string;
    text: string;
    images: string[];
    answersType: GeneralVokiAnswerType;
    answersCount: number;
    orderInVoki: number;
    isMultipleChoice: boolean;
}
export type QuestionSettings = {
    forceSequentialAnswering: boolean;
    shuffleQuestions: boolean;
}