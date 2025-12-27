<script lang="ts">
	import type { PageProps } from './$types';
	import RatingsDistributionPieChart from './_c_page/RatingsDistributionPieChart.svelte';

	let { data }: PageProps = $props();
	const roundedAverage = $derived.by(() => Math.round(data.averageRating * 100) / 100);
	const totalRatings = $derived.by(
		() =>
			data.valueToCountDistribution[1] +
			data.valueToCountDistribution[2] +
			data.valueToCountDistribution[3] +
			data.valueToCountDistribution[4] +
			data.valueToCountDistribution[5]
	);
</script>

<div class="main-ratings-data">
	<div class="fields-part">
		<div class="main-field">
			Average rating: <label
				>{roundedAverage}<svg class="star-icon"><use href="#common-star-icon" /></svg>
			</label>
		</div>
		<div class="main-field">Total ratings count: <label>{totalRatings}</label></div>
	</div>
	<div class="chart-part">
		<RatingsDistributionPieChart distribution={data.valueToCountDistribution} />
	</div>
</div>

<style>
	.main-ratings-data {
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
