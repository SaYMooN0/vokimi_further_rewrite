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

    goToPreviousQuestion(): void {
        if (this.currentQuestionOrder > 0) {
            this.currentQuestionOrder -= 1;
        }
    }
    goToNextQuestion(): void {
        if (this.currentQuestionOrder < this.#questions.length - 1) {
            this.currentQuestionOrder += 1;
        }
    }
    jumpToSpecificQuestion(questionIndex: number): Err[] {
        if (questionIndex < 0 || questionIndex >= this.#questions.length) {
            return [{ message: "Cannot jump to specific question, because it does not exist" }];
        }
        this.currentQuestionOrder = questionIndex;
        return [];

    }

    isCurrentQuestionLast() { return this.currentQuestionOrder === this.#questions.length - 1; }
    isCurrentQuestionFirst() { return this.currentQuestionOrder === 0; }
    totalQuestionsCount() { return this.#questions.length }

    async finishTakingAndReceiveResult(): Promise<Err[] | GeneralVokiTakingResultData> {
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
}