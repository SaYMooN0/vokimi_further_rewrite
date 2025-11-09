<script lang="ts">
	import VokiCoAuthorsListUserItem from './c_co_authors_list/VokiCoAuthorsListUserItem.svelte';

	interface Props {
		viewerId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
	}
	let { viewerId, coAuthorIds, invitedForCoAuthorUserIds }: Props = $props();
	const sortedCoAuthorIds = $derived([...coAuthorIds].sort((a, b) => a.localeCompare(b)));
	const sortedInvitedIds = $derived(
		[...invitedForCoAuthorUserIds].sort((a, b) => a.localeCompare(b))
	);
</script>

{#snippet cancelInviteButton(userId: string)}
	<button class="action-btn">Cancel invite</button>
{/snippet}
{#snippet dropCoAuthorButton(userId: string)}
	<button class="action-btn">Drop co-author</button>
{/snippet}

<div class="all-co-authors-list">
	{#each sortedCoAuthorIds as userId}
		{#if userId === viewerId}
			<VokiCoAuthorsListUserItem {userId} userCoAuthorState="viewerIsAuthor" actionButton={null} />
		{:else}
			<VokiCoAuthorsListUserItem
				{userId}
				userCoAuthorState="coAuthor"
				actionButton={dropCoAuthorButton}
			/>
		{/if}
	{/each}
	{#if coAuthorIds.length > 0 && invitedForCoAuthorUserIds.length > 0}
		<h2 class="invited-subheading">Invited co-authors({invitedForCoAuthorUserIds.length})</h2>
	{/if}
	{#each sortedInvitedIds as userId}
		<VokiCoAuthorsListUserItem
			{userId}
			userCoAuthorState="invitedUser"
			actionButton={cancelInviteButton}
		/>
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
</style>
