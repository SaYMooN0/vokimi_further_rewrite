<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<h1 class="results-header">{@render children()}</h1>

<style>
	.results-header {
		min-height: var(--side-bar-links-top-padding);
		padding: 0.75rem 4rem;
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 650;
		text-align: center;
		letter-spacing: 0.25px;
	}
</style>
