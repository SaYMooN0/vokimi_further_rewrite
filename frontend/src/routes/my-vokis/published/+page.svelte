<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import { toast } from 'svelte-sonner';
	import { MyPublishedVokisCacheStore } from './my-published-vokis-cache-store.svelte';
	import type { PageProps } from './$types';
	import type {
		VokiItemViewErrStateProps,
		VokiItemViewOkStateProps
	} from '$lib/components/voki_item/c_voki_item/types';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';

	let { data }: PageProps = $props();

	function getVokiViewItemState(
		vokiId: string
	):
		| { name: 'ok'; data: VokiItemViewOkStateProps }
		| { name: 'loading' }
		| { name: 'err'; data: VokiItemViewErrStateProps } {
		const voki = MyPublishedVokisCacheStore.Get(vokiId);
		if (voki.state.name === 'loading') {
			return { name: 'loading' };
		} else if (voki.state.name === 'err') {
			return { name: 'err', data: { vokiId, errs: voki.state.errs } };
		} else {
			return {
				name: 'ok',
				data: {
					vokiId,
					voki: {
						name: voki.state.data.name,
						cover: voki.state.data.cover,
						primaryAuthorId: voki.state.data.primaryAuthorId,
						coAuthorIds: voki.state.data.coAuthorIds
					},
					onMoreBtnClick: () => toast.error("Voki more button isn't implemented yet"),
					link: `/catalog/${vokiId}`
				}
			};
		}
	}
</script>

{#if !data.response.isSuccess}
	<DefaultErrBlock errList={data.response.errs} />
{:else if data.response.data.vokiIds.length === 0}
	<h1>You don't have any published vokis</h1>
{:else}
	<VokiItemsGridContainer>
		{#each data.response.data.vokiIds as vokiId}
			<VokiItemView state={getVokiViewItemState(vokiId)} />
		{/each}
	</VokiItemsGridContainer>
{/if}
