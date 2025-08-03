<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import type { Snippet } from 'svelte';

	interface Props {
		children: Snippet;
		errs: Err[];
		mainBtnText: string;
		mainBtnOnClick: () => void;
		secondaryBtnIconId: string;
		secondaryBtnOnClick: () => void;
	}
	let {
		children,
		errs,
		mainBtnText,
		mainBtnOnClick,
		secondaryBtnIconId,
		secondaryBtnOnClick
	}: Props = $props();
</script>

<div class="answer-content-with-actions">
	{@render children()}
	{#if errs.length > 0}
		<DefaultErrBlock errList={errs} />
	{/if}
	<div class="buttons-container">
		<button class="main-btn" onclick={() => mainBtnOnClick()}>{mainBtnText}</button>
		<button class="secondary-btn" onclick={() => secondaryBtnOnClick()}>
			<svg><use href={secondaryBtnIconId} /></svg>
		</button>
	</div>
</div>

<style>
    
	.answer-content-with-actions {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}

	.buttons-container {
		display: flex;
		flex-direction: row;
		justify-content: flex-end;
		margin-top: auto;
		gap: 0.5rem;
		width: 100%;
	}

	.buttons-container > * {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		box-shadow: var(--shadow);
		outline: none;
		cursor: pointer;
	}

	.main-btn {
		padding: 0 1.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 480;
		letter-spacing: 1.5px;
	}

	.main-btn:hover {
		background-color: var(--primary-hov);
	}

	.secondary-btn {
		width: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		stroke-width: 2.1;
	}

	.secondary-btn:hover {
		background-color: var(--err-foreground);
		color: var(--primary-foreground);
		stroke-width: 1.8;
	}

	.secondary-btn svg {
		width: 1.5rem;
		height: 1.5rem;
	}
</style>
