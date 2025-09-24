import type { GeneralVokiResultsVisibility } from "../../types";

export type ViewReceivedResultsData = {
    results: ResultPreviewWithUserTakingsData[];
    resultsVisibility: GeneralVokiResultsVisibility;
    vokiName: string;
    resultsCount: number;
}
export type ResultPreviewWithUserTakingsData = {
    id: string,
    name: string,
    image: string | null,
    takings: TakingPeriodData[]
}
export type TakingPeriodData = {
    start: Date,
    finish: Date
}