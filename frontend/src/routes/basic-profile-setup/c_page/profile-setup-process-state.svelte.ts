import { ApiTags } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import { SvelteSet } from "svelte/reactivity";


export class ProfileSetupProcessState {

    #tagsSuggestionsState: TagSuggestionsState = $state(
        { name: 'loading', ...ProfileSetupProcessState.#tagsStateEmptySuggestions() },
    );

    readonly #chosenLanguages = new SvelteSet<Language>();
    readonly chosenFavoriteTags = new SvelteSet<Tag>();
    readonly profilePicInputValue = $state<string>("");
    readonly displayNameInputValue = $state<string>("");

    readonly suggestedTags = $derived<() => Set<Tag>>(() => {
        const tags = new Set<string>();
        for (const lang of this.#chosenLanguages) {
            const arr = this.#tagsSuggestionsState.suggestionsByLangs[lang];
            if (arr) arr.forEach((t) => tags.add(t));
        }
        this.#tagsSuggestionsState.defaultSuggestions.forEach((t) => tags.add(t));
        return tags;
    });

    constructor(initialLangs: Language[], initialTags: Tag[], initialProfilePic: string, initialDisplayName: string) {

        initialLangs.forEach((l) => this.#chosenLanguages.add(l));
        initialTags.forEach((t) => this.chosenFavoriteTags.add(t));
        this.profilePicInputValue = initialProfilePic;
        this.displayNameInputValue = initialDisplayName;
        this.#loadTagsSuggestion();
    }

    isLanguageChosen(language: Language): boolean {
        return this.#chosenLanguages.has(language);
    }

    toggleLanguage(language: Language): void {
        if (this.#chosenLanguages.has(language)) {
            this.#chosenLanguages.delete(language);
        } else {
            this.#chosenLanguages.add(language);
        }
    }
    anyLanguagesChosen() {
        return this.#chosenLanguages.size > 0
    }
    chooseTag(tag: Tag): void {
        this.chosenFavoriteTags.add(tag);

    }
    removeChosenTag(tag: Tag): void {
        this.chosenFavoriteTags.delete(tag);
    }
    static #tagsStateEmptySuggestions(): TagSuggestionsResponse {
        return {
            defaultSuggestions: [], suggestionsByLangs: {
                'Eng': [], 'Rus': [], 'Spa': [], 'Ger': [],
                'Fra': [], 'Ukr': [], 'Por': [], 'Other': []
            }
        }
    };
    async #loadTagsSuggestion(): Promise<void> {
        const response = await ApiTags.fetchJsonResponse<TagSuggestionsResponse>
    }
}

type Tag = string;
type TagSuggestionsResponse = { defaultSuggestions: Tag[], suggestionsByLangs: Record<Language, Tag[]> };
type TagSuggestionsState = TagSuggestionsResponse & (
    | { name: 'loading' }
    | { name: 'ok' }
    | { name: 'errs', errs: Err[] }
);
