<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<div>{JSON.stringify(data.response.data)}</div>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load all received voki results "
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}
