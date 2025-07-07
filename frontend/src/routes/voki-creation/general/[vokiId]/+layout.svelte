<script lang="ts">
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './c_layout/GeneralVokiCreationLayoutNavBar.svelte';

	const { children }: { children: Snippet } = $props();
</script>

<GeneralVokiCreationLayoutNavBar />
{@render children()}
