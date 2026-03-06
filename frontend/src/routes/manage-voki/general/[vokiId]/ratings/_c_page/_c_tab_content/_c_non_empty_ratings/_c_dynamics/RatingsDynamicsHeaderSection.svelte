<script lang="ts">
	import { DateUtils } from '$lib/ts/utils/date-utils';
	import RatingsDateInput from './_c_header/RatingsDateInput.svelte';

	interface Props {
		from: Date | null;
		to: Date | null;
		today: Date;
		vokiPublicationDate: Date;
	}
	let { today, from = $bindable(), to = $bindable(), vokiPublicationDate }: Props = $props();

	let fromError = $derived.by(() => {
		if (from) {
			const selectedFrom = new Date(from);
			selectedFrom.setHours(0, 0, 0, 0);
			const pubDate = new Date(vokiPublicationDate);
			pubDate.setHours(0, 0, 0, 0);
			const todayDate = new Date(today);
			todayDate.setHours(0, 0, 0, 0);

			if (selectedFrom < pubDate) {
				return `Cannot be before publication date (${DateUtils.toLocaleDateOnly(vokiPublicationDate)})`;
			}
			if (selectedFrom > todayDate) {
				return 'Cannot be in the future';
			}
			const effectiveTo = to ? new Date(to) : new Date(today);
			effectiveTo.setHours(0, 0, 0, 0);
			if (selectedFrom >= effectiveTo) {
				return 'Cannot be same or after end date';
			}
		}
		return null;
	});

	let toError = $derived.by(() => {
		if (to) {
			const selectedTo = new Date(to);
			selectedTo.setHours(0, 0, 0, 0);
			const pubDate = new Date(vokiPublicationDate);
			pubDate.setHours(0, 0, 0, 0);
			const todayDate = new Date(today);
			todayDate.setHours(0, 0, 0, 0);

			if (selectedTo > todayDate) {
				return 'Cannot be in the future';
			}
			if (selectedTo < pubDate) {
				return `Cannot be before publication date (${DateUtils.toLocaleDateOnly(vokiPublicationDate)})`;
			}
			const effectiveFrom = from ? new Date(from) : new Date(vokiPublicationDate);
			effectiveFrom.setHours(0, 0, 0, 0);
			if (selectedTo <= effectiveFrom) {
				return 'Cannot be same or before start date';
			}
		}
		return null;
	});

	export function anyInputError() {
		return !!(fromError || toError);
	}
</script>

<div class="header-section">
	<h2 class="section-title">See voki ratings dynamics</h2>
	<div class="filters">
		<RatingsDateInput
			label="From"
			date={from}
			error={fromError}
			quickPickLabel="Voki publishing"
			onQuickPick={() => {
				from = null;
			}}
			onCustomDateSelect={() => {
				from = vokiPublicationDate;
			}}
			onDateChange={(d) => {
				from = d;
			}}
		/>

		<RatingsDateInput
			label="To"
			date={to}
			error={toError}
			quickPickLabel="Today"
			onQuickPick={() => {
				to = null;
			}}
			onCustomDateSelect={() => {
				to = today;
			}}
			onDateChange={(d) => {
				to = d;
			}}
		/>
	</div>
</div>

<style>
	.header-section {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
		padding: 1.75rem 2rem;
	}

	.section-title {
		font-size: 2rem;
		font-weight: 650;
		color: var(--text);
		margin: 0;
	}

	.filters {
		display: flex;
		flex-direction: row;
		gap: 2rem;
		align-items: flex-start;
		flex-wrap: wrap;
	}
</style>
