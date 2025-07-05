<script lang="ts">
	import { StringUtils } from '$lib/ts/string-utils';
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
		<use href="#{StringUtils.pascalToKebab(type)}-voki-type-icon" />
	</svg>
	<p>{name}</p>
	<div class="indicator" class:selected={isSelected}>
		<span />
	</div>
</div>

<style>
	.card {
		display: grid;
		justify-content: center;
		place-items: center center;
		width: 10rem;
		height: 10rem;
		padding: 0.5rem 0;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		transition: all 0.16s ease;
		cursor: pointer;
		grid-template-rows: auto 1fr auto;
		box-shadow: var(--shadow);
	}
	.card:hover {
		transform: scale(1.06);
	}

	.card > :global(svg) {
		margin-top: 1rem;
		height: 3.5rem;
		aspect-ratio: 1/1;
		color: var(--primary);
		stroke-width: 1.7;
	}

	.card > p {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 500;
		cursor: inherit;
	}
	.indicator {
		height: 1.5rem;
		border: 0.1875rem solid var(--muted);
		border-radius: 1.5rem;
		aspect-ratio: 1/1;
		background-color: var(--muted);
		display: flex;
		justify-content: center;
		align-items: center;
		transition: box-shadow 0.04s ease-in;
	}
	.card:hover .indicator {
		box-shadow: var(--shadow);
	}
	.indicator.selected {
		border-color: var(--primary);
	}
	.indicator span {
		height: 70%;
		display: block;
		aspect-ratio: 1/1;
	}
	.indicator.selected span {
		background-color: var(--primary);
		border-radius: 1.5rem;
	}
</style>
