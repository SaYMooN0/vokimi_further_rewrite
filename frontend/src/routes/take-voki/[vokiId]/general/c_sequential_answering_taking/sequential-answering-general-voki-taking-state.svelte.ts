import type { GeneralVokiTakingData } from "../types";

export class SequentialAnsweringGeneralVokiTakingState {
    constructor(data: GeneralVokiTakingData) {
        if (!data.forceSequentialAnswering) {
            throw new Error("Cannot create GeneralVokiTakingState, because voki force sequential answering");
        }

    }
}