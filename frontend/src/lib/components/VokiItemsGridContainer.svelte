<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<div class="vokis-container">
	{@render children()}
</div>
<style>
	.vokis-container {
		display: flex;
		display: grid;
		gap: 1rem;
		grid-template-columns: repeat(auto-fill, minmax(22rem, 1fr));
	}
</style>
