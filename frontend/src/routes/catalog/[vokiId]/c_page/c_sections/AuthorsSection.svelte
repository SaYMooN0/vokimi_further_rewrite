<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	interface Props {
		primaryAuthorId: string;
		coAuthorIds: string[];
	}
	let { primaryAuthorId, coAuthorIds }: Props = $props();
</script>

<div class="participants-section">
	<label class="by-label">by: </label>
	<BasicUserDisplay userId={primaryAuthorId} interactionLevel="WholeComponentLink" />
	{#if coAuthorIds.length > 0}
		<svg class="plus-icon"><use href="#common-plus-icon" /></svg>
		{#each coAuthorIds as coAuthorId}
			<BasicUserDisplay userId={coAuthorId} interactionLevel="WholeComponentLink" />
		{/each}
	{/if}
</div>

<style>
	.by-label {
		margin-right: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		font-style: italic;
	}

	.participants-section {
		display: flex;
		flex-direction: row;
		align-items: center;
		flex-wrap: wrap;
		gap: 0.375rem;
	}

	.plus-icon {
		margin: 0 0.25rem;
		color: var(--secondary-foreground);
		stroke-width: 3.25;
		height: 1rem;
		width: 1rem;
		padding: 0.125rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
		border-radius: 99vw;
		display: flex;
		align-items: center;
		justify-content: center;
	}
</style>
