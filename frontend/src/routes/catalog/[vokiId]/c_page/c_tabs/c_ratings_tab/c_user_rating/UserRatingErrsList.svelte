<script lang="ts">
	import type { Err } from '$lib/ts/err';

	interface Props {
		errs: Err[];
		clearErrs: () => void;
	}
	let { errs, clearErrs }: Props = $props();
</script>

<div class="errs-list">
	<svg class="close-btn" onclick={() => clearErrs()}><use href="#common-cross-icon" /></svg>
	{#each errs as err}
		<div class="err-item">{err.message}</div>
	{/each}
</div>

<style>
	.errs-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		position: relative;
		width: 100%;
		padding: 0.5rem 0.75rem;
		background-color: var(--err-back);
		border-radius: 0.675rem;
        box-shadow: var(--err-shadow);
	}

	.close-btn {
		position: absolute;
		top: 0.25rem;
		right: 0.25rem;
		width: fit-content;
		height: 1.25rem;
		border-radius: 0.75rem;
		cursor: pointer;
		padding: 0.125rem;
		background-color: var(--back);
		color: var(--secondary-foreground);
		transition: all 0.04s ease-in;
		aspect-ratio: 1/1;
		stroke-width: 2.2;
	}
	.close-btn:hover {
		background-color: var(--secondary);
		color: var(--muted-foreground);
		border-radius: 0.5rem;
	}
	.err-item {
		color: var(--err-foreground);
		font-size: 1rem;
		font-weight: 500;
	}
</style>
