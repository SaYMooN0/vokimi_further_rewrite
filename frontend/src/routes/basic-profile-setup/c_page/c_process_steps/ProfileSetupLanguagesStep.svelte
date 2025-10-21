<script lang="ts">
	import { LanguageUtils, type Language } from '$lib/ts/language';

	interface Props {
		isLanguageChosen: (language: Language) => boolean;
		toggleLanguage: (language: Language) => void;
	}
	let { isLanguageChosen, toggleLanguage }: Props = $props();
</script>

<div class="languages-container">
	{#each LanguageUtils.values() as lang}
		<div
			class="language unselectable"
			class:active={isLanguageChosen(lang)}
			onclick={() => toggleLanguage(lang)}
		>
			<svg><use href={LanguageUtils.icon(lang)} /></svg>			
			{LanguageUtils.name(lang)}
		</div>
	{/each}
</div>

<style>
	.languages-container {
		justify-self: center;
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		width: 50rem;
		gap: 4rem;
		row-gap: 1.5rem;
		grid-template-columns: repeat(auto-fit, minmax(7rem, 1fr));
		justify-items: center;
	}

	.language {
		width: 11rem;
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 0.5rem;
		align-items: center;
		justify-content: center;
		padding: 0.5rem 1rem;
		border-radius: 0.375rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		transition: background-color 0.15s;
		font-weight: 500;
		font-size: 1.125rem;
	}

	.language:not(.active):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.language.active {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
	.language > svg {
		height: 1.5rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.375rem;
		stroke-width: 1.9;
	}
</style>
