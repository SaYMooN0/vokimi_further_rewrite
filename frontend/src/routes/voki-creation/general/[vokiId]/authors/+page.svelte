<script lang="ts">
	import AuthorsPageComponent from '../../../_c_shared/_c_shared_pages/AuthorsPageComponent.svelte';
	import { CoAuthorsPageState } from '../../../_c_shared/_c_shared_pages/_c_authors/co-authors-page-state.svelte';
	import type { VokiCreationAuthorsInfo } from '../../../_c_shared/_c_shared_pages/_c_authors/types';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import { GeneralVokiCreationAuthorsPageState } from './general-voki-creation-authors-page-state.svelte';
	import { setVokiCreationCurrentPageState } from '../../../voki-creation-page-context';

	let { data }: PageProps = $props();

	setVokiCreationCurrentPageState(new GeneralVokiCreationAuthorsPageState());

	function initPageState(vokiId: string, data: VokiCreationAuthorsInfo) {
		let pageState = new CoAuthorsPageState(
			vokiId,
			data.primaryAuthorId,
			data.coAuthorIds,
			data.invitedForCoAuthorUserIds,
			data.maxVokiCoAuthors,
			data.expectedManagers
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
