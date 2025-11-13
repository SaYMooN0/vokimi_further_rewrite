<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokisCatalog } from '$lib/ts/backend-communication/backend-services';
	import { ErrUtils, type Err } from '$lib/ts/err';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
	let isVokiNotFound = $derived(errs.some((err) => ErrUtils.isWithVokiNotFoundCode(err)));
	async function isVokiAlreadyPublished(): Promise<boolean> {
		const response = await ApiVokisCatalog.fetchJsonResponse<{ isPublished: boolean }>(
			`/vokis/${vokiId}/is-published`,
			{ method: 'GET' }
		);
		return response.isSuccess && response.data.isPublished;
	}
</script>

<div class="unable-to-load">
	{#if isVokiNotFound}
		{#await isVokiAlreadyPublished()}
			<div class="is-published-check-loading">Something went wrong. We make additional checks</div>
		{:then isPublished}
			{#if isPublished}
				<h1>This Voki is already published</h1>
			{:else}
				<h1>Unable to load page data</h1>
				<DefaultErrBlock errList={errs} />
			{/if}
		{/await}
	{:else}
		<h1>Unable to load page data</h1>
		<DefaultErrBlock errList={errs} />
	{/if}
</div>

<style>
	.unable-to-load {
		display: flex;
		flex-direction: column;
		justify-content: center;
		width: 60rem;
		margin: 4rem auto;
	}
	.is-published-check-loading {
		animation: fade-in-from-zero-with-delay 1s ease-in;
	}
</style>
