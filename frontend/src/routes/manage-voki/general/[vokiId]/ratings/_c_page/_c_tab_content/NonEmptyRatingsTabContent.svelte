<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import type { VokiDailyRatingsSnapshot } from '../../types';
	import RatingsDistributionPieChart from './_c_non_empty_ratings/RatingsDistributionPieChart.svelte';
	import TakeAndRetrieveNewSnapshotButton from './_c_shared/TakeAndRetrieveNewSnapshotButton.svelte';
	import type { Err } from '$lib/ts/err';
	import RatingsHistoryLineCharts from './_c_non_empty_ratings/RatingsHistoryLineCharts.svelte';

	interface Props {
		lastSnapshot: VokiDailyRatingsSnapshot;
		onTakeAndRetrieveRatingsSnapshotBtnClicked: () => void;
		snapshotsRetrievingState: { name: 'ok' } | { name: 'loading' } | { name: 'errs'; errs: Err[] };
		lineChartFilter: { from: Date | null; to: Date | null };
		snapshotsToShow: VokiDailyRatingsSnapshot[];
		vokiPublicationDate: Date;
	}
	let {
		lastSnapshot,
		onTakeAndRetrieveRatingsSnapshotBtnClicked,
		snapshotsRetrievingState,
		lineChartFilter,
		snapshotsToShow,
		vokiPublicationDate
	}: Props = $props();
	const totalSum = $derived(
		lastSnapshot.distribution[1] * 1 +
			lastSnapshot.distribution[2] * 2 +
			lastSnapshot.distribution[3] * 3 +
			lastSnapshot.distribution[4] * 4 +
			lastSnapshot.distribution[5] * 5
	);
	const totalCount = $derived(
		lastSnapshot.distribution[1] +
			lastSnapshot.distribution[2] +
			lastSnapshot.distribution[3] +
			lastSnapshot.distribution[4] +
			lastSnapshot.distribution[5]
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
			Average rating: <label class="accent-val"
				>{averageRating}<svg class="star-icon"><use href="#common-star-icon" /></svg>
			</label>
		</div>
		<div class="main-field">
			Total ratings count: <label class="accent-val">{totalCount}</label>
		</div>
		<div class="main-field">
			Last time updated: <div class="last-updated-val">
				<label use:relativeTime={{ date: lastSnapshot.date }}></label>
				<TakeAndRetrieveNewSnapshotButton
					{onTakeAndRetrieveRatingsSnapshotBtnClicked}
					{snapshotsRetrievingState}
				/>
			</div>
		</div>
	</div>
	<div class="pie-chart-part">
		<RatingsDistributionPieChart distribution={lastSnapshot.distribution} />
	</div>
</div>
<RatingsHistoryLineCharts
	bind:from={lineChartFilter.from}
	bind:to={lineChartFilter.to}
	{snapshotsToShow}
	{vokiPublicationDate}
/>

<style>
	.ratings-top-data {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 2rem;
	}

	.fields-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		height: 100%;
	}

	.main-field {
		width: 100%;
		height: fit-content;
		padding: 1.5rem 2rem;
		border-radius: 0.75rem;
		color: var(--muted-foreground);
		font-size: 2.125rem;
		font-weight: 525;
		letter-spacing: 0.125px;
		white-space: nowrap;
	}

	.main-field > .accent-val {
		display: inline-flex;
		flex-direction: row;
		align-items: center;
		color: var(--primary);
		font-weight: 600;
	}

	.main-field .star-icon {
		width: 2.675rem;
		height: 2.675rem;
		margin-bottom: 0.25rem;
		margin-left: 0.25rem;
		fill: var(--primary);
	}
</style>
