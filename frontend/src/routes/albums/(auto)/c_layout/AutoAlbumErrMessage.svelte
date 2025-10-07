<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';

	interface Props {
		albumName: string;
		errs: Err[];
	}
	let { albumName, errs }: Props = $props();
</script>

<div class="msg-container">
	<h1>Could not load your {albumName} Vokis album</h1>
	<a href="/albums" class="go-back-link">Go back to albums</a>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		width: 40rem;
	}

	.go-back-link {
		padding: 0.25rem 1rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
