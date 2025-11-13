<script lang="ts">
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { toast } from 'svelte-sonner';
	import VokiCoAuthorsListUserItem from './c_co_authors_list/VokiCoAuthorsListUserItem.svelte';
	import { getConfirmActionDialogOpenFunction } from '../../../../c_layout/ts_layout_contexts/confirm-action-dialog-context';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		isViewerPrimaryAuthor: boolean;
		viewerId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		vokiId: string;
	}
	let { viewerId, coAuthorIds, invitedForCoAuthorUserIds, isViewerPrimaryAuthor, vokiId }: Props =
		$props();
	const sortedCoAuthorIds = $derived([...coAuthorIds].sort((a, b) => a.localeCompare(b)));
	const sortedInvitedIds = $derived(
		[...invitedForCoAuthorUserIds].sort((a, b) => a.localeCompare(b))
	);
	async function cancelInvite(userId: string) {
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			coAuthorIds: string[];
			invitedForCoAuthorUserIds: string[];
		}>(`/vokis/${vokiId}/cancel-co-author-invite`, RJO.DELETE({ userId }));
		console.log(response);
		if (response.isSuccess) {
			coAuthorIds = response.data.coAuthorIds;
			invitedForCoAuthorUserIds = response.data.invitedForCoAuthorUserIds;
			toast.success('Invite cancelled');
		} else {
			toast.error("Couldn't cancel invite");
		}
	}
	const { open: openConfirmationDialog, close: closeConfirmationDialog } =
		getConfirmActionDialogOpenFunction();
	async function dropCoAuthor(userId: string) {
		openConfirmationDialog({
			mainContent: dropCoAuthorConfirm(userId),
			dialogButtons: {
				confirmBtnText: 'Drop',
				confirmBtnOnclick: () => {
					closeConfirmationDialog();
				},
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: () => {
					closeConfirmationDialog();
				}
			}
		});
	}
</script>

{#snippet cancelInviteButton(userId: string)}
	<button class="action-btn" onclick={() => cancelInvite(userId)}>Cancel invite</button>
{/snippet}
{#snippet dropCoAuthorButton(userId: string)}
	<button class="action-btn">Drop co-author</button>
{/snippet}

<div class="all-co-authors-list">
	{#each sortedCoAuthorIds as userId}
		<VokiCoAuthorsListUserItem
			{userId}
			userCoAuthorState={userId === viewerId ? 'viewerIsAuthor' : 'coAuthor'}
			actionButton={isViewerPrimaryAuthor ? dropCoAuthorButton : null}
		/>
	{/each}
	{#if coAuthorIds.length > 0 && invitedForCoAuthorUserIds.length > 0}
		<h2 class="invited-subheading">Invited co-authors({invitedForCoAuthorUserIds.length})</h2>
	{/if}
	{#each sortedInvitedIds as userId}
		<VokiCoAuthorsListUserItem
			{userId}
			userCoAuthorState="invitedUser"
			actionButton={isViewerPrimaryAuthor ? cancelInviteButton : null}
		/>
	{/each}
</div>
{#snippet dropCoAuthorConfirm(userId: string)}
	<p>
		Are you sure you want to drop <BasicUserDisplay
			{userId}
			interactionLevel={'UniqueNameGotoOnClick'}
		/>
	</p>
{/snippet}

<style>
	.all-co-authors-list {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
		width: 100%;
		margin-top: 1rem;
	}
	.action-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		border: none;
		padding: 0.375rem 0;

		border-radius: 0.5rem;
		width: 8rem;
		cursor: pointer;
	}
	.action-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
