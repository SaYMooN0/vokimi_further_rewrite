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
    takings: GeneralVokiTakenRecordData[]
}
export type GeneralVokiTakenRecordData = {
    id: string;
    start: Date,
    finish: Date
}