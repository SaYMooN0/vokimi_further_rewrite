import type { VokiPageTab } from "./+page.server";

export class VokiPageState {
    vokiId: string;
    currentTab: VokiPageTab = $state('about');
    constructor(vokiId: string, pageTab: VokiPageTab) {
        this.vokiId = vokiId;
        this.currentTab = pageTab;
    }
}