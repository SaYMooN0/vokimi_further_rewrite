<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import ErrView from './ErrView.svelte';

	let { errList }: { errList: Err[] } = $props();
</script>

<div class="err-block">
	{#each errList as err}
		<ErrView {err} />
	{/each}
</div>

<style>
	.err-block {
		display: flex;
		flex-direction: column;
		gap: 0.375rem;
		width: 100%;
		box-sizing: border-box;
		border-radius: 0.5rem;
	}
</style>
