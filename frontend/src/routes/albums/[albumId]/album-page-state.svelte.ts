import type { VokiItemViewOkStateProps } from "$lib/components/voki_item/c_voki_item/types";
import type { VokiItemViewState } from "$lib/components/voki_item/VokiItemView.svelte";
import { PublishedVokisStore } from "$lib/ts/stores/published-vokis-store.svelte";
import type { PublishedVokiBriefInfo } from "$lib/ts/voki";
import type { VokiType } from "$lib/ts/voki-type";
import { toast } from "svelte-sonner";
import { SvelteSet } from "svelte/reactivity";
import type { PublishedVokiViewState } from "../../my-vokis/published/my-published-vokis-cache-store.svelte";

export class AlbumPageState {
    allLoadedVokis: PublishedVokiViewState[] = $state([]);
    filterAndSort = $state({
        chosenVokiTypes: new SvelteSet<VokiType>(),
        currentSortOption: "Newest" as "From A to Z" | "From Z to A" | "Newest" | "Oldest"
    });

    readonly allSortOptions = ["From A to Z", "From Z to A", "Newest", "Oldest"] as const;
    readonly #onMoreBtnClick: (e: MouseEvent) => void;
    readonly vokiIds: string[];
    constructor(vokiIds: string[], openContextMenu: (mEvent: MouseEvent) => void) {
        this.vokiIds = vokiIds;
        this.loadVokis();
        this.#onMoreBtnClick = (e) => openContextMenu(e);
    }
    loadVokis() {
        this.allLoadedVokis = this.vokiIds.map((id) => PublishedVokisStore.Get(id));
    }

    forceRefetch() {
        PublishedVokisStore.Clear();
        this.loadVokis();
    }

    private mapToView(item: PublishedVokiBriefInfo): VokiItemViewOkStateProps {
        return {
            vokiId: item.id,
            type: item.type,
            voki: {
                name: item.name,
                cover: item.cover,
                primaryAuthorId: item.primaryAuthorId,
                coAuthorIds: item.coAuthorIds
            },
            link: `/voki/${item.id}`,
            onMoreBtnClick: (e) => this.#onMoreBtnClick(e),
            flags: {
                language: item.language,
                hasMatureContent: item.hasMatureContent,
                authenticatedOnlyTaking: item.signedInOnlyTaking
            }
        };
    }

    sortedAndFilteredVokis: () => VokiItemViewState[] = $derived(() => {
        const okItems: PublishedVokiBriefInfo[] = [];
        const nonOkItems: VokiItemViewState[] = [];

        for (const item of this.allLoadedVokis) {
            if (item.state === "ok") {
                okItems.push(item.data);
            } else if (item.state === "errs") {
                nonOkItems.push({
                    name: "errs",
                    data: { errs: item.errs }
                });
            } else {
                nonOkItems.push({ name: "loading" });
            }
        }

        let filtered = okItems;
        if (this.filterAndSort.chosenVokiTypes.size > 0) {
            filtered = filtered.filter((v) =>
                this.filterAndSort.chosenVokiTypes.has(v.type)
            );
        }

        const sort = this.filterAndSort.currentSortOption;
        filtered.sort((a, b) => {
            switch (sort) {
                case "From A to Z":
                    return a.name.localeCompare(b.name);

                case "From Z to A":
                    return b.name.localeCompare(a.name);

                case "Newest":
                    return b.publicationDate.getTime() - a.publicationDate.getTime();

                case "Oldest":
                    return a.publicationDate.getTime() - b.publicationDate.getTime();

                default:
                    return 0;
            }
        });

        const okConverted: VokiItemViewState[] = filtered.map((v) => ({
            name: "ok",
            data: this.mapToView(v)
        }));

        return [...okConverted, ...nonOkItems];
    });

    chooseSortOption(opt: string) {
        if (this.allSortOptions.includes(opt as any)) {
            this.filterAndSort.currentSortOption = opt as any;
        } else {
            toast.error("Unknown sort option");
        }
    }

    toggleTypeFilter(type: VokiType) {
        if (this.filterAndSort.chosenVokiTypes.has(type)) {
            this.filterAndSort.chosenVokiTypes.delete(type);
        } else {
            this.filterAndSort.chosenVokiTypes.add(type);
        }
    }
}
