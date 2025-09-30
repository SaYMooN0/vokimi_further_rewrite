<script lang="ts">
	interface Props {
		averageRating: number;
		count: number;
	}
	let { averageRating, count }: Props = $props();
</script>

<div class="avg-container">
	Average rating:
	<div class="value-container">
		{Math.round(averageRating * 100) / 100}<svg class="star"><use href="#common-star-icon" /></svg
		>/5
	</div>
	<span class="ratings-count">
		({count} total rating{count === 1 ? '' : 's'})
	</span>
</div>

<style>
	.avg-container {
		display: flex;
		align-items: center;
		flex-direction: row;
		font-size: 1.25rem;
		font-weight: 450;
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
		margin: 0 0.125rem 0.125rem 0.25rem;
	}

	.ratings-count {
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		margin-left: auto;
		margin-right: 0.5rem;
	}
</style>
