<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView errs={data.response.errs} />
{:else}
	{JSON.stringify(data.response)}
{/if}
