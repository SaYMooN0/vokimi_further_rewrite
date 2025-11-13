<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	let { primaryAuthorId, coAuthorIds }: { primaryAuthorId: string; coAuthorIds: string[] } =
		$props<{ primaryAuthorId: string; coAuthorIds: string[] }>();
</script>

<div class="authors-section">
	<label class="by-label">by: </label>
	<BasicUserDisplay userId={primaryAuthorId} interactionLevel="WholeComponentLink" />
	{#if coAuthorIds.length > 0}
		<label class="plus-label">+</label>
		<div class="co-authors-container">
			{#each coAuthorIds as coAuthorId}
				<BasicUserDisplay userId={coAuthorId} interactionLevel="WholeComponentLink" />
			{/each}
		</div>
	{/if}
</div>

<style>
	.by-label {
		margin-right: 0.5rem;
		font-size: 1rem;
		font-weight: 450;
		color: var(--secondary-foreground);

	}

	.authors-section {
		display: flex;
		flex-direction: row;
		align-items: center;
	}

	.plus-label {
		margin: 0 0.25rem;
		font-size: 1rem;
		font-weight: 450;
		color: var(--secondary-foreground);
	}
</style>
