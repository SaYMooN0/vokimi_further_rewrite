<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import { toast } from 'svelte-sonner';

	let { tags, vokiId }: { tags: string[]; vokiId: string } = $props<{
		tags: string[];
		vokiId: string;
	}>();
</script>

<div class="voki-tags-section">
	{#if tags.length === 0}
		<FieldNotSetLabel text="This voki has no tags" />
	{:else}
		{#each tags as tag}
			<label class="tag">#{tag}</label>
		{/each}
	{/if}
	<button
		class="suggest-tag-btn"
		onclick={() => toast.error('Tags suggestion is not implemented yet')}>Suggest a tag</button
	>
</div>

<style>
	.voki-tags-section {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		width: 100%;
		row-gap: 0.375rem;
	}

	.tag {
		padding: 0.125rem 0.375rem;
		margin-left: 0.375rem;
		border-radius: 0.25rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		box-shadow: var(--shadow-xs);
	}
	.suggest-tag-btn {
		margin-left: 0.375rem;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
