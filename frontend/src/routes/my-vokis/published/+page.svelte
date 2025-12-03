<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import { onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../c_shared/MyVokisPageInitialLoading.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';
	import { MyPublishedVokisPageState } from './my-published-vokis-page-state.svelte';
	import { toast } from 'svelte-sonner';
	import BaseContextMenu from '$lib/components/context_menus/BaseContextMenu.svelte';

	const pageState = new MyPublishedVokisPageState();

	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => pageState.forceRefetch(),
			get isLoading() {
				return pageState.publishedVokiIds.state === 'loading';
			}
		});
	});
	function openContextMenu(mEvent: MouseEvent, vokiId: string): void {
		if (vokiItemContextMenu) {
			vokiItemContextMenu.open(mEvent.x, mEvent.y, vokiId);
		} else {
			toast.error('Failed to open context menu');
		}
	}
</script>

{#if pageState.publishedVokiIds.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your published vokis" />
{:else if pageState.publishedVokiIds.state === 'errs'}
	<DefaultErrBlock errList={pageState.publishedVokiIds.errs} />
{:else if pageState.publishedVokiIds.state === 'loaded'}
	{#if pageState.publishedVokiIds.vokiIds.length === 0}
		<h1>You don't have any published vokis</h1>
	{:else}
		
		<VokiItemsGridContainer>
			{#each pageState.publishedVokiIds.vokiIds as vokiId}
				<VokiItemView
					state={pageState.getVokiViewItemState(vokiId, (ev) => openContextMenu(ev, vokiId))}
				/>
			{/each}
		</VokiItemsGridContainer>
	{/if}
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}
