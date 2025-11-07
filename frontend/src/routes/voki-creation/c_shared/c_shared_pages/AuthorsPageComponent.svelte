<script lang="ts">
	import PrimaryAuthorDisplay from './c_authors/PrimaryAuthorDisplay.svelte';
	import CoAuthorsList from './c_authors/CoAuthorsList.svelte';
	import InviteCoAuthorsMessage from './c_authors/InviteCoAuthorsMessage.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	interface Props {
		primaryAuthorId: string;
		initCoAuthorIds: string[];
		initInvitedForCoAuthorUserIds: string[];
		vokiCreationDate: Date;
		maxVokiCoAuthors: number;
		vokiId: string;
	}
	let {
		primaryAuthorId,
		initCoAuthorIds,
		initInvitedForCoAuthorUserIds,
		vokiCreationDate,
		maxVokiCoAuthors,
		vokiId
	}: Props = $props();
	let coAuthorIds = $state<string[]>(initCoAuthorIds);
	let invitedForCoAuthorUserIds = $state<string[]>(initInvitedForCoAuthorUserIds);
	function updateCoAuthorsInfo(newCoAuthorIds: string[], newInvitedIds: string[]) {
		coAuthorIds = newCoAuthorIds;
		invitedForCoAuthorUserIds = newInvitedIds;
	}
</script>

<AuthView>
	{#snippet children(authState)}
		{#if authState.name === 'authenticated'}
			<div class="authors-tab-container">
				<PrimaryAuthorDisplay
					viewerId={authState.userId}
					{primaryAuthorId}
					creationDate={vokiCreationDate}
				/>
				<CoAuthorsList
					viewerId={authState.userId}
					{coAuthorIds}
					{invitedForCoAuthorUserIds}
				/>
				<InviteCoAuthorsMessage
					{vokiId}
					maxCoAuthors={maxVokiCoAuthors}
					{primaryAuthorId}
					{coAuthorIds}
					{invitedForCoAuthorUserIds}
					isViewerPrimaryAuthor={primaryAuthorId === authState.userId}
					{updateCoAuthorsInfo}
				/>
			</div>
		{:else if authState.name === 'loading'}
			<div>Loading...</div>
		{:else}
			<h3>Something went wrong please reload the page</h3>
		{/if}
	{/snippet}
</AuthView>

<style>
	.authors-tab-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		padding-top: 1rem;
	}
</style>
