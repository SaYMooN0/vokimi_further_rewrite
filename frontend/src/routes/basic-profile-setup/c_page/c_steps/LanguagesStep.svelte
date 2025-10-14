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
			class="language"
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
		display: grid;
		gap: 0.75rem;
		grid-template-columns: repeat(auto-fit, minmax(7rem, 1fr));
		justify-items: center;
	}

	.language {
		display: flex;
		align-items: center;
		justify-content: center;
		padding: 0.5rem 1rem;
		border-radius: var(--radius);
		background: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		transition: background 0.15s;
		user-select: none;
	}

	.language:not(.active):hover {
		background: var(--accent);
		color: var(--accent-foreground);
	}

	.language.active {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	svg {
		width: 1.5rem;
		aspect-ratio: 1;
	}
</style>
