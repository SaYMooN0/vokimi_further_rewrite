<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../../../c_shared/FieldNotSetLabel.svelte';

	let { relatedResultIds }: { relatedResultIds: string[] } = $props<{
		relatedResultIds: string[];
	}>();
</script>

<div class="related-results">
	<label class="related-results-label">Related results ({relatedResultIds.length})</label>
	{#if relatedResultIds.length === 0}
		<FieldNotSetLabel text="related results" className="no-related-results" />
	{:else}
		{#each relatedResultIds as result}
			<div class="result">
				{result}
			</div>
		{/each}
	{/if}
</div>

<style>
	.related-results {
		width: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.375rem;
		height: 100%;
		justify-content: center;
	}
	.related-results > :global(.no-related-results) {
		margin: 0;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 450;
	}
	.related-results-label {
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
		margin-bottom: 0.25rem;
	}
	.result {
		display: block;
		width: 100%;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 450;
	}
</style>
