<script lang="ts">
	import CoAuthorInviteDialog from './c_invite_co_authors_message/CoAuthorInviteDialog.svelte';

	interface Props {
		vokiId: string;
		maxCoAuthors: number;
		coAuthorsWithInvitedIds: string[];
		isViewerPrimaryAuthor: boolean;
		updateCoAuthorsInfo: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}
	let {
		vokiId,
		maxCoAuthors,
		coAuthorsWithInvitedIds,
		isViewerPrimaryAuthor,
		updateCoAuthorsInfo
	}: Props = $props();
	let dialog = $state<CoAuthorInviteDialog>()!;
</script>

<CoAuthorInviteDialog bind:this={dialog} {vokiId} updateParentCoAuthors={updateCoAuthorsInfo} />
{#if coAuthorsWithInvitedIds.length <= maxCoAuthors}
	<h1 class="limit-reached">
		Co-authors limit reached.Voki cannot have more than {maxCoAuthors} co-authors
	</h1>
{:else}
<p class="places-left"></p>
 {#if isViewerPrimaryAuthor}
	<button class="invite-btn" onclick={() => dialog.open()}>Invite new co-authors</button>
{:else}

{/if}
{/if}
