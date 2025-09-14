import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { Err } from "$lib/ts/err";
import { RequestJsonOptions } from "$lib/ts/request-json-options";
import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData, GeneralVokiTakingResultData } from "../types";

export class DefaultGeneralVokiTakingState {
    readonly vokiId: string;
    readonly #sessionId: string;
    readonly #serverStartedAt: Date;
    readonly #clientStartedAt: Date;
    readonly #questions: GeneralVokiTakingQuestionData[];
    readonly totalQuestionsCount: number;

    readonly chosenAnswers = $state<Record<string, Record<string, boolean>>>({});
    currentQuestionOrder = $state(0);
    currentQuestion: GeneralVokiTakingQuestionData | undefined;

    constructor(data: GeneralVokiTakingData) {
        if (data.forceSequentialAnswering) {
            throw new Error(
                "Cannot create GeneralVokiTakingState, because voki force sequential answering"
            );
        }

        this.vokiId = data.id;
        this.#sessionId = data.sessionId;
        this.#serverStartedAt = data.startedAt;
        this.#clientStartedAt = new Date();
        this.#questions = data.questions;
        this.totalQuestionsCount = data.totalQuestionsCount;

        if (this.#questions.length !== this.totalQuestionsCount) {
            throw new Error(
                `GeneralVokiTakingState inconsistency: questions length (${this.#questions.length}) != totalQuestionsCount (${this.totalQuestionsCount})`
            );
        }

        this.#questions.forEach(q => {
            this.chosenAnswers[q.id] = Object.fromEntries(
                q.answers.map(a => [a.id, false])
            ) as Record<string, boolean>;
        });

        this.currentQuestion = $derived<GeneralVokiTakingQuestionData | undefined>(
            this.#questions.find(
                q => q.orderInVokiTaking === this.currentQuestionOrder
            )
        );
    }

    goToPreviousQuestion(): void {
        if (this.currentQuestionOrder > 0) {
            this.currentQuestionOrder -= 1;
        }
    }

    goToNextQuestion(): void {
        if (this.currentQuestionOrder < this.totalQuestionsCount - 1) {
            this.currentQuestionOrder += 1;
        }
    }

    jumpToSpecificQuestion(orderInVokiTaking: number): Err[] {
        if (orderInVokiTaking === this.currentQuestion?.orderInVokiTaking) {
            return [];
        }
        const q = this.#questions.find(q => q.orderInVokiTaking === orderInVokiTaking);
        if (!q) {
            return [{
                message: `Cannot jump to specific question, because question with order ${orderInVokiTaking} does not exist`
            }];
        }
        this.currentQuestionOrder = q.orderInVokiTaking;
        return [];
    }


    isCurrentQuestionLast() {
        return this.currentQuestionOrder === this.totalQuestionsCount - 1;
    }

    isCurrentQuestionFirst() {
        return this.currentQuestionOrder === 0;
    }

    async finishTakingAndReceiveResult(): Promise<
        | { isSuccess: false, errs: ErrMessageWithQuestionOrder[] }
        | ResponseResult<GeneralVokiTakingResultData>
    > {
        const errs = this.checkErrsBeforeFinish();
        if (errs.length > 0) {
            return { isSuccess: false, errs };
        }
        const response = await ApiVokiTakingGeneral.fetchJsonResponse<{}>(
            `/vokis/${this.vokiId}/finish-taking-with-free-answering`,
            RequestJsonOptions.POST({
                serverStart: this.#serverStartedAt,
                clientStart: this.#clientStartedAt,
                sessionId: this.#sessionId
            })
        );

        if (response.isSuccess) {
            // remove cookies
            // show result
        }
        return response;
    }

    checkErrsBeforeFinish(): ErrMessageWithQuestionOrder[] {
        const errs: ErrMessageWithQuestionOrder[] = [];

        for (const q of this.#questions) {
            const chosenMap = this.chosenAnswers[q.id] ?? {};
            const chosenCount = Object.values(chosenMap).filter(Boolean).length;

            const min = q.minAnswersCount ?? 0;
            const max = q.maxAnswersCount ?? Number.POSITIVE_INFINITY;

            const orderHuman = (q.orderInVokiTaking ?? 0) + 1;
            const preview =
                !q.text || q.text.length < 40 ? (q.text ?? '') : `${q.text.slice(0, 30)}…`;

            const title = `#${orderHuman} '${preview}'`;

            if (chosenCount === 0 && min > 0) {
                errs.push({
                    message: `Question ${title} is not answered. Please choose at least ${min} ${min === 1 ? 'answer' : 'answers'}.`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }

            if (chosenCount < min) {
                errs.push({
                    message: `Question ${title}: you chose ${chosenCount}, but at least ${min} ${min === 1 ? 'answer is' : 'answers are'} required.`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }

            if (chosenCount > max) {
                errs.push({
                    message: `Question ${title}: you chose ${chosenCount}, but at most ${max} ${max === 1 ? 'answer is' : 'answers are'} allowed.`,
                    questionOrder: q.orderInVokiTaking
                });
                continue;
            }
        }

        return errs;
    }


}
type ErrMessageWithQuestionOrder = { message: string, questionOrder: number };