<script lang="ts">
	interface Props {
		suggestions: Set<string>;
		isTagChosen: (tag: string) => boolean;
		chooseTag: (tag: string) => void;
		removeTag: (tag: string) => void;
	}

	let { suggestions, isTagChosen, chooseTag, removeTag }: Props = $props();
</script>

<div class="suggestion-list">
	{#each suggestions as tag}
		{#if isTagChosen(tag)}
			<div class="tag chosen" onclick={() => removeTag(tag)}>#{tag}</div>
		{:else}
			<div class="tag" onclick={() => chooseTag(tag)}>#{tag}</div>
		{/if}
	{/each}
</div>

<style>
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
