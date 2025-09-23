<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiResultWithDistributionPercent } from '../types';

	interface Props {
		result: VokiResultWithDistributionPercent;
		showDistribution: boolean;
	}
	let { result, showDistribution }: Props = $props();
</script>

<div class="result-item">
	{#if result.Image}
		<img
			class="result-image"
			src={StorageBucketMain.fileSrc(result.Image)}
			alt={`Image for result "${result.Name}"`}
		/>
	{:else}
		<div class="no-image">
			<svg><use href="#no-image-icon" /></svg>
			<span>No image</span>
		</div>
	{/if}

	<div class="item-body">
		<h3 class="result-name">{result.Name}</h3>
		{#if showDistribution}
			<div
				class="progress"
				aria-valuemin="0"
				aria-valuemax="100"
				aria-valuenow={Math.round(result.DistributionPercent)}
			>
				<div
					class="progress-fill"
					style={`width:${Math.max(0, Math.min(100, result.DistributionPercent))}%`}
				/>
			</div>
			<label class="percent-label">{Math.round(result.DistributionPercent)}%</label>
		{:else}
			<div class="distribution-hidden">Author decided to hide results distribution</div>
		{/if}
	</div>
</div>

<style>
	.result-item {
		display: grid;
		grid-template-columns: 5rem 1fr;
		align-items: center;
		gap: 1rem;
		padding: 1rem;
		border-radius: calc(var(--radius) + 0.25rem);
		box-shadow: var(--shadow-md);
	}

	.result-image {
		width: 4.5rem;
		height: 4.5rem;
		border-radius: 0.5rem;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
		background: var(--secondary);
	}
	.no-image {
		height: 100%;
		aspect-ratio: 1/1;
		display: grid;
		grid-template-rows: 1fr auto;
		color: var(--secondary-foreground);
		justify-items: center;
		background-color: var(--secondary);
		padding: 0.375rem;
		border-radius: 0.75rem;
		border: 0.125rem solid var(--secondary-foreground);
	}
	.no-image svg {
		width: 100%;
		height: 100%;
        stroke-width: 1.125;
	}
	.no-image span {
		font-size: 0.875rem;
		font-weight: 500;
        padding: 0.25rem 0;
	}
	.item-body {
		display: grid;
		gap: 0.5rem;
	}

	.result-name {
		margin: 0;
		font-size: 1.1rem;
		font-weight: 600;
		color: var(--text);
	}

	.progress {
		position: relative;
		width: 100%;
		height: 0.8rem;
		border-radius: 999rem;
		background: var(--secondary);
		box-shadow: inset var(--shadow-xs);
		overflow: hidden;
	}

	.progress-fill {
		height: 100%;
		border-radius: inherit;
		background: linear-gradient(
			90deg,
			color-mix(in srgb, var(--primary) 70%, var(--back) 30%),
			var(--primary)
		);

		box-shadow: var(--shadow);
		transition: width 220ms ease;
	}

	.percent-label {
		display: inline-block;
		min-width: 3rem;
		padding: 0.15rem 0.5rem;
		border-radius: 0.5rem;
		background: var(--accent);
		color: var(--accent-foreground);
		font-size: 0.95rem;
		font-weight: 600;
		text-align: center;
		box-shadow: var(--shadow);
	}
</style>
