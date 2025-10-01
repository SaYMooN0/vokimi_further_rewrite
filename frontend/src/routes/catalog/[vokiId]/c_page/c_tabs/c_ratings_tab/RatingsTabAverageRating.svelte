<script lang="ts">
	import VokiPageTabSectionLabel from '../c_tabs_shared/VokiPageTabSectionLabel.svelte';

	interface Props {
		averageRating: number;
		count: number;
		isOutdated: boolean;
	}
	let { averageRating, count, isOutdated }: Props = $props();
</script>

<div class="avg-container">
	<VokiPageTabSectionLabel fieldName="Average rating:" />
	<div class="value-container">
		{Math.round(averageRating * 100) / 100}<svg class="star"><use href="#common-star-icon" /></svg
		>/5
	</div>
	{#if isOutdated}
		<button>refresh</button>
	{/if}
	<VokiPageTabSectionLabel
		fieldName="({count} total rating{count === 1 ? '' : 's'})"
		className="ratings-count"
	/>
</div>

<style>
	.avg-container {
		display: flex;
		flex-direction: row;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 500;
		cursor: default;
	}

	.value-container {
		display: flex;
		align-items: center;
		margin: 0 0.5rem;
	}

	.star {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 0;
		fill: var(--primary);
		margin-left: 0.125rem;
		margin-bottom: 0.125rem;
	}

	.avg-container > :global(.ratings-count) {
		margin-left: auto;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}
</style>
