import type { GeneralVokiResultsVisibility } from "../../types"

export type ViewAllVokiResultsResponse = {
    vokiName: string;
    results: VokiResultWithDistributionPercent[];
    showResultsDistribution: boolean;
    resultsVisibility: GeneralVokiResultsVisibility;
}
export type VokiResultWithDistributionPercent = {
    Id: string
    Name: string
    Image: string | null
    DistributionPercent: number
}