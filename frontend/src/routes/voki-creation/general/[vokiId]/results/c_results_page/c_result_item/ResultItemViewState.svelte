<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { ResultOverViewData } from '../../types';
	import ResultItemButtons from './ResultItemButtons.svelte';

	let {
		result,
		refetchOnDelete,
		startEditing
	}: { result: ResultOverViewData; refetchOnDelete: () => void; startEditing: () => void } =
		$props<{ result: ResultOverViewData; refetchOnDelete: () => void; startEditing: () => void }>();
	async function deleteResult() {}
</script>

<div class="result-item-name">{result.name}</div>
<div class="result-content">
	<p class="result-text">{result.text}</p>
	{#if result.image}
		<img class="result-image" src={StorageBucketMain.fileSrc(result.image)} alt="result" />
	{/if}
</div>
<ResultItemButtons
	mainBtnText="Edit"
	mainBtnOnClick={() => startEditing()}
	secondaryBtnIconId="#common-trash-can-icon"
	secondaryBtnOnClick={() => deleteResult()}
/>

<style>
	.result-item-name {
		font-size: 1.675rem;
		font-weight: 475;
		letter-spacing: 0.5px;
		text-indent: 0.5em;
		word-break: break-all;
	}
	.result-content {
		display: flex;
		flex-direction: row;
	}
	.result-text {
		word-break: break-all;
		font-size: 1.25rem;
		font-weight: 420;
		letter-spacing: 0.1px;
	}
	.result-image {
		max-width: 15rem;
		max-height: 40rem;
		object-fit: cover;
	}
</style>
