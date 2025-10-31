<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import type { Err } from '$lib/ts/err';
	import type { UserProfilePreview } from '$lib/ts/users';
	import CoAuthorInvitedMessage from './c_inviting_dialog/CoAuthorInvitedMessage.svelte';
	import UserSearchBar from './c_inviting_dialog/UserSearchBar.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { VokiCreationAuthorsInfo } from '../types';
	interface Props {
		maxCoAuthorsCount: number;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		vokiId: string;
		updateParentCoAuthors: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}

	let {
		maxCoAuthorsCount,
		coAuthorIds,
		invitedForCoAuthorUserIds,
		vokiId,
		updateParentCoAuthors
	}: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;

	export function open() {
		dialog.open();
	}
	async function inviteUser(userId: string) {
		// const response = await ApiVokiCreationCore.fetchJsonResponse<VokiCreationAuthorsInfo>(
		// 	`/vokis/${vokiId}/invite-co-author`,
		// 	RJO.POST({ newCoAuthorId: userId })
		// );
		// if (response.isSuccess) {
		// 	updateParent(response.data);
		// 	isAlreadyInvited = true;
		// 	invitedUserId = userId;
		// } else {
		// 	errs = response.errs;
		// }
	}
	let searchBarComponent = $state<UserSearchBar>();
</script>

<DialogWithCloseButton dialogId="co-author-inviting-dialog" bind:this={dialog}>
	<UserSearchBar bind:this={searchBarComponent} />
	<p1 class="co-authors-count"
		>Co-authors (with invited) count: {coAuthorIds.length +
			invitedForCoAuthorUserIds.length}/{maxCoAuthorsCount}</p1
	>
	{#if searchBarComponent && searchBarComponent.IsInputEmpty()}
		<div class="empty-state">
			<div class="emoji">🔍</div>
			<p class="empty-state-title">Type to discover creators</p>
			<p class="empty-state-subtitle">Search by display name or @uniqueName</p>
		</div>
	{:else if searchBarComponent && searchBarComponent.SearchedUsers().length === 0}
		<div class="empty-state">
			<div class="emoji">🕵️‍♀️</div>
			<p class="empty-state-title">No matches found</p>
			<p class="empty-state-subtitle">Try another name — someone’s out there 👀</p>
		</div>
	{:else if searchBarComponent}
		<div class="users-list">
			{#each searchBarComponent.SearchedUsers() as user}
				<div class="user">
					{JSON.stringify(user)}
					<button class="invite-btn" onclick={() => inviteUser(user.userId)}>Invite</button>
				</div>
			{/each}
		</div>
	{/if}

	<label class="note"
		>Note: invited user will see the type, name, cover and other main details of this Voki</label
	>
</DialogWithCloseButton>

<style>
	:global(#co-author-inviting-dialog > .dialog-content) {
		width: 42rem;
		padding: 1.5rem 3rem;
		display: grid;
		grid-template-rows: auto auto 32rem auto;
	}
	.co-authors-count {
		color: var(--secondary-foreground);
		font-size: 1rem;
		text-align: center;
		font-weight: 500;
		margin: 0.25rem 0 0;
	}
	.note {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		margin-top: auto;
		text-align: center;
	}
	.empty-state {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		text-align: center;
		border: 0.125rem solid var(--secondary);
		border-radius: var(--voki-cover-border-radius);
		background-color: var(--back);
		box-shadow: var(--shadow);
		padding: 2rem;
		gap: 0.75rem;
		animation: var(--default-fade-in);
	}

	.empty-state-title {
		font-size: 1.375rem;
		font-weight: 600;
		color: var(--text);
	}

	.empty-state-subtitle {
		font-size: 0.9375rem;
		color: var(--muted-foreground);
		max-width: 30rem;
		line-height: 1.5;
	}
</style>
