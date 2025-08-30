<script lang="ts">
	import { page } from '$app/state';
	import type { Snippet } from 'svelte';
	const { children }: { children: Snippet } = $props();
</script>

<h1>Taking {page.data.vokiTypeName}<label>{page.data.vokiName}</label>voki</h1>
{@render children()}

<style>
	h1 {
		display: flex;
		justify-content: center;
		align-items: center;
		height: var(--side-bar-links-top-padding);
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 440;
	}

	h1 label {
		display: inline-block;
		max-width: calc(60vw - 4rem);
		margin: 0 0.25rem;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--primary);
		font-weight: 490;
	}
</style>
