<script lang="ts">
	import type { RatingValueToCountType } from '../types';
	import RatingsDistributionPieChart from './_c_content/RatingsDistributionPieChart.svelte';

	interface Props {
		distribution: RatingValueToCountType;
	}
	let { distribution }: Props = $props();
	const totalSum = $derived(
		distribution[1] * 1 +
			distribution[2] * 2 +
			distribution[3] * 3 +
			distribution[4] * 4 +
			distribution[5] * 5
	);
	const totalCount = $derived(
		distribution[1] + distribution[2] + distribution[3] + distribution[4] + distribution[5]
	);
	const averageRating = $derived.by(() => {
		if (totalCount === 0) {
			return 0;
		}
		return Math.round((totalSum * 100) / totalCount) / 100;
	});
</script>

<div class="ratings-top-data">
	<div class="fields-part">
		<div class="main-field">
			Average rating: <label
				>{averageRating}<svg class="star-icon"><use href="#common-star-icon" /></svg>
			</label>
		</div>
		<div class="main-field">Total ratings count: <label>{totalCount}</label></div>
	</div>
	<div class="pie-chart-part">
		<RatingsDistributionPieChart {distribution} />
	</div>
</div>


<style>
	.ratings-top-data {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 2rem;
	}
	.fields-part {
		height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		flex-direction: column;
	}
	.main-field {
		width: 100%;
		height: fit-content;
		padding: 2rem 2rem;
		border-radius: 0.75rem;
		color: var(--muted-foreground);
		font-weight: 475;
		font-size: 2.5rem;
		letter-spacing: 0.25px;
		white-space: nowrap;
	}
	.main-field > label {
		color: var(--primary);
		font-weight: 500;
		display: inline-flex;
		flex-direction: row;
		align-items: center;
	}
	.main-field .star-icon {
		width: 2.5rem;
		height: 2.5rem;
		fill: var(--primary);
		margin-left: 0.25rem;
		margin-bottom: 0.25rem;
	}
</style>
