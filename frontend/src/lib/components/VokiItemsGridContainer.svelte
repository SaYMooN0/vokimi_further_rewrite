<script lang="ts">
	import type { Snippet } from 'svelte';

	let { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<div class="vokis-container">
	{@render children()}
</div>

<style>
	.vokis-container {
		display: grid;
		gap: 0.5rem;
		width: 100%;
		padding: 0.5rem 0;
		grid-template-columns: repeat(auto-fill, minmax(20rem, 1fr));
	}

	@media (width >= 1536px) and (width <= 1919px){
		.vokis-container {
			grid-template-columns: repeat(auto-fill, minmax(20rem, 1fr));
		}
	}

	@media  (width <= 1535px) {
			.vokis-container {
			grid-template-columns: repeat(auto-fill, minmax(17rem, 1fr));
		}
	}
</style>
