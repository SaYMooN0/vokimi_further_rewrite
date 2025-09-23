import type { GeneralVokiResultsVisibility } from "../../types";

export type GeneralVokiSingleResultViewData = {
    id: string;
    name: string;
    text: string;
    image: string | null;
    ResultsVisibility: GeneralVokiResultsVisibility;
    ResultsCount: number;
}