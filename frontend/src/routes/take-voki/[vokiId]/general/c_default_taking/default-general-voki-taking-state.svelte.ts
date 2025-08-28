import type { Err } from "$lib/ts/err";
import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData, GeneralVokiTakingResultData } from "../types";

export class DefaultGeneralVokiTakingState {
    readonly vokiId: string;
    readonly #takingSessionId: string;
    readonly #serverStartTime: Date;
    readonly #questions: GeneralVokiTakingQuestionData[];

    receivedResult: GeneralVokiTakingResultData | null = $state(null);
    //question id : answer id - is selected
    readonly chosenAnswers = $state(new Map<string, Map<string, boolean>>());
    currentQuestionIndex = $state(0);

    constructor(data: GeneralVokiTakingData) {
        if (data.forceSequentialAnswering) {
            throw new Error("Cannot create GeneralVokiTakingState, because voki force sequential answering");
        }

        this.vokiId = data.id;
        this.#takingSessionId = data.takingSessionId;
        this.#serverStartTime = data.serverStartTime;
        this.#questions = data.questions;
        this.#questions.forEach(q => this.chosenAnswers.set(q.id, new Map(q.answers.map(a => [a.id, false]))));
    }
    goToPreviousQuestion(): Err[] {
        if (this.currentQuestionIndex > 0) {
            this.currentQuestionIndex -= 1;
            return [];
        }
        return [{ message: "Cannot go to previous question, because it is the first question" }];
    }
    goToNextQuestion(): Err[] {
        if (this.currentQuestionIndex < this.#questions.length - 1) {
            this.currentQuestionIndex += 1;
            return [];
        }
        return [{ message: "Cannot go to next question, because it is the last question" }];
    }
    jumpToSpecificQuestion(questionIndex: number): Err[] {
        if (questionIndex < 0 || questionIndex >= this.#questions.length) {
            return [{ message: "Cannot jump to specific question, because it does not exist" }];
        }
        this.currentQuestionIndex = questionIndex;
        return [];

    }
    async finishAndSend(): Promise<Err[]> {
        return [];
    }
    isCurrentQuestionLast(): boolean {
        return this.currentQuestionIndex === this.#questions.length - 1;
    }
    isCurrentQuestionFirst(): boolean {
        return this.currentQuestionIndex === 0;
    }
}