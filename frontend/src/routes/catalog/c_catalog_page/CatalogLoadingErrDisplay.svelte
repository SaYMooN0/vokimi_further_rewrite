<script lang="ts">
	import DefaultErrBlock from "$lib/components/errs/DefaultErrBlock.svelte";
	import type { Err } from "$lib/ts/err";

	let { errs }: { errs: Err[] } = $props<{ errs: Err[] }>();
</script>

<div class="container">
	<h1>Could not load catalog</h1>
	<DefaultErrBlock errList={errs} />

</div>

<style>
    .container {
        display: flex;
        flex-direction: column;
        justify-content: center;
        margin: 8rem auto;
    }
</style>


