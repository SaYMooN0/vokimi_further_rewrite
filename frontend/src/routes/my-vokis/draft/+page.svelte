<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { toast } from 'svelte-sonner';
	import { MyDraftVokisCacheStore } from './my-draft-vokis-cache-store.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type {
		VokiItemViewErrStateProps,
		VokiItemViewOkStateProps
	} from '$lib/components/voki_item/c_voki_item/types';
	import { ApiVokiCreationCore } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import MyVokisPageUnexpectedStateAfterLoading from '../c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { getContext, onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../c_shared/MyVokisPageInitialLoading.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';

	function getVokiViewItemState(
		vokiId: string
	):
		| { name: 'ok'; data: VokiItemViewOkStateProps }
		| { name: 'loading' }
		| { name: 'errs'; data: VokiItemViewErrStateProps } {
		const voki = MyDraftVokisCacheStore.Get(vokiId);
		if (voki.state === 'loading') {
			return { name: 'loading' };
		} else if (voki.state === 'errs') {
			return { name: 'errs', data: { vokiId, errs: voki.errs } };
		} else {
			return {
				name: 'ok',
				data: {
					vokiId,
					voki: {
						name: voki.data.name,
						cover: voki.data.cover,
						primaryAuthorId: voki.data.primaryAuthorId,
						coAuthorIds: voki.data.coAuthorIds
					},
					onMoreBtnClick: () => toast.error("Voki more button isn't implemented yet"),
					link: `/voki-creation/${StringUtils.pascalToKebab(voki.data.type)}/${vokiId}`
				}
			};
		}
	}
	let draftVokiIds:
		| { state: 'errs'; errs: Err[] }
		| { state: 'loaded'; vokiIds: string[] }
		| { state: 'loading' } = $state({ state: 'loading' });

	async function loadDraftVokis() {
		draftVokiIds = { state: 'loading' };
		const response = await ApiVokiCreationCore.fetchJsonResponse<{ vokiIds: string[] }>(
			'/list-user-voki-ids',
			{ method: 'GET' }
		);
		if (response.isSuccess) {
			draftVokiIds = { state: 'loaded', vokiIds: response.data.vokiIds };
		} else {
			draftVokiIds = { state: 'errs', errs: response.errs };
		}
	}
	const registerPageApi = registerCurrentPageApi();
	

	async function forceRefetch() {
		MyDraftVokisCacheStore.Clear();
		await loadDraftVokis();
	}

	onMount(() => {
		registerPageApi({
			forceRefetch,
			get isLoading() {
				return draftVokiIds.state === 'loading';
			}
		});
		loadDraftVokis();
	});
</script>

{#if draftVokiIds.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your draft vokis" />
{:else if draftVokiIds.state === 'errs'}
	<DefaultErrBlock errList={draftVokiIds.errs} />
{:else if draftVokiIds.state === 'loaded'}
	<VokiItemsGridContainer>
		{#each draftVokiIds.vokiIds as vokiId}
			<VokiItemView state={getVokiViewItemState(vokiId)} />
		{/each}
	</VokiItemsGridContainer>
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={forceRefetch} />
{/if}
