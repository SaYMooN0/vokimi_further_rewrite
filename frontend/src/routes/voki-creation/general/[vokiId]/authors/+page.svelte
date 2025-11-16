<script lang="ts">
	import AuthorsPageComponent from '../../../c_shared/c_shared_pages/AuthorsPageComponent.svelte';
	import { CoAuthorsPageState } from '../../../c_shared/c_shared_pages/c_authors/co-authors-page-state.svelte';
	import type { VokiCreationAuthorsInfo } from '../../../c_shared/c_shared_pages/c_authors/types';
	import VokiCreationPageLoadingErr from '../../../c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();

	function initPageState(vokiId: string, data: VokiCreationAuthorsInfo) {
		let pageState = new CoAuthorsPageState(
			vokiId,
			data.primaryAuthorId,
			data.coAuthorIds,
			data.invitedForCoAuthorUserIds,
			data.maxVokiCoAuthors
		);
		return pageState;
	}
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<AuthorsPageComponent
		pageState={initPageState(data.vokiId!, data.data)}
		vokiCreationDate={data.data.vokiCreationDate}
	/>
{/if}
