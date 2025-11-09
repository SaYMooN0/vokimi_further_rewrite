<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	let { primaryAuthorId, coAuthorIds }: { primaryAuthorId: string; coAuthorIds: string[] } =
		$props<{ primaryAuthorId: string; coAuthorIds: string[] }>();
</script>

<div class="authors-section">
	<label class="by-label">by: </label>
	<BasicUserDisplay userId={primaryAuthorId} isLink={true} />
	{#if coAuthorIds.length > 0}
		and
		<div class="co-authors-container">
			{#each coAuthorIds as coAuthorId}
				<div class="co-author">
					<a href="/authors/{coAuthorId}">{coAuthorId}</a>
				</div>
			{/each}
		</div>
	{/if}
</div>

<style>
	.by-label {
		margin-right: 0.5rem;
		font-size: 1rem;
		font-weight: 450;
	}

	.authors-section {
		display: flex;
		flex-direction: row;
		align-items: center;
	}
</style>
