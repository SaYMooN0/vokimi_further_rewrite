<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		headerText: string;
		rightButton: {
			icon: Snippet;
			text: string;
			onclick: () => void;
		};
	}
	let { headerText, rightButton }: Props = $props();
</script>

<div class="header-container">
	<h1>{headerText}</h1>
	<button class="right-btn" onclick={() => rightButton.onclick()}
		>{@render rightButton.icon()} {rightButton.text}</button
	>
</div>

<style>
	.header-container {
		display: grid;
		grid-template-columns: 1fr auto;
		padding: 0 2rem;
		margin: 2rem 0 0.5rem;
	}

	.right-btn {
		display: grid;
		align-items: center;
		gap: 0.25rem;
		width: 12.5rem;
		padding: 0 1rem;
		border: none;
		border-radius: 0.75rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 480;
		grid-template-columns: 1.375rem 1fr;
	}

	.right-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.right-btn > :global(svg) {
		width: 100%;
		aspect-ratio: 1/1;
		color: inherit;
		stroke-width: 2.2;
	}
</style>
