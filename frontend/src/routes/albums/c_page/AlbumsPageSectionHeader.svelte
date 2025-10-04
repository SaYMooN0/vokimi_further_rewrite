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
	.right-button {
		width: 12rem;
	}
</style>
