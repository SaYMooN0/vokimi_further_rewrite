<script lang="ts">
	import AuthorsPageComponent from '../../../c_shared/c_shared_pages/AuthorsPageComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<AuthorsPageComponent
		vokiId={data.vokiId!}
		primaryAuthorId={data.data.primaryAuthorId}
		initCoAuthorIds={data.data.coAuthorIds}
		initInvitedForCoAuthorUserIds={data.data.invitedForCoAuthorUserIds}
		vokiCreationDate={data.data.vokiCreationDate}
		maxVokiCoAuthors={data.data.maxVokiCoAuthors}
	/>
{/if}
