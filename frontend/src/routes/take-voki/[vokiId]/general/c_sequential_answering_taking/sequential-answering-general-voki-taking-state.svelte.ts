import { ApiVokiTakingGeneral, RJO } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData } from "../types";

export class SequentialAnsweringGeneralVokiTakingState {
    readonly vokiId: string;
    readonly #sessionId: string;
    readonly #serverSessionStartTime: Date;
    readonly #clientSessionStartTime: Date;
    readonly totalQuestionsCount: number;
    readonly #clearVokiSeenUpdateTimer: () => void;

    currentQuestionChosenAnswers: Record<string, boolean> = $state({});
    currentQuestion = $state<GeneralVokiTakingQuestionData & CurrentQuestionShownAtTime>();
    isLoadingNextQuestion = $state(false);

    get isCurrentQuestionLast() {
        return this.currentQuestion?.orderInVokiTaking === this.totalQuestionsCount - 1;
    }
    get isCurrentQuestionFirst() {
        if (this.currentQuestion) {
            return this.currentQuestion?.orderInVokiTaking === 0
        }
        return false;
    }

    constructor(data: GeneralVokiTakingData, clearVokiSeenUpdateTimer: () => void) {
        if (!data.forceSequentialAnswering) {
            throw new Error("Cannot create voki taking state, because voki is with free answering");
        }

        this.vokiId = data.id;
        this.#sessionId = data.sessionId;
        this.#serverSessionStartTime = data.startedAt;
        this.#clientSessionStartTime = new Date();
        this.totalQuestionsCount = data.totalQuestionsCount;
        this.#clearVokiSeenUpdateTimer = clearVokiSeenUpdateTimer;

        if (data.questions.length != 1) {
            throw new Error("Cannot create voki taking state, because data contains incorrect questions count");
        }
        const firstQuestion = data.questions[0];
        this.currentQuestion = { ...firstQuestion, clientShownAt: new Date(), serverShownAt: data.startedAt };
        this.resetCurrentAnswers();
    }
    async goToNextQuestion(): Promise<Err[]> {
        if (this.currentQuestion === undefined) {
            return [{ message: "No current question" }];
        }
        if (this.isCurrentQuestionLast) {
            return [{ message: "Cannot go to next question, because current question is last" }];
        }
        const err = this.checkErrsForCurrentQuestion();
        if (err) {
            return [err];
        }
        this.isLoadingNextQuestion = true;
        const response = await ApiVokiTakingGeneral.fetchJsonResponse<GeneralVokiTakingQuestionData & { serverShownAt: Date }>(
            `/vokis/${this.vokiId}/sequential-answering/answer-question`,
            RJO.POST({
                SessionId: this.#sessionId,
                questionId: this.currentQuestion.id,
                questionOrderInVokiTaking: this.currentQuestion.orderInVokiTaking,
                chosenAnswers: this.currentQuestionChosenAnswers,
                serverQuestionShownAt: this.currentQuestion.serverShownAt,
                clientQuestionShownAt: this.currentQuestion.clientShownAt,
                clientQuestionAnsweredAt: new Date()
            })
        );
        this.isLoadingNextQuestion = false;
        if (response.isSuccess) {
            this.currentQuestion = { ...response.data, clientShownAt: new Date() };
            this.resetCurrentAnswers();

            return [];
        }
        return response.errs;
    }
    async finishTakingAndReceiveResult(): Promise<
        | { isSuccess: false, errs: Err[] }
        | ResponseResult<{ receivedResultId: string }>
    > {
        if (this.currentQuestion === undefined) {
            return { isSuccess: false, errs: [{ message: "No current question" }] };
        }
        if (!this.isCurrentQuestionLast) {
            return { isSuccess: false, errs: [{ message: "Cannot finish taking and receive result, because current question is not last" }] };
        }
        const err = this.checkErrsForCurrentQuestion();
        if (err) {
            return { isSuccess: false, errs: [err] };
        }
        this.isLoadingNextQuestion = true;
        const response = await ApiVokiTakingGeneral.fetchJsonResponse<{ receivedResultId: string }>(
            `/vokis/${this.vokiId}/sequential-answering/finish`,
            RJO.POST({
                SessionId: this.#sessionId,
                LastQuestionId: this.currentQuestion.id,
                LastQuestionOrderInVokiTaking: this.currentQuestion.orderInVokiTaking,
                LastQuestionChosenAnswers: this.currentQuestionChosenAnswers,
                ServerLastQuestionShownAt: this.currentQuestion.serverShownAt,
                ClientLastQuestionShownAt: this.currentQuestion.clientShownAt,
                ClientLastQuestionAnsweredAt: new Date(),
                ServerSessionStartTime: this.#serverSessionStartTime,
                ClientSessionStartTime: this.#clientSessionStartTime,
                ClientSessionFinishTime: new Date()
            })
        );
        this.isLoadingNextQuestion = false;
        if (response.isSuccess) {
            this.#clearVokiSeenUpdateTimer();
            return response;
        }
        return response;
    }
    checkErrsForCurrentQuestion(): Err | null {
        const min = this.currentQuestion?.minAnswersCount ?? 0;
        const max = this.currentQuestion?.maxAnswersCount ?? Number.POSITIVE_INFINITY;
        const chosenCount = Object.values(this.currentQuestionChosenAnswers).filter(Boolean).length;

        if (chosenCount === 0 && min > 0) {
            return { message: `You haven't answered this question yet. Please select at least ${min} ${min === 1 ? 'answer' : 'answers'} to continue.` };
        }

        if (chosenCount < min) {
            return { message: `Please choose at least ${min} ${min === 1 ? 'answer' : 'answers'} to continue.` };
        }

        if (chosenCount > max) {
            return { message: `You selected ${chosenCount} answers, but only up to ${max} ${max === 1 ? 'answer' : 'answers'} ${max === 1 ? 'is' : 'are'} allowed.` };
        }

        return null;
    }
    resetCurrentAnswers() {
        this.currentQuestionChosenAnswers = Object.fromEntries(
            this.currentQuestion!.answers.map(a => [a.id, false])
        );
    }

}
type CurrentQuestionShownAtTime = { serverShownAt: Date, clientShownAt: Date }