<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<h1 class="results-header">{@render children()}</h1>

<style>
	.results-header {
		padding: 0.75rem 4rem;
		margin: 0;
		font-size: 1.75rem;
		font-weight: 650;
		color: var(--secondary-foreground);
		min-height: var(--side-bar-links-top-padding);
		letter-spacing: 0.25px;
		text-align: center;
	}
</style>
