<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { UserProfilePreview } from '$lib/ts/users';
	import UserSearchBar from './c_inviting_dialog/UserSearchBar.svelte';
	import SearchedUsersListDisplay from './c_inviting_dialog/SearchedUsersListDisplay.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
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
	let searchedUsers = $state<UserProfilePreview[]>([]);
	let searchBarInputVal = $state('');
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
		{searchedUsers}
		isUserAlreadyCoAuthor={coAuthorIds.includes}
		isUserInvitedForCoAuthor={invitedForCoAuthorUserIds.includes}
		{inviteUser}
	/>

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
