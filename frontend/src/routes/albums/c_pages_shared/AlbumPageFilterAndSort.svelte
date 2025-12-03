<script lang="ts">
	import { VokiTypeUtils, type VokiType } from '$lib/ts/voki-type';

	interface Props {
		chosenVokiTypes: Set<VokiType>;
		onVokiTypeClick: (vokiType: VokiType) => void;
		sortOptions: Iterable<string>;
		chooseSortOption: (sortOption: string) => void;
		currentSortOption: string;
	}
	let {
		chosenVokiTypes,
		onVokiTypeClick,
		sortOptions,
		chooseSortOption,
		currentSortOption
	}: Props = $props();
	let sortOptionsContainer = $state<HTMLDivElement>()!;
	let showSortOptions = $state(false);
	function onSortOptionsContainerClick() {
		showSortOptions = !showSortOptions;
	}
</script>

<div class="container">
	<label class="main-label">Types:</label>
	<div class="voki-types unselectable">
		{#each VokiTypeUtils.all() as vokiType}
			<div
				class="type-badge"
				class:selected={chosenVokiTypes.has(vokiType)}
				onclick={() => onVokiTypeClick(vokiType)}
			>
				<svg><use href={VokiTypeUtils.icon(vokiType)} /></svg>
				{VokiTypeUtils.name(vokiType)}
			</div>
		{/each}
	</div>
	<label class="main-label sort-label">Sort:</label>

	<div class="sort-options-container unselectable" onclick={() => onSortOptionsContainerClick()}>
		{currentSortOption}
		<div class="all-options" class:show={showSortOptions}>
			{#each sortOptions as option}
				<div
					class="option"
					onclick={() => chooseSortOption(option)}
					class:selected={option === currentSortOption}
				>
					{option}
					{#if option === currentSortOption}
						<svg><use href="#common-check-icon" /></svg>
					{/if}
				</div>
			{/each}
		</div>
	</div>
</div>

<style>
	.container {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
		align-items: center;
		margin: 0.5rem 0 1rem;
	}
	.main-label {
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 500;
	}
	.voki-types {
		display: flex;
		gap: 0.5rem;
	}
	.type-badge {
		display: flex;
		align-items: center;
		gap: 0.125rem;
		padding: 0.25rem 1rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1rem;
		border-radius: 100vw;
		font-weight: 450;
		cursor: pointer;
	}
	.type-badge:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.type-badge.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
	.type-badge > svg {
		width: 1.25rem;
		height: 1.25rem;
		color: inherit;
		stroke-width: 2;
	}
	.sort-label {
		margin-left: auto;
	}
	.sort-options-container {
		display: flex;
		gap: 0.5rem;
		position: relative;
		background-color: var(--primary);
		color: var(--primary-foreground);
		padding: 0.25rem 0;
		width: 10rem;
		display: flex;
		align-items: center;
		justify-content: center;
		cursor: pointer;
		border-radius: 0.375rem;
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.125px;
	}
	.sort-options-container:hover {
		background-color: var(--primary-hov);
	}
	.sort-options-container:has(.all-options.show) {
		border-radius: 0.375rem 0.375rem 0 0;
	}

	.all-options {
		position: absolute;
		opacity: 0;
		pointer-events: none;
		top: 100%;
		left: 0;
		z-index: 100;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		width: calc(100% - 2px);
		transform: translateX(1px);
		border-radius: 0 0 0.375rem 0.375rem;
		box-shadow: var(--shadow-md), var(--shadow-xs);
	}
	.all-options.show {
		opacity: 1;
		pointer-events: all;
	}
	.option {
		padding: 0.25rem 0.25rem 0.25rem 1rem;
		cursor: pointer;
		display: grid;
		grid-template-columns: 1fr auto;
		font-size: 1.125rem;
	}
	.option:hover {
		background-color: var(--muted);
	}
	.option:active {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.option.selected {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.option > svg {
		width: 1.5rem;
		height: 1.5rem;
		color: inherit;
		stroke-width: 2;
	}
</style>
