<script lang="ts">
	import type { Snippet } from 'svelte';

	const { text, children }: { text: string; children: Snippet } = $props<{
		text: string;
		children: Snippet;
	}>();
</script>

<div class="field">
	{@render children()}
	<label>{text}</label>
</div>

<style>
    
</style>