<script lang="ts">
	import CoAuthorInviteDialog from './c_invite_co_authors_message/CoAuthorInviteDialog.svelte';
	import InviteFirstCoAuthorMessage from './c_invite_co_authors_message/InviteFirstCoAuthorMessage.svelte';
	import InviteMoreCoAuthorsMessage from './c_invite_co_authors_message/InviteMoreCoAuthorsMessage.svelte';

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
{:else if isViewerPrimaryAuthor}
	<InviteMoreCoAuthorsMessage
		openInviteDialog={() => dialog.open()}
		{maxCoAuthors}
		{coAuthorsWithInvitedCount}
	/>
{:else}
	<div class="locked-info">
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M4.27 18.84C4.49 20.52 5.88 21.82 7.56 21.9C8.98 21.97 10.42 22 12 22C13.58 22 15.02 21.97 16.44 21.9C18.12 21.82 19.51 20.52 19.73 18.84C19.88 17.75 20 16.64 20 15.5C20 14.36 19.88 13.25 19.73 12.15C19.51 10.49 18.12 9.18 16.44 9.1C15.02 9.03 13.58 9 12 9C10.42 9 8.98 9.03 7.56 9.1C5.88 9.18 4.49 10.49 4.27 12.15C4.12 13.25 4 14.36 4 15.5C4 16.64 4.12 17.75 4.27 18.84Z"
				stroke="currentColor"
			/>
			<path
				d="M7.5 9V6.5C7.5 4.01 9.51 2 12 2C14.49 2 16.5 4.01 16.5 6.5V9"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
			<path
				d="M12 15.5H12.01"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
		</svg>
		<p>Only the primary author can invite co-authors</p>
	</div>
{/if}

<style>

	.locked-info {
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 0.5rem;
		color: var(--muted-foreground);
	}

	.locked-info svg {
		height: 1.25rem;
		width: 1.25rem;
		stroke-width: 1.5;
		color: var(--muted-foreground);
	}
</style>
