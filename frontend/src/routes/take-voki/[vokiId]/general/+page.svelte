<script lang="ts">
	import UnableLoadVokiToTake from '../c_pages_shared/UnableLoadVokiToTake.svelte';
	import type { PageProps } from './$types';
	import DefaultGeneralVokiTaking from './DefaultGeneralVokiTaking.svelte';
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
	console.log(data);
</script>

{#if data.response.isSuccess}
	{#if vokiTakingCase(data.response.data) === 'default'}
		<DefaultGeneralVokiTaking takingData={data.response.data} />
	{/if}
{:else}
	<UnableLoadVokiToTake
		errs={data.response.errs}
		vokiId={data.vokiId!}
		vokiTypeName={data.vokiTypeName}
	/>
{/if}
