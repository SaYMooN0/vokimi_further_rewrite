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
	{#if result.image}
		<img
			class="result-image"
			src={StorageBucketMain.fileSrc(result.image)}
			alt={`Image for result "${result.name}"`}
		/>
	{:else}
		<div class="no-image">
			<svg><use href="#no-image-icon" /></svg>
			<span>No image</span>
		</div>
	{/if}

	<div class="item-body">
		<h3 class="result-name">{result.name}</h3>
		{#if showDistribution}
			<div
				class="progress"
				aria-valuemin="0"
				aria-valuemax="100"
				aria-valuenow={Math.round(result.distributionPercent)}
			>
				<div
					class="progress-fill"
					style={`width:${Math.max(0, Math.min(100, result.distributionPercent))}%`}
				/>
			</div>
			<label class="percent-label">{Math.round(result.distributionPercent)}%</label>
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
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
		object-fit: cover;
	}

	.no-image {
		display: grid;
		height: 100%;
		padding: 0.375rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.75rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		aspect-ratio: 1/1;
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.no-image svg {
		width: 100%;
		height: 100%;
		stroke-width: 1.125;
	}

	.no-image span {
		padding: 0.25rem 0;
		font-size: 0.875rem;
		font-weight: 500;
	}

	.item-body {
		display: grid;
		gap: 0.5rem;
	}

	.result-name {
		margin: 0;
		color: var(--text);
		font-size: 1.1rem;
		font-weight: 600;
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
