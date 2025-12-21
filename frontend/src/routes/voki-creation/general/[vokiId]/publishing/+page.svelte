<script lang="ts">
	import PublishingPageComponent from '../../../c_shared/c_shared_pages/PublishingPageComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	let { data }: PageProps = $props();
	console.log(data);
</script>

{#if !data.response.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId} errs={data.response.errs} />
{:else}
	<PublishingPageComponent
		vokiId={data.vokiId}
		primaryAuthorId={data.response.data.primaryAuthorId}
		coAuthorIds={data.response.data.coAuthorIds}
		initialIssues={data.response.data.issues}
	/>
{/if}
