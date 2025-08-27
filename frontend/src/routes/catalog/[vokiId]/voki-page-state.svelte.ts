import type { VokiPageTab } from "./+page.server";

export class VokiPageState {
    vokiId: string;
    currentTab: VokiPageTab = $state('about');
    ratingsCount: number = $state(0);
    commentsCount: number = $state(0);
    constructor(vokiId: string, pageTab: VokiPageTab, ratingsCount: number, commentsCount: number) {
        this.vokiId = vokiId;
        this.currentTab = pageTab;
        this.ratingsCount = ratingsCount;
        this.commentsCount = commentsCount;
    }
}