<script lang="ts">
	import type { VokiType } from '$lib/ts/voki';
	interface Props {
		type: VokiType;
		name: string;
		isSelected: boolean;
		onclick: () => void;
	}
	let { type, name, isSelected, onclick }: Props = $props();
</script>

<div class="card" class:selected={isSelected} {onclick}>
	<svg>
		<use href="#{type}-voki-type-icon" />
	</svg>
	<p>{name}</p>
</div>

<style>
	.card {
		display: grid;
		justify-content: center;
		place-items: center center;
		width: 12rem;
		height: 10rem;
		padding: 1rem 0;
		border: 0.25rem solid transparent;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		transition:
			transform 0.15s ease,
			border-color 0.08s ease;
		cursor: pointer;
		grid-template-rows: 4rem 1fr;
	}

	.card:hover {
		transform: scale(1.07);
	}

	.card.selected {
		border-color: var(--primary);
		transform: scale(1.15);
	}

	.card > :global(svg) {
		height: 100%;
		color: var(--primary);
		stroke-width: 1.6;
	}

	.card > p {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 500;
		cursor: inherit;
	}
</style>
