<script lang="ts">
	import type { PageProps } from './$types';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';
	import type { VokiItemViewOkStateProps } from '$lib/components/voki_item/c_voki_item/types';
	import BaseContextMenu from '$lib/components/BaseContextMenu.svelte';

	let { data }: PageProps = $props();
	let contextMenu = $state<BaseContextMenu>()!;
	function assembleVokiItemStateData(voki: PublishedVokiBriefInfo): VokiItemViewOkStateProps {
		return {
			vokiId: voki.id,
			voki: {
				name: voki.name,
				cover: voki.cover,
				primaryAuthorId: voki.primaryAuthorId,
				coAuthorIds: voki.coAuthorIds
			},
			onMoreBtnClick: (mEvent: MouseEvent) => contextMenu.open(mEvent.x, mEvent.y, voki.id),
			link: `/catalog/${voki.id}`,
			flags: {
				language: voki.language,
				hasMatureContent: voki.hasMatureContent,
				authenticatedOnlyTaking: voki.signedInOnlyTaking
			}
		};
	}
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load vokis catalog" />
{:else if data.data.vokis.length === 0}
	<h1>Voki catalog is empty</h1>
{:else}
	<BaseContextMenu bind:this={contextMenu}>
		<div>Content</div>
	</BaseContextMenu>
	<VokiItemsGridContainer>
		{#each data.data.vokis as voki}
			<VokiItemView
				state={{
					name: 'ok',
					data: assembleVokiItemStateData(voki)
				}}
			/>
		{/each}
	</VokiItemsGridContainer>
{/if}
