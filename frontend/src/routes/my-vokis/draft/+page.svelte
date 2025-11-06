<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../c_shared/MyVokisPageInitialLoading.svelte';
	import { MyDraftVokisPageState } from './my-draft-vokis-page-state.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';

	const pageState = new MyDraftVokisPageState();
	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => pageState.forceRefetch(),
			get isLoading() {
				return pageState.draftVokiIds.state === 'loading';
			}
		});
	});
</script>

{#if pageState.draftVokiIds.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your draft vokis" />
{:else if pageState.draftVokiIds.state === 'errs'}
	<DefaultErrBlock errList={pageState.draftVokiIds.errs} />
{:else if pageState.draftVokiIds.state === 'loaded'}
	<VokiItemsGridContainer>
		{#each pageState.draftVokiIds.vokiIds as vokiId}
			<VokiItemView state={pageState.getVokiViewItemState(vokiId)} />
		{/each}
	</VokiItemsGridContainer>
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}
