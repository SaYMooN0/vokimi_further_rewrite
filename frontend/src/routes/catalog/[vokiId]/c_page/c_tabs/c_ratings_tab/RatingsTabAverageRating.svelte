<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import VokiPageTabSectionLabel from '../c_tabs_shared/VokiPageTabSectionLabel.svelte';

	interface Props {
		averageRating: number;
		count: number;
		isOutdated: boolean;
		reloadOutdated: () => Promise<void>;
	}
	let { averageRating, count, isOutdated, reloadOutdated }: Props = $props();
	async function onReloadButtonClick() {
		loading = true;
		await reloadOutdated();
		loading = false;
	}
	let loading = $state(false);
</script>

<div class="avg-container">
	<VokiPageTabSectionLabel fieldName="Average rating:" />
	<div class="value-container">
		{Math.round(averageRating * 100) / 100}<svg class="star"><use href="#common-star-icon" /></svg
		>/5
	</div>
	{#if isOutdated}
		<svg
			class="reload-btn"
			onclick={() => {
				onReloadButtonClick();
			}}><use href="#common-reload-icon" /></svg
		>
	{/if}
	<VokiPageTabSectionLabel
		fieldName="({count} total rating{count === 1 ? '' : 's'})"
		className="ratings-count"
	/>
	{#if loading}
		<div class="loader-backdrop">
			<LinesLoader sizeRem={1.6} strokePx={2} />
		</div>
	{/if}
</div>

<style>
	.avg-container {
		display: flex;
		flex-direction: row;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 500;
		cursor: default;
		position: relative;
		z-index: 1;
	}

	.value-container {
		display: flex;
		align-items: center;
		margin: 0 0.5rem;
	}
	.reload-btn {
		margin-left: 1rem;
		border-radius: 1rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		width: 1.5rem;
		height: 1.5rem;
		transition: all 0.2s ease-out;
		stroke-width: 2.23;
		transform: rotate(60deg);
		padding: 0.25rem;
	}
	.reload-btn:hover {
		transform: rotate(120deg);
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.reload-btn:active {
		transform: scale(0.96);
		transform: rotate(360deg);
	}
	.star {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 0;
		fill: var(--primary);
		margin-left: 0.125rem;
		margin-bottom: 0.125rem;
	}

	.avg-container > :global(.ratings-count) {
		margin-left: auto;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}
	.loader-backdrop {
		position: absolute;
		width: calc(100% + 1rem);
		height: calc(100% + 0.5rem);
		top: 50%;
		left: 50%;
		z-index: 2;
		background-color: var(--secondary);
		display: flex;
		justify-content: center;
		align-items: center;
		box-shadow: var(--shadow-xs);
		border-radius: 0.5rem;
		transform: translate(-50%, -50%);
		animation: loader-fade-in-from 1s ease;
		opacity: 0.7;
	}
	.loader-backdrop > :global(.container) {
		--loader-color: var(--text);
		opacity: inherit;
	}
	@keyframes loader-fade-in-from {
		0%,
		40% {
			opacity: 0;
		}
		100% {
			opacity: 0.7;
		}
	}
</style>
