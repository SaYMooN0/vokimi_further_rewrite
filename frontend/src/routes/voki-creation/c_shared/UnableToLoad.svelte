<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';

	let { errs }: { errs: Err[] } = $props<{ errs: Err[] }>();
</script>

<div class="unable-to-load">
	<h1>Unable to load page data</h1>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.unable-to-load {
		display: flex;
		width: 60rem;
		flex-direction: column;
		justify-content: center;
		margin: 4rem auto;
	}
</style>
