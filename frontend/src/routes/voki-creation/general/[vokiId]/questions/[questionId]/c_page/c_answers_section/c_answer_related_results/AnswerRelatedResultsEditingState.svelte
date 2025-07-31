<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../c_shared/FieldNotSetLabel.svelte';

	const {
		resultNames,
		openResultSelectingDialog
	}: { resultNames: string[]; openResultSelectingDialog: () => void } = $props<{
		resultNames: string[];
		openResultChoosingDialog: () => void;
	}>();
	const maxResultsCount = 10;
</script>

<div class="results">
	{#if resultNames.length > 0}
		<FieldNotSetLabel text="No related results" />
	{:else}
		{#each resultNames as result}
			<div class="result">
				{result}
			</div>
		{/each}
	{/if}
	{#if resultNames.length < maxResultsCount}
		<button class="add-btn" onclick={openResultSelectingDialog}>
			<svg><use href="#common-plus-icon" /></svg>
			related results
		</button>
	{/if}
</div>

<style>
	.result {
		text-overflow: ellipsis;
		overflow: hidden;
		display: flex;
	}
</style>
