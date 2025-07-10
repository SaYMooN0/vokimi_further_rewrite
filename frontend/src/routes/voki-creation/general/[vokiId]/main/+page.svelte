<script lang="ts">
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import MainInfoPageComponent from '../../../c_shared/с_shared_pages/MainInfoPageComponent.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<MainInfoPageComponent mainInfo={data.data} />
{/if}
