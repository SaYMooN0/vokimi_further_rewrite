import type { Language } from "$lib/ts/language";
import { SvelteSet } from "svelte/reactivity";

type Tag = string;

export class ProfileSetupState {
    readonly #suggestedForAllLangs = ['music', 'literature', 'valorant', 'youtube', 'programming'];

    readonly #allSuggestedTagsByLang: Record<Language, Tag[]> = {
        Rus: ['мор_утопия', 'валорант', 'Жан_Поль_Сартр'],
        Eng: ['booktok', 'dune', 'elden_ring'],
        Spa: ['don_quijote', 'la_casa_de_papel', 'real_madrid'],
        Ger: ['die_zauberflote', 'schachnovelle', 'heidegger'],
        Fra: ['le_petit_prince', 'les_miserables', 'asterix'],
        Ukr: ['гоголь', 'taras_shevchenko'],
        Por: ['cidade_de_deus', 'os_lusiadas', 'capoeira'],
        Other: ['anime', 'kpop', 'manga']
    };

    readonly #chosenLanguages = new SvelteSet<Language>();
    readonly #chosenFavoriteTags = new SvelteSet<Tag>();
    // eslint-disable-next-line no-unused-private-class-members
    readonly #chosenProfilePic = $state<string>('');

    readonly suggestedTags = $derived<() => Set<Tag>>(() => {
        const tags = new Set<string>();
        for (const lang of this.#chosenLanguages) {
            const arr = this.#allSuggestedTagsByLang[lang];
            if (arr) arr.forEach((t) => tags.add(t));
        }
        this.#suggestedForAllLangs.forEach((t) => tags.add(t));
        return tags;
    });

    constructor(initialLangs: Language[], initialTags: Tag[], initialProfilePic: string) {
        initialLangs.forEach((l) => this.#chosenLanguages.add(l));
        initialTags.forEach((t) => this.#chosenFavoriteTags.add(t));
        this.#chosenProfilePic = initialProfilePic ?? '';
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
}