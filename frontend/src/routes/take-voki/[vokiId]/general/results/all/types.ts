import type { GeneralVokiResultsVisibility } from "../../types"

export type ViewAllVokiResultsResponse = {
    vokiName: string;
    results: VokiResultWithDistributionPercent[];
    showResultsDistribution: boolean;
    resultsVisibility: GeneralVokiResultsVisibility;
}
export type VokiResultWithDistributionPercent = {
    id: string
    name: string
    image: string | null
    distributionPercent: number
}