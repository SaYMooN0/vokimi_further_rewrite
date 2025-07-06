<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { getVokiBriefInfo } from '../my-vokis-cache-store.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if data.errs}
	<DefaultErrBlock errList={data.errs} />
{:else}
	<div>
		{#each data.draftVokiIds as vokiId}
			<div>
				<h1>{vokiId}</h1>
				{#await getVokiBriefInfo(vokiId)}
					<h3>Loading...</h3>
				{:then data} 
					<h3>{JSON.stringify(data)}</h3>
				{/await}
			</div>
		{/each}
	</div>
{/if}

<style>
	
</style>
