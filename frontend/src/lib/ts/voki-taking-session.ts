export type ExistingActiveSessionForVokiData = {
    vokiId: string;
    sessionId: string;
    startedAt: Date;
    questionsWithSavedAnswersCount: number;
    totalQuestionsCount: number;
};