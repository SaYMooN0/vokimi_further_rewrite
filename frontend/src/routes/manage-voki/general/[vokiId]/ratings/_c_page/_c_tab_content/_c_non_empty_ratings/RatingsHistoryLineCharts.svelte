<script lang="ts">
	import type { VokiDailyRatingsSnapshot } from '../../../types';
	import RatingsHistoryLineChartComponent from './_c_line_charts/RatingsHistoryLineChartComponent.svelte';

	interface Props {
		from: Date | null;
		to: Date | null;
		snapshotsToShow: VokiDailyRatingsSnapshot[];
		vokiPublicationDate: Date;
	}
	let {
		from = $bindable(),
		to = $bindable(),
		snapshotsToShow,
		vokiPublicationDate
	}: Props = $props();

	let fromType = $state<'voki_publishing' | 'chosen_date'>(
		from === null ? 'voki_publishing' : 'chosen_date'
	);
	let toType = $state<'today' | 'chosen_date'>(to === null ? 'today' : 'chosen_date');

	const today = new Date();
	today.setHours(0, 0, 0, 0);

	const publicationStartOfDay = new Date(vokiPublicationDate);
	publicationStartOfDay.setHours(0, 0, 0, 0);

	let fromError = $derived.by(() => {
		if (fromType === 'chosen_date' && from) {
			const selectedFrom = new Date(from);
			selectedFrom.setHours(0, 0, 0, 0);
			if (selectedFrom < publicationStartOfDay) {
				return `Cannot be before publication date (${publicationStartOfDay.toLocaleDateString()})`;
			}
		}
		return null;
	});

	let toError = $derived.by(() => {
		if (toType === 'chosen_date' && to) {
			const selectedTo = new Date(to);
			selectedTo.setHours(0, 0, 0, 0);
			if (selectedTo > today) {
				return 'Cannot be in the future';
			}
		}
		return null;
	});

	$effect(() => {
		if (fromType === 'voki_publishing') {
			from = null;
		} else if (from === null) {
			from = new Date();
		}
	});

	$effect(() => {
		if (toType === 'today') {
			to = null;
		} else if (to === null) {
			to = new Date();
		}
	});

	const dateFormatterForNativeInput = (date: Date | null) => {
		if (!date) return '';
		const offset = date.getTimezoneOffset();
		const localDate = new Date(date.getTime() - offset * 60 * 1000);
		return localDate.toISOString().split('T')[0];
	};

	const handleFromDateChange = (e: Event) => {
		const input = e.target as HTMLInputElement;
		if (!input.value) return;
		from = new Date(input.value);
	};

	const handleToDateChange = (e: Event) => {
		const input = e.target as HTMLInputElement;
		if (!input.value) return;
		to = new Date(input.value);
	};

	const formatter = new Intl.DateTimeFormat(undefined, {
		month: 'short',
		day: 'numeric',
		year: 'numeric'
	});

	const chartData = $derived.by(() => {
		return snapshotsToShow
			.filter((s) => {
				const sDate = new Date(s.date);
				sDate.setHours(0, 0, 0, 0);
				return sDate <= today;
			})
			.map((s) => {
				const dist = s.distribution;
				const totalSum = dist[1] * 1 + dist[2] * 2 + dist[3] * 3 + dist[4] * 4 + dist[5] * 5;
				const totalCount = dist[1] + dist[2] + dist[3] + dist[4] + dist[5];
				const average = totalCount === 0 ? 0 : Math.round((totalSum * 100) / totalCount) / 100;
				const xLabel = formatter.format(new Date(s.date));

				return {
					xLabel,
					average,
					totalCount
				};
			});
	});

	const averageData = $derived(chartData.map((d) => ({ xLabel: d.xLabel, yValue: d.average })));
	const totalData = $derived(chartData.map((d) => ({ xLabel: d.xLabel, yValue: d.totalCount })));
</script>

<div class="line-charts-wrapper">
	<div class="header-section">
		<h2 class="section-title">See voki ratings dynamics</h2>

		<div class="filters">
			<div class="filter-group">
				<span class="filter-label">From:</span>
				<div class="input-container">
					<select bind:value={fromType} class="custom-select">
						<option value="voki_publishing">Voki publishing</option>
						<option value="chosen_date">Chosen date</option>
					</select>
					{#if fromType === 'chosen_date'}
						<input
							type="date"
							value={dateFormatterForNativeInput(from)}
							onchange={handleFromDateChange}
							class="custom-date-input"
							class:error={fromError}
						/>
					{/if}
					{#if fromError}
						<span class="error-msg">{fromError}</span>
					{/if}
				</div>
			</div>

			<div class="filter-group">
				<span class="filter-label">To:</span>
				<div class="input-container">
					<select bind:value={toType} class="custom-select">
						<option value="today">Today</option>
						<option value="chosen_date">Chosen date</option>
					</select>
					{#if toType === 'chosen_date'}
						<input
							type="date"
							value={dateFormatterForNativeInput(to)}
							onchange={handleToDateChange}
							class="custom-date-input"
							class:error={toError}
						/>
					{/if}
					{#if toError}
						<span class="error-msg">{toError}</span>
					{/if}
				</div>
			</div>
		</div>
	</div>

	<div class="charts-grid">
		<RatingsHistoryLineChartComponent
			title="Average Rating"
			data={averageData}
			color="var(--primary)"
			yMax={5}
		/>
		<RatingsHistoryLineChartComponent
			title="Total Ratings Count"
			data={totalData}
			color="var(--primary)"
		/>
	</div>
</div>

<style>
	.line-charts-wrapper {
		display: flex;
		flex-direction: column;
		gap: 2rem;
		margin-top: 1rem;
	}

	.header-section {
		display: flex;
		flex-direction: column;
		gap: 1.5rem;
		background: var(--back);
		padding: 1.5rem 2rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-xs);
	}

	.section-title {
		font-size: 1.4rem;
		font-weight: 600;
		color: var(--text);
		margin: 0;
	}

	.filters {
		display: flex;
		flex-direction: row;
		gap: 2.5rem;
		align-items: center;
		flex-wrap: wrap;
	}

	.filter-group {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.75rem;
	}

	.filter-label {
		font-size: 0.95rem;
		color: var(--text);
		font-weight: 500;
	}

	.custom-select,
	.custom-date-input {
		padding: 0.6rem 1rem;
		border-radius: var(--radius);
		border: 1px solid var(--border);
		background-color: var(--back);
		color: var(--text);
		font-size: 0.95rem;
		outline: none;
		transition:
			border-color 0.2s,
			box-shadow 0.2s;
		cursor: pointer;
		box-shadow: var(--shadow-xs);
	}

	.custom-select:hover,
	.custom-date-input:hover {
		border-color: var(--primary);
	}

	.custom-select:focus,
	.custom-date-input:focus {
		border-color: var(--primary);
		box-shadow: 0 0 0 2px color-mix(in srgb, var(--primary) 20%, transparent);
	}

	.charts-grid {
		display: flex;
		flex-direction: column;
		gap: 2rem;
	}

	.input-container {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		position: relative;
	}

	.custom-date-input.error {
		border-color: var(--destructive);
	}

	.custom-date-input.error:focus {
		box-shadow: 0 0 0 2px color-mix(in srgb, var(--destructive) 20%, transparent);
	}

	.error-msg {
		position: absolute;
		top: 100%;
		left: 0;
		font-size: 0.75rem;
		color: var(--destructive);
		white-space: nowrap;
		margin-top: 0.125rem;
	}
</style>
