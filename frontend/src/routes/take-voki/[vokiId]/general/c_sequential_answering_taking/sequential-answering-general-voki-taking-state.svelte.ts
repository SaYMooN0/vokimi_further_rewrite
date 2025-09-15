import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData, GeneralVokiTakingResultData } from "../types";

export class SequentialAnsweringGeneralVokiTakingState {
    readonly vokiId: string;
    currentQuestion: GeneralVokiTakingQuestionData | undefined;
    readonly #clearVokiSeenUpdateTimer: () => void;

    isCurrentQuestionLast() { return true; }


    constructor(data: GeneralVokiTakingData, clearVokiSeenUpdateTimer: () => void) {
        this.vokiId = data.id;

        if (!data.forceSequentialAnswering) {
            throw new Error("Cannot create GeneralVokiTakingState, because voki force sequential answering");
        }

        this.#clearVokiSeenUpdateTimer = clearVokiSeenUpdateTimer;

    }
    async goToNextQuestion(): Promise<Err[]> {
        //validate current chosen
        // if (this.currentQuestionOrder >= this.totalQuestionsCount - 1) {
        //     return; 
        // }
        //fetch current
        //     this.currentQuestionOrder += 1;
        return [];
    }
    async finishTakingAndReceiveResult(): Promise<ResponseResult<GeneralVokiTakingResultData>> {
        //validate current chosen
        // check if last
        // command finish voki taking
        return { isSuccess: false, errs: [] };
    }
}