<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokisCatalog } from '$lib/ts/backend-communication/backend-services';
	import { ErrUtils, type Err } from '$lib/ts/err';
	import type { VokiType } from '$lib/ts/voki-type';
	import VokiAlreadyPublishedMessage from './c_page_loading_err/VokiAlreadyPublishedMessage.svelte';
	import { onMount } from 'svelte';

	interface Props {
		errs: Err[];
		vokiId: string;
	}
	let { errs, vokiId }: Props = $props();
	let componentState: 'additional-checks' | 'voki-published' | 'unable-to-load' =
		$state('unable-to-load');
	onMount(() => {
		const isVokiNotFound = errs.some((err) => ErrUtils.isWithVokiNotFoundCode(err));
		if (isVokiNotFound) {
			componentState = 'additional-checks';
			ApiVokisCatalog.fetchJsonResponse<{ vokiType: VokiType }>(`/vokis/${vokiId}/does-exist`, {
				method: 'GET'
			})
				.then(
					(response) => (componentState = response.isSuccess ? 'voki-published' : 'unable-to-load')
				)
				.catch(() => (componentState = 'unable-to-load'));
		} else {
			componentState = 'unable-to-load';
		}
	});
</script>

<div class="container">
	{#if componentState === 'voki-published'}
		<VokiAlreadyPublishedMessage {vokiId} />
	{:else if componentState === 'additional-checks'}
		<div class="is-published-check-loading">Something went wrong. We make additional checks</div>
	{:else if componentState === 'unable-to-load'}
		<h1>Unable to load page data</h1>
		<DefaultErrBlock errList={errs} />
	{:else}
		<div>Something went wrong</div>
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
