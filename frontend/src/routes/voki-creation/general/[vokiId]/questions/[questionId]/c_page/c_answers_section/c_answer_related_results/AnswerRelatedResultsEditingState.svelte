<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../c_shared/FieldNotSetLabel.svelte';
	import type { ResultIdWithName } from '../../../../types';

	let {
		results,
		openRelatedResultsSelectingDialog
	}: {
		results: ResultIdWithName[];
		openRelatedResultsSelectingDialog: () => void;
	} = $props<{
		results: ResultIdWithName[];
		openRelatedResultsSelectingDialog: () => void;
	}>();
	const maxResultsCount = 10;
</script>

<div class="results">
	{#if results.length === 0}
		<FieldNotSetLabel text="No related results" className="no-results" />
	{:else}
		{#each results as result}
			<div class="result">
				{result}
			</div>
		{/each}
	{/if}
	{#if results.length < maxResultsCount}
		<button class="add-btn" onclick={() => openRelatedResultsSelectingDialog()}>
			<svg><use href="#common-plus-icon" /></svg>
			related results
		</button>
	{/if}
</div>

<style>
	.results {
		width: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.375rem;
	}
	.results:has(:global(.no-results)) {
		align-content: center;
		height: 100%;
		justify-content: center;
	}
	.results > :global(.no-results) {
		margin: 0;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 450;
	}

	.result {
		text-overflow: ellipsis;
		overflow: hidden;
		display: flex;
	}
	.add-btn {
		margin-top: 0.25rem;
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		background-color: var(--primary);
		border: none;
		padding: 0.125rem 1rem;
		border-radius: 4rem;
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 400;
		cursor: pointer;
		width: fit-content;
		align-self: center;
	}
	.add-btn > svg {
		height: 1.25rem;
		width: 1.25rem;
		stroke-width: 2;
	}
</style>
