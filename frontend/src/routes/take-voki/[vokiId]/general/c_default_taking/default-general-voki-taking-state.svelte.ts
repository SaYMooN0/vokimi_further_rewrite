import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { RequestJsonOptions } from "$lib/ts/request-json-options";
import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData, GeneralVokiTakingResultData } from "../types";

export class DefaultGeneralVokiTakingState {
    readonly vokiId: string;
    readonly #sessionId: string;
    readonly #serverStartedAt: Date;
    readonly #clientStartedAt: Date;
    readonly #questions: GeneralVokiTakingQuestionData[];

    receivedResult: GeneralVokiTakingResultData | null = $state(null);
    //question id : answer id - is selected
    readonly chosenAnswers = $state<Record<string, Record<string, boolean>>>({});
    currentQuestionOrder = $state(0);
    currentQuestion: GeneralVokiTakingQuestionData | undefined;

    constructor(data: GeneralVokiTakingData) {
        if (data.forceSequentialAnswering) {
            throw new Error("Cannot create GeneralVokiTakingState, because voki force sequential answering");
        }

        this.vokiId = data.id;
        this.#sessionId = data.sessionId;
        this.#serverStartedAt = data.startedAt;
        this.#clientStartedAt = new Date();
        this.#questions = data.questions;
        this.#questions.forEach(q => {
            this.chosenAnswers[q.id] = Object.fromEntries(
                q.answers.map(a => [a.id, false])
            ) as Record<string, boolean>;
        });

        this.currentQuestion = $derived<GeneralVokiTakingQuestionData | undefined>(
            this.#questions.find(q => q.orderInVokiTaking === this.currentQuestionOrder)
        );
    }

    goToPreviousQuestion(): Err[] {
        if (this.currentQuestionOrder > 0) {
            this.currentQuestionOrder -= 1;
            return [];
        }
        return [{ message: "Cannot go to previous question, because it is the first question" }];
    }
    goToNextQuestion(): Err[] {
        if (this.currentQuestionOrder < this.#questions.length - 1) {
            this.currentQuestionOrder += 1;
            return [];
        }
        return [{ message: "Cannot go to next question, because it is the last question" }];
    }
    jumpToSpecificQuestion(questionIndex: number): Err[] {
        if (questionIndex < 0 || questionIndex >= this.#questions.length) {
            return [{ message: "Cannot jump to specific question, because it does not exist" }];
        }
        this.currentQuestionOrder = questionIndex;
        return [];

    }

    isCurrentQuestionLast(): boolean {
        return this.currentQuestionOrder === this.#questions.length - 1;
    }
    isCurrentQuestionFirst(): boolean {
        return this.currentQuestionOrder === 0;
    }

    async finishAndSend(): Promise<Err[]> {
        const errs = this.checkErrsBeforeFinish();
        if (errs.length > 0) {
            return errs;
        }
        const response = await ApiVokiTakingGeneral.fetchJsonResponse<{}>(
            `/vokis/${this.vokiId}/finish-taking-with-free-answering`,
            RequestJsonOptions.POST({
                serverStart: this.#serverStartedAt,
                clientStart: this.#clientStartedAt,
                sessionId: this.#sessionId,

            })
        );

        if (response.isSuccess) {
            //remove cookies
            //show result
        } else {
            return response.errs;
        }
        return [];
    }
    checkErrsBeforeFinish(): Err[] {
        return [];
    }
    totalQuestionsCount() {
        return this.#questions.length
    }
}