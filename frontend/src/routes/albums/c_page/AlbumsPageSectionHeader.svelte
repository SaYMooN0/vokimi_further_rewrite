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
		padding: 0 1rem;
		width: 12.5rem;
		display: grid;
		grid-template-columns: 1.375rem 1fr;
		align-items: center;
		background-color: var(--muted);
		border: none;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 480;
		gap: 0.25rem;
		border-radius: 0.75rem;
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
