export type GeneralVokiTakenResult = {
    receivedResult: GeneralVokiTakenReceivedVokiResult,
    timeTakenMilliseconds: number;
}
export type GeneralVokiTakenReceivedVokiResult = {
    id: string;
    name: string;
    text: string;
    image: string | null;
}