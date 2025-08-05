<script lang="ts">
	import type { ResultOverViewData } from '../types';
	import ResultItemEditingState from './c_result_item/ResultItemEditingState.svelte';
	import ResultItemViewState from './c_result_item/ResultItemViewState.svelte';

	let {
		result,
		vokiId,
		refetchOnDelete,
		updateParentOnSave
	}: {
		result: ResultOverViewData;
		vokiId: string;
		refetchOnDelete: () => void;
		updateParentOnSave: (result: ResultOverViewData) => void;
	} = $props<{
		result: ResultOverViewData;
		vokiId: string;
		refetchOnDelete: () => void;
		updateParentOnSave: (result: ResultOverViewData) => void;
	}>();
	let isEditing = $state(false);
</script>

<div class="result" class:editing={isEditing}>
	{#if isEditing}
		<ResultItemEditingState
			{result}
			{vokiId}
			{updateParentOnSave}
			cancelEditing={() => (isEditing = false)}
		/>
	{:else}
		<ResultItemViewState {result} {refetchOnDelete} startEditing={() => (isEditing = true)} />
	{/if}
</div>

<style>
	.result {
		padding: 0.25rem 0.5rem;
		border: 0.125rem solid var(--muted);
		border-radius: 0.75rem;
		gap: 0.25rem;
        display: flex;
        flex-direction: column;
	}
	.result.editing {
		border: 0.125rem dashed var(--muted);
		gap: 0.5rem;
        
	}
</style>
