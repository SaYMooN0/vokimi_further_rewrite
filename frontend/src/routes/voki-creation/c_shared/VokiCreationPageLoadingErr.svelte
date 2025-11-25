<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokisCatalog } from '$lib/ts/backend-communication/backend-services';
	import { ErrUtils, type Err } from '$lib/ts/err';
	import type { VokiType } from '$lib/ts/voki-type';
	import VokiAlreadyPublishedMessage from './c_page_loading_err/VokiAlreadyPublishedMessage.svelte';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
	let isVokiNotFound = $derived(errs.some((err) => ErrUtils.isWithVokiNotFoundCode(err)));
	async function isVokiAlreadyPublished(): Promise<boolean> {
		const response = await ApiVokisCatalog.fetchJsonResponse<{ vokiType: VokiType }>(
			`/vokis/${vokiId}/does-exist`,
			{ method: 'GET' }
		);
		return response.isSuccess;
	}
</script>

<div class="container">
	{#if isVokiNotFound}
		{#await isVokiAlreadyPublished()}
			<div class="is-published-check-loading">Something went wrong. We make additional checks</div>
		{:then isPublished}
			{#if isPublished}
				<VokiAlreadyPublishedMessage {vokiId} />
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
	.container {
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
