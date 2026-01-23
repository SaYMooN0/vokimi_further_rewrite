<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		results: Snippet;
		answerContent: Snippet;
	}
	let { results, answerContent }: Props = $props();
</script>

<div class="answer">
	{@render results?.()}
	<div class="sep" />
	{@render answerContent?.()}
</div>

<style>
	.sep {
		width: 1px;
		height: calc(100%);
		margin: 0 0.5rem;
		background-color: var(--muted);
		align-self: center;
	}
	.answer {
		--results-width: 13rem;

		display: grid;
		grid-template-columns: var(--results-width) auto 1fr;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border-radius: 0.75rem;
		box-shadow: rgb(0 0 0 / 5%) 0 0 0 1px;
	}
</style>
