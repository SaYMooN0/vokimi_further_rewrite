<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/string-utils';
	import ErrView from './ErrView.svelte';

	let {
		errList,
		containerId = StringUtils.rndStrWithPref('err-block-')
	}: { errList: Err[]; containerId?: string } = $props();
</script>

<div class="err-block" id={containerId}>
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
