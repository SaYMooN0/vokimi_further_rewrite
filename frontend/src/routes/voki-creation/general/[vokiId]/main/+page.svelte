<script lang="ts">
	import MainInfoPageComponent from '../../../c_shared/c_shared_pages/MainInfoPageComponent.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<MainInfoPageComponent mainInfo={data.data} vokiId={data.vokiId!} />
{/if}
