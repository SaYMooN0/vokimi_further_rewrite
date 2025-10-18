<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type { Err } from '$lib/ts/err';

	interface Props {
		tagsSuggestionsState:
			| { name: 'loading' }
			| { name: 'ok'; tags: Iterable<string> }
			| { name: 'errs'; errs: Err[] };
		isTagChosen: (tag: string) => boolean;
		chooseTag: (tag: string) => void;
		removeTag: (tag: string) => void;
	}

	let { tagsSuggestionsState, isTagChosen, chooseTag, removeTag }: Props = $props();
</script>

{#if tagsSuggestionsState.name === 'loading'}
	<div class="suggestions-not-loaded">
		<CubesLoader sizeRem={2} />
		<h1>Loading suggestions</h1>
	</div>
{:else if tagsSuggestionsState.name === 'errs'}
	<div class="suggestions-not-loaded">
		<h1>Couldn't load tag suggestions</h1>
		<DefaultErrBlock errList={tagsSuggestionsState.errs} />
	</div>
{:else if tagsSuggestionsState.name === 'ok'}
	<div class="suggestion-list">
		{#each tagsSuggestionsState.tags as tag}
			{#if isTagChosen(tag)}
				<div class="tag chosen" onclick={() => removeTag(tag)}>#{tag}</div>
			{:else}
				<div class="tag" onclick={() => chooseTag(tag)}>#{tag}</div>
			{/if}
		{/each}
	</div>
{:else}{/if}

<style>
	.suggestions-not-loaded {
		display: flex;
		justify-content: start;
		padding-top: 2rem;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
	}
	.suggestions-not-loaded h1 {
		font-size: 1.25rem;
		font-weight: 500;
		color: var(--secondary-foreground);
	}
	
	.suggestion-list {
		display: flex;
		justify-content: center;
		column-gap: 1rem;
		row-gap: 0.75rem;
		flex-wrap: wrap;
		align-items: center;
		height: fit-content;
	}
	.tag {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		display: grid;
		grid-template-columns: 1fr auto;
		padding: 0.125rem 0.675rem;
		border-radius: 1rem;
		font-weight: 450;
		gap: 0.125rem;
		cursor: pointer;
		letter-spacing: 0.1px;
		font-size: 1rem;
		box-shadow: var(--shadow-xs);
	}
	.tag:not(.chosen):hover {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}
	.tag.chosen {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: none;
	}
</style>
