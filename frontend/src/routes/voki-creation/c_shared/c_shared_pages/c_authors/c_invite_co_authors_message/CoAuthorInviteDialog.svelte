<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { UserProfilePreview } from '$lib/ts/users';
	import UserSearchBar from './c_inviting_dialog/UserSearchBar.svelte';
	import SearchedUsersListDisplay from './c_inviting_dialog/SearchedUsersListDisplay.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { UserPreviewWithInvitesSettings } from '../types';
	import { SvelteSet } from 'svelte/reactivity';
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
	let searchedUsers = $state<UserPreviewWithInvitesSettings[]>([]);
	let searchBarInputVal = $state('');
	export function open() {
		dialog.open();
	}
	let usersChosenToInvite = new SvelteSet<UserPreviewWithInvitesSettings>([]);
	function isUserInListToInvite(userId: string) {
		return [...usersChosenToInvite].some((u) => u.id === userId);
	}
</script>

<DialogWithCloseButton dialogId="co-author-inviting-dialog" bind:this={dialog}>
	<UserSearchBar bind:searchBarInputVal bind:searchedUsers />
	<p1 class="co-authors-count"
		>Co-authors (with invited) count: {coAuthorIds.length +
			invitedForCoAuthorUserIds.length}/{maxCoAuthorsCount}
		{searchedUsers.length}</p1
	>
	<SearchedUsersListDisplay
		isInputEmpty={StringUtils.isNullOrWhiteSpace(searchBarInputVal)}
		userOptions={searchedUsers}
		isUserCoAuthor={coAuthorIds.includes}
		isUserAlreadyInvited={invitedForCoAuthorUserIds.includes}
		{isUserInListToInvite}
	/>
	<div class="confirm-btn-container">
		{#if usersChosenToInvite.size > 0}
			<label>Choose users to invite</label>
		{:else}
			<button>Invite {usersChosenToInvite.size} users for co-author</button>
			<label
				>{Array.from(usersChosenToInvite)
					.map((u) => `@${u.uniqueName}`)
					.join(', ')} will be invited</label
			>
		{/if}
	</div>
	<label class="note"
		>Note: invited user will see the type, name, cover and other main details of this Voki</label
	>
</DialogWithCloseButton>

<style>
	:global(#co-author-inviting-dialog > .dialog-content) {
		display: grid;
		width: 42rem;
		padding: 1.5rem 3rem;
		grid-template-rows: auto auto 32rem auto;
	}

	.co-authors-count {
		margin: 0.25rem 0 0;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
	}

	.note {
		margin-top: auto;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		text-align: center;
	}
</style>
