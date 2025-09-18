<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import type { PageProps } from './$types';
	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<div>{JSON.stringify(data.response.data)}</div>

	<!-- {#if allowToSeeResultsList}
		<a href="/take-voki/{vokiId}/results">See results list</a>
	{:else}
		<div>Not allowed to see results list</div>
	{/if} -->
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load voki result "
		errs={data.response.errs}
		additionalParams={[
			{ name: 'vokiId', value: data.vokiId },
			{ name: 'resultId', value: data.resultId }
		]}
	/>
{/if}
