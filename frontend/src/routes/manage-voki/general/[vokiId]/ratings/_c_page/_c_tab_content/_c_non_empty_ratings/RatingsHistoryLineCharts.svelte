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

	let actualFrom = $derived.by(() => {
		if (from === null) return publicationStartOfDay;
		const d = new Date(from);
		d.setHours(0, 0, 0, 0);
		return d;
	});

	let actualTo = $derived.by(() => {
		if (to === null) return today;
		const d = new Date(to);
		d.setHours(0, 0, 0, 0);
		return d;
	});

	let fromError = $derived.by(() => {
		if (fromType === 'chosen_date' && from) {
			const selectedFrom = new Date(from);
			selectedFrom.setHours(0, 0, 0, 0);
			if (selectedFrom < publicationStartOfDay) {
				return `Cannot be before publication date (${publicationStartOfDay.toLocaleDateString()})`;
			}
			if (selectedFrom > today) {
				return 'Cannot be in the future';
			}
			if (actualTo && selectedFrom >= actualTo) {
				return 'Cannot be same or after end date';
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
			if (selectedTo < publicationStartOfDay) {
				return `Cannot be before publication date (${publicationStartOfDay.toLocaleDateString()})`;
			}
			if (actualFrom && selectedTo <= actualFrom) {
				return 'Cannot be same or before start date';
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
				<span class="filter-label">From</span>
				<div class="input-container">
					<div class="pill-toggle" role="group" aria-label="From date type">
						<button
							class="pill-btn"
							class:active={fromType === 'voki_publishing'}
							onclick={() => (fromType = 'voki_publishing')}
						>
							Since publishing
						</button>
						<button
							class="pill-btn"
							class:active={fromType === 'chosen_date'}
							onclick={() => (fromType = 'chosen_date')}
						>
							Custom date
						</button>
					</div>
					<input
						type="date"
						value={dateFormatterForNativeInput(from)}
						onchange={handleFromDateChange}
						class="custom-date-input"
						class:error={fromError}
						class:hidden-input={fromType !== 'chosen_date'}
						tabindex={fromType === 'chosen_date' ? 0 : -1}
					/>
					{#if fromError}
						<span class="error-msg">{fromError}</span>
					{/if}
				</div>
			</div>

			<div class="filter-group">
				<span class="filter-label">To</span>
				<div class="input-container">
					<div class="pill-toggle" role="group" aria-label="To date type">
						<button
							class="pill-btn"
							class:active={toType === 'today'}
							onclick={() => (toType = 'today')}
						>
							Today
						</button>
						<button
							class="pill-btn"
							class:active={toType === 'chosen_date'}
							onclick={() => (toType = 'chosen_date')}
						>
							Custom date
						</button>
					</div>
					<input
						type="date"
						value={dateFormatterForNativeInput(to)}
						onchange={handleToDateChange}
						class="custom-date-input"
						class:error={toError}
						class:hidden-input={toType !== 'chosen_date'}
						tabindex={toType === 'chosen_date' ? 0 : -1}
					/>
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
			errorMessage={fromError || toError}
		/>
		<RatingsHistoryLineChartComponent
			title="Total Ratings Count"
			data={totalData}
			color="var(--primary)"
			errorMessage={fromError || toError}
		/>
	</div>
</div>

<style>
	.line-charts-wrapper {
		display: flex;
		flex-direction: column;
		margin-top: 1.5rem;
		background: var(--back);
		border-radius: 1.25rem;
		box-shadow: var(--shadow-xs);
		overflow: hidden;
	}

	.header-section {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
		padding: 1.75rem 2rem;
	}

	.section-title {
		font-size: 1.35rem;
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

	.filter-group {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.75rem;
	}

	.filter-label {
		font-size: 0.875rem;
		color: var(--muted-foreground);
		font-weight: 600;
		text-transform: uppercase;
		letter-spacing: 0.05em;
		white-space: nowrap;
		min-width: 2.5rem;
	}

	.input-container {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
		position: relative;
		align-items: center;
	}

	.pill-toggle {
		display: flex;
		flex-direction: row;
		border: 1px solid var(--border);
		border-radius: 999px;
		overflow: hidden;
		background: color-mix(in srgb, var(--muted) 40%, var(--back));
		padding: 0.125rem;
		gap: 0.125rem;
	}

	.pill-btn {
		padding: 0.5rem 1rem;
		border: none;
		border-radius: 999px;
		background: transparent;
		color: var(--muted-foreground);
		font-family: inherit;
		font-size: 1rem;
		font-weight: 500;
		cursor: pointer;
		transition:
			background-color 0.18s ease,
			color 0.18s ease,
			box-shadow 0.18s ease;
		white-space: nowrap;
		outline: none;
	}

	.pill-btn:hover:not(.active) {
		background-color: var(--muted);
		color: var(--text);
	}

	.pill-btn.active {
		background: var(--back);
		color: var(--primary);
		font-weight: 600;
		box-shadow: var(--shadow);
	}

	.custom-date-input {
		box-sizing: border-box;
		padding: 0.5rem 0.875rem;
		border-radius: 999px;
		border: 0.125rem solid var(--muted);
		background-color: var(--back);
		color: var(--text);
		font-family: inherit;
		font-size: 0.875rem;
		font-weight: 500;
		outline: none;
		transition:
			border-color 0.18s,
			box-shadow 0.18s,
			background-color 0.18s,
			opacity 0.15s;
		cursor: pointer;
	}

	.custom-date-input.hidden-input {
		visibility: hidden;
		opacity: 0;
		pointer-events: none;
	}

	.custom-date-input.error {
		background-color: var(--red-1);
		color: var(--red-3);
		border-color: var(--red-3);
	}

	.error-msg {
		position: absolute;
		top: calc(100% + 0.25rem);
		left: 0;
		font-size: 0.875rem;
		color: var(--red-3);
		background-color: var(--red-1);
		white-space: nowrap;
		font-weight: 450;
		pointer-events: none;
		padding: 0.125rem 0.5rem;
		border-radius: 999px;
	}

	.charts-grid {
		display: flex;
		flex-direction: column;
		gap: 3.5rem;
		padding: 2.5rem;
	}
</style>
