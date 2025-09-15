<script lang="ts">
	import { browser } from '$app/environment';
	import { VokiCatalogVisitMarkerCookie } from '$lib/ts/cookies/voki-catalog-visit-marker-cookie';
	import { onMount, onDestroy } from 'svelte';
	import UnableLoadVokiToTake from '../c_pages_shared/UnableLoadVokiToTake.svelte';
	import type { PageProps } from './$types';
	import DefaultGeneralVokiTaking from './DefaultGeneralVokiTaking.svelte';
	import SequentialAnsweringGeneralVokiTaking from './SequentialAnsweringGeneralVokiTaking.svelte';
	import type { GeneralVokiTakingData } from './types';

	let { data }: PageProps = $props();
	function vokiTakingCase(
		vokiTakingData: GeneralVokiTakingData
	): 'default' | 'sequentialAnswering' {
		if (vokiTakingData.forceSequentialAnswering) {
			return 'sequentialAnswering';
		}

		return 'default';
	}

	let refreshTimer: number | undefined;
	onMount(() => {
		if (browser) {
			VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);

			refreshTimer = window.setInterval(() => {
				VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);
			}, 60_000);
		}
	});
	function clearRefreshTimer() {
		if (refreshTimer) {
			clearInterval(refreshTimer);
		}
	}
	onDestroy(clearRefreshTimer);
</script>

{#if data.response.isSuccess}
	{#if vokiTakingCase(data.response.data) === 'default'}
		<DefaultGeneralVokiTaking
			takingData={data.response.data}
			clearVokiSeenUpdateTimer={clearRefreshTimer}
		/>
	{:else if vokiTakingCase(data.response.data) === 'sequentialAnswering'}
		<SequentialAnsweringGeneralVokiTaking
			takingData={data.response.data}
			clearVokiSeenUpdateTimer={clearRefreshTimer}
		/>
	{/if}
{:else}
	<UnableLoadVokiToTake
		errs={data.response.errs}
		vokiId={data.vokiId!}
		vokiTypeName={data.vokiTypeName}
	/>
{/if}
