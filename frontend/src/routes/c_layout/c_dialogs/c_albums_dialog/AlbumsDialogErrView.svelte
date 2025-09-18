<script lang="ts">
	import type { Err } from '$lib/ts/err';

	let { err }: { err: Err | null } = $props<{ err: Err | null }>();
</script>

<div class="err-view">
	<h1>Something went wrong</h1>
	<p>{err?.message ?? 'Unexpected error'}</p>
</div>

<style>
	.err-view {
		width: 100%;
		height: 100%;
	}
</style>
