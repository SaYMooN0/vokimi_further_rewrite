<script lang="ts">
	import { browser } from '$app/environment';
	import { VokiCatalogVisitMarkerCookie } from '$lib/ts/cookies/voki-catalog-visit-marker-cookie';
	import { onMount, onDestroy } from 'svelte';
	import type { PageProps } from './$types';
	import DefaultGeneralVokiTaking from './DefaultGeneralVokiTaking.svelte';
	import SequentialAnsweringGeneralVokiTaking from './SequentialAnsweringGeneralVokiTaking.svelte';
	import type { GeneralVokiTakingData } from './types';
	import { goto } from '$app/navigation';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiTakingHeader from '../_c_types_shared/VokiTakingHeader.svelte';

	let { data }: PageProps = $props();
	console.log(data);
	function vokiTakingCase(
		vokiTakingData: GeneralVokiTakingData
	): 'default' | 'sequentialAnswering' {
		if (vokiTakingData.forceSequentialAnswering) {
			return 'sequentialAnswering';
		}

		return 'default';
	}
	function onResultReceived(receivedResultId: string) {
		goto(`/take-voki/${data.vokiId}/general/results/${receivedResultId}`, { replaceState: true });
	}
	let refreshTimer: number | undefined;
	function clearMarkerCookie() {
		if (refreshTimer) {
			clearInterval(refreshTimer);
		}
		VokiCatalogVisitMarkerCookie.clear(data.vokiId);
	}
	onMount(() => {
		if (browser) {
			VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);

			refreshTimer = window.setInterval(() => {
				VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);
			}, 60_000);
		}
	});
	onDestroy(clearMarkerCookie);
</script>

{#if data.response.isSuccess}
	<VokiTakingHeader vokiType={'General'} vokiName={data.response.data.vokiName} />

	{#if vokiTakingCase(data.response.data) === 'default'}
		<DefaultGeneralVokiTaking
			takingData={data.response.data}
			clearVokiSeenUpdateTimer={clearMarkerCookie}
			{onResultReceived}
		/>
	{:else if vokiTakingCase(data.response.data) === 'sequentialAnswering'}
		<SequentialAnsweringGeneralVokiTaking
			takingData={data.response.data}
			clearVokiSeenUpdateTimer={clearMarkerCookie}
			{onResultReceived}
		/>
	{/if}
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load requested general voki"
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}
