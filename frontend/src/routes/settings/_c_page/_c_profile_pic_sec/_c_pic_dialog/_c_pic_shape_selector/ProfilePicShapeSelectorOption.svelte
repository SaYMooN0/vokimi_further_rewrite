<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		onclick: () => void;
		name: string;
		children: Snippet;
		isActive: boolean;
	}
	let { onclick, name, children: icon, isActive }: Props = $props();
</script>

<div class="option" {onclick} class:active={isActive}>
	<div class="example">
		{@render icon()}
	</div>
	<label>{name}</label>
</div>

<style>
	.option {
		border-radius: 1rem;
		display: flex;
		justify-content: center;
		align-items: center;
		flex-direction: column;
		border: 0.125rem solid var(--secondary);
		box-shadow: var(--shadow);
		padding: 0.5rem 1.25rem;
		gap: 0.5rem;
		cursor: pointer;
		transition:
			transform 0.1s ease-in-out,
			border-color 0.1s ease-in-out;
	}
	.option:not(.active):hover {
		transform: scale(1.02);
		border-color: var(--muted);
		box-shadow: var(--shadow), var(--shadow-md);
	}
	.option.active {
		border-color: var(--primary);
		transform: scale(1.06);
		box-shadow: none;
	}
	.option > label {
		font-size: 1rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		transition: all 0.1s ease-in-out;
		cursor: inherit;
	}
	.example {
		height: 4rem;
		aspect-ratio: 1;
		padding: 0.375rem;
	}
	.option:not(.active):hover > .example {
		transform: scale(1.06);
	}
</style>
