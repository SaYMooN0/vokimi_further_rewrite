<script lang="ts">
	import UnableLoadVokiToTake from '../c_pages_shared/UnableLoadVokiToTake.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiTaking from './GeneralVokiTaking.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<GeneralVokiTaking takingData={data.response.data} />
{:else}
	<UnableLoadVokiToTake
		errs={data.response.errs}
		vokiId={data.vokiId!}
		vokiTypeName={data.vokiTypeName}
	/>
{/if}
