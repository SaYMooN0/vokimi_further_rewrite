<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { LanguageUtils } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';

	export function open() {
		dialog.open();
	}
	let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="voki-flags-info-dialog" subheading="Voki flags">
	<div class="flags-container">
		<div class="flag-info">
			<div class="single-icon-container">
				<svg class="flag-icon"><use href="#common-auth-only-taking-icon" /></svg>
			</div>
			<label
				><span class="flag-name">Authenticated only taking</span> Voki flag means that this Voki author
				made so the it can only be taken by signed in users</label
			>
		</div>
		<div class="columns-sep"></div>
		<div class="flag-info">
			<div class="single-icon-container">
				<svg class="flag-icon"><use href="#common-mature-content-icon" /></svg>
			</div>
			<label
				><span class="flag-name">Mature content</span> Voki flag means that either the author or Vokimi
				has marked this voki as containing content that is not suitable for children</label
			>
		</div>
		<div class="columns-sep"></div>
		<div class="flag-info">
			<div class="language-icons">
				{#each LanguageUtils.values() as lang}
					<div class="flag-icon">
						<svg><use href="#languages-icons-{StringUtils.pascalToKebab(lang)}" /></svg>
					</div>
				{/each}
			</div>
			<label><span class="flag-name">Language</span> Voki flag shows the language of the voki</label
			>
		</div>
	</div>
</DialogWithCloseButton>

<style>
	.flags-container {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr;
		gap: 1rem;
	}
	.columns-sep {
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--secondary);
	}
	.flag-info {
		display: grid;
		grid-template-rows: 8rem auto;
		width: 16rem;
		justify-items: center;
	}
	.single-icon-container {
		height: 5rem;
		align-self: center;
	}
	.single-icon-container > svg {
		height: 100%;
		width: 100%;
		color: var(--accent-foreground);
		stroke-width: 1.6;
	}
	.flag-icon {
		align-self: center;
		padding: 0.25rem;
		box-sizing: border-box;
		background-color: var(--back);
		box-shadow: var(--shadow), var(--shadow-xs);
		border-radius: 24%;
		aspect-ratio: 1/1;
        display: flex;
        justify-content: center;
        align-items: center;
	}
	.flag-info > label {
		text-align: justify;
		text-indent: 0.5em;
		font-size: 1.125rem;
		padding: 1rem 0;
	}
	.flag-name {
		font-weight: 500;
	}

	.language-icons {
		display: flex;
		gap: 0.75rem;
		flex-direction: row;
		flex-wrap: wrap;
		justify-content: center;
		align-content: center;
	}
	.language-icons svg {
		height: 2rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.375rem;
	}
	svg:has(use[href='#languages-icons-other']) {
		stroke-width: 1.4;
	}
</style>
