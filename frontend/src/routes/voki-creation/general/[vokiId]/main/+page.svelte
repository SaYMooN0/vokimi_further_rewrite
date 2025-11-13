<script lang="ts">
	import MainInfoPageComponent from '../../../c_shared/c_shared_pages/MainInfoPageComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<MainInfoPageComponent mainInfo={data.data} vokiId={data.vokiId!} />
{/if}
