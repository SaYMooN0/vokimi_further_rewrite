<script lang="ts">
	import type { VokiRatingValue } from '$lib/ts/voki';
	import type { RatingValueToCountType } from '../types';

	interface Props {
		distribution: RatingValueToCountType;
	}

	let { distribution }: Props = $props();

	const ORDER: VokiRatingValue[] = [1, 2, 3, 4, 5];

	let highlighted = $state<VokiRatingValue | null>(null);
	let clearTimer: number | null = null;

	function highlight(value: VokiRatingValue) {
		highlighted = value;
		if (clearTimer !== null) {
			clearTimeout(clearTimer);
		}
		clearTimer = window.setTimeout(() => {
			highlighted = null;
			clearTimer = null;
		}, 2000);
	}

	const total = $derived.by(() => {
		let sum = 0;
		for (const v of ORDER) {
			sum += distribution[v];
		}
		return sum;
	});

	function polarToCartesian(cx: number, cy: number, r: number, angleDeg: number) {
		const a = ((angleDeg - 90) * Math.PI) / 180;
		return { x: cx + r * Math.cos(a), y: cy + r * Math.sin(a) };
	}

	function describeWedge(cx: number, cy: number, r: number, startDeg: number, endDeg: number) {
		const sweep = endDeg - startDeg;
		const start = polarToCartesian(cx, cy, r, startDeg);
		const end = polarToCartesian(cx, cy, r, endDeg);
		const largeArc = sweep > 180 ? 1 : 0;

		return `M ${cx} ${cy} L ${start.x} ${start.y} A ${r} ${r} 0 ${largeArc} 1 ${end.x} ${end.y} Z`;
	}

	const slices = $derived.by(() => {
		const cx = 50;
		const cy = 50;
		const r = 46;

		let acc = -90;

		return ORDER.map((value) => {
			const count = distribution[value];
			const pct = count / total;
			const span = pct * 360;

			const start = acc;
			const end = acc + span;
			acc = end;

			return {
				value,
				count,
				path: describeWedge(cx, cy, r, start, end)
			};
		});
	});
</script>

<div class="root">
	<svg class="chart" viewBox="0 0 100 100" role="img" aria-label="Rating distribution">
		<g class="slices">
			{#each slices as s (s.value)}
				<path class={`slice slice-${s.value}`} d={s.path} onclick={() => highlight(s.value)}>
					<title>{s.value}★: {s.count}</title>
				</path>
			{/each}
		</g>
	</svg>

	<div class="legend">
		{#each slices as s (s.value)}
			<div class="legend-row" class:highlighted={highlighted === s.value}>
				<span class={`dot dot-${s.value}`} aria-hidden="true"></span>
				<span class="legend-label">{s.value} <svg><use href="#common-star-icon" /></svg></span>
				<span class="legend-value">{s.count}</span>
			</div>
		{/each}
	</div>
</div>

<style>
	.root {
		display: flex;
		align-items: center;
		gap: 2rem;
		--chart-size: 12rem;
		background-color: var(--secondary);
		border-radius: 1rem;
	}

	.chart {
		--slice-1: var(--primary-hov);
		--slice-2: color-mix(in srgb, var(--primary-hov) 80%, var(--secondary));
		--slice-3: color-mix(in srgb, var(--primary-hov) 60%, var(--secondary));
		--slice-4: color-mix(in srgb, var(--primary-hov) 40%, var(--secondary));
		--slice-5: color-mix(in srgb, var(--primary-hov) 20%, var(--secondary));

		width: var(--chart-size);
		height: var(--chart-size);
		display: block;
	}

	.slice {
		stroke-width: 0.35;
		cursor: pointer;
	}

	.slice-1 {
		fill: var(--slice-1);
	}
	.slice-2 {
		fill: var(--slice-2);
	}
	.slice-3 {
		fill: var(--slice-3);
	}
	.slice-4 {
		fill: var(--slice-4);
	}
	.slice-5 {
		fill: var(--slice-5);
	}


	.legend {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		min-width: 10rem;
	}

	.legend-row {
		display: grid;
		grid-template-columns: 0.75rem 1fr auto;
		align-items: center;
		column-gap: 0.5rem;
		padding: 0.2rem 0.4rem;
		border-radius: var(--radius);
		transition: background-color 0.15s ease-in-out;
	}

	.legend-row.highlighted {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.dot {
		width: 0.5rem;
		height: 0.5rem;
		border-radius: 999rem;
		background-color: var(--muted-foreground);
	}

	.dot-1 {
		background: var(--slice-1);
	}
	.dot-2 {
		background: var(--slice-2);
	}
	.dot-3 {
		background: var(--slice-3);
	}
	.dot-4 {
		background: var(--slice-4);
	}
	.dot-5 {
		background: var(--slice-5);
	}

	.legend-label {
		color: var(--text);
		font-size: 0.95rem;
	}

	.legend-label > svg {
		width: 0.8rem;
		height: 0.8rem;
	}
	.legend-value {
		color: var(--muted-foreground);
		font-size: 0.95rem;
		font-variant-numeric: tabular-nums;
	}
</style>
