import type { VokiPageTab } from "./+page.server";

export class VokiPageState {
    vokiId: string;
    pageTab: VokiPageTab = 'about';
    constructor(vokiId: string, pageTab: VokiPageTab) {
        this.vokiId = vokiId;
        this.pageTab = pageTab;
    }
}