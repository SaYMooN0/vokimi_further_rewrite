<script lang="ts">
	import CoAuthorInviteDialog from './c_invite_co_authors_message/CoAuthorInviteDialog.svelte';
	import InviteFirstCoAuthorMessage from './c_invite_co_authors_message/InviteFirstCoAuthorMessage.svelte';
	import MoreCoAuthorsCanBeInvitedMessage from './c_invite_co_authors_message/MoreCoAuthorsCanBeInvitedMessage.svelte';

	interface Props {
		vokiId: string;
		maxCoAuthors: number;
		primaryAuthorId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		isViewerPrimaryAuthor: boolean;
		updateCoAuthorsInfo: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}
	let {
		vokiId,
		maxCoAuthors,
		primaryAuthorId,
		coAuthorIds,
		invitedForCoAuthorUserIds,
		isViewerPrimaryAuthor,
		updateCoAuthorsInfo
	}: Props = $props();
	let coAuthorsWithInvitedCount = $derived(coAuthorIds.length + invitedForCoAuthorUserIds.length);
	let dialog = $state<CoAuthorInviteDialog>()!;
</script>

<CoAuthorInviteDialog
	bind:this={dialog}
	maxCoAuthorsCount={maxCoAuthors}
	{primaryAuthorId}
	{coAuthorIds}
	{invitedForCoAuthorUserIds}
	{vokiId}
	updateParentCoAuthors={updateCoAuthorsInfo}
/>

{#if coAuthorsWithInvitedCount === 0 && isViewerPrimaryAuthor}
	<InviteFirstCoAuthorMessage openInviteDialog={() => dialog.open()} />
{:else if coAuthorsWithInvitedCount >= maxCoAuthors}
	<div class="co-authors-limit">
		<h2>Co-authors limit reached</h2>
		<p>Voki cannot have more than {maxCoAuthors} co-authors.</p>
	</div>
{:else}
	<MoreCoAuthorsCanBeInvitedMessage
		openInviteDialog={() => dialog.open()}
		{maxCoAuthors}
		{coAuthorsWithInvitedCount}
		{isViewerPrimaryAuthor}
	/>
{/if}
