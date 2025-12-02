import type { VokiItemViewOkStateProps, VokiItemViewErrStateProps } from "$lib/components/voki_item/c_voki_item/types";
import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { MyPublishedVokisCacheStore } from "./my-published-vokis-cache-store.svelte";


type PublishedVokiIdsState =
    | { state: 'loading' }
    | { state: 'errs'; errs: Err[] }
    | { state: 'loaded'; vokiIds: string[] };

export class MyPublishedVokisPageState {
    publishedVokiIds: PublishedVokiIdsState = $state({ state: 'loading' });

    constructor() {
        this.loadPublishedVokis();
    }

    async forceRefetch() {
        MyPublishedVokisCacheStore.Clear();
        await this.loadPublishedVokis();
    }

    async loadPublishedVokis() {
        this.publishedVokiIds = { state: 'loading' };

        const response = await ApiVokisCatalog.fetchJsonResponse<{ vokiIds: string[] }>(
            '/vokis/list-user-voki-ids',
            { method: 'GET' }
        );

        if (response.isSuccess) {
            this.publishedVokiIds = { state: 'loaded', vokiIds: response.data.vokiIds };
        } else {
            this.publishedVokiIds = { state: 'errs', errs: response.errs };
        }
    }

    getVokiViewItemState(
        vokiId: string,
        openContextMenu: (mEvent: MouseEvent) => void
    ):
        | { name: 'ok'; data: VokiItemViewOkStateProps }
        | { name: 'loading' }
        | { name: 'errs'; data: VokiItemViewErrStateProps } {
        const voki = MyPublishedVokisCacheStore.Get(vokiId);

        if (voki.state === 'loading') {
            return { name: 'loading' };
        }
        if (voki.state === 'errs') {
            return { name: 'errs', data: { vokiId, errs: voki.errs } };
        }

        return {
            name: 'ok',
            data: {
                vokiId,
                type: voki.data.type,
                voki: {
                    name: voki.data.name,
                    cover: voki.data.cover,
                    primaryAuthorId: voki.data.primaryAuthorId,
                    coAuthorIds: voki.data.coAuthorIds
                },
                onMoreBtnClick: (e) => openContextMenu(e),
                link: `/catalog/${vokiId}`
            }
        };
    }
}
