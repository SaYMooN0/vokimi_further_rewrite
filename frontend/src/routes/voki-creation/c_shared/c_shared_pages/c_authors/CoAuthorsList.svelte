<script lang="ts">
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { toast } from 'svelte-sonner';
	import VokiCoAuthorsListUserItem from './c_co_authors_list/VokiCoAuthorsListUserItem.svelte';
	import DropCoAuthorConfirmationDialog from './c_co_authors_list/DropCoAuthorConfirmationDialog.svelte';

	interface Props {
		isViewerPrimaryAuthor: boolean;
		viewerId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		vokiId: string;
		updateParentCoAuthors: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}
	let {
		viewerId,
		coAuthorIds,
		invitedForCoAuthorUserIds,
		isViewerPrimaryAuthor,
		vokiId,
		updateParentCoAuthors
	}: Props = $props();

	async function cancelInvite(userId: string) {
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			coAuthorIds: string[];
			invitedForCoAuthorUserIds: string[];
		}>(`/vokis/${vokiId}/cancel-co-author-invite`, RJO.DELETE({ userId }));
		if (response.isSuccess) {
			updateParentCoAuthors(response.data.coAuthorIds, response.data.invitedForCoAuthorUserIds);
			toast.success('Invite cancelled');
		} else {
			toast.error("Couldn't cancel invite");
		}
	}
	let dropCoAuthorDialog = $state<DropCoAuthorConfirmationDialog>()!;
</script>

{#snippet cancelInviteButton(userId: string)}
	<button class="action-btn" onclick={() => cancelInvite(userId)}>Cancel invite</button>
{/snippet}
{#snippet dropCoAuthorButton(userId: string)}
	<button class="action-btn" onclick={() => dropCoAuthorDialog.open(userId)}>Drop co-author</button>
{/snippet}

<DropCoAuthorConfirmationDialog
	bind:this={dropCoAuthorDialog}
	{vokiId}
	onSuccessfulDrop={updateParentCoAuthors}
/>
<div class="all-co-authors-list">
	{#each [...coAuthorIds].sort((a, b) => a.localeCompare(b)) as userId}
		{#key userId}
			<VokiCoAuthorsListUserItem
				{userId}
				userCoAuthorState={userId === viewerId ? 'viewerIsAuthor' : 'coAuthor'}
				actionButton={isViewerPrimaryAuthor ? dropCoAuthorButton : null}
			/>
		{/key}
	{/each}
	{#if coAuthorIds.length > 0 && invitedForCoAuthorUserIds.length > 0}
		<h2 class="invited-subheading">Invited co-authors({invitedForCoAuthorUserIds.length})</h2>
	{/if}

	{#each [...invitedForCoAuthorUserIds].sort((a, b) => a.localeCompare(b)) as userId}
		{#key userId}
			<VokiCoAuthorsListUserItem
				{userId}
				userCoAuthorState="invitedUser"
				actionButton={isViewerPrimaryAuthor ? cancelInviteButton : null}
			/>
		{/key}
	{/each}
</div>

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
