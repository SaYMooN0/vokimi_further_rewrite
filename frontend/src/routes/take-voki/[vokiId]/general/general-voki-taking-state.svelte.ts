import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData } from "./types";

export class GeneralVokiTakingState {
    readonly #vokiId: string;
    readonly #forceSequentialAnswering: boolean;
    readonly #takingSessionId: string;
    readonly #serverStartTime: Date;
    readonly #questions: GeneralVokiTakingQuestionData[];
    currentQuestionIndex = $state(0);

    constructor(data: GeneralVokiTakingData) {
        this.#vokiId = data.id;
        this.#forceSequentialAnswering = data.forceSequentialAnswering;
        this.#takingSessionId = data.takingSessionId;
        this.#serverStartTime = data.serverStartTime;
        this.#questions = data.questions;
    }
    goToPreviousQuestion() {
        if (this.#forceSequentialAnswering) { return; }
        if (this.currentQuestionIndex > 0) {
            this.currentQuestionIndex -= 1;
        }
    }
    goToNextQuestion() {
        if (this.currentQuestionIndex < this.#questions.length - 1) {
            this.currentQuestionIndex += 1;
        }
    }
}