<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import UserSearchBar from './c_inviting_dialog/UserSearchBar.svelte';
	import SearchedUsersListDisplay from './c_inviting_dialog/SearchedUsersListDisplay.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { UserPreviewWithInvitesSettings } from '../types';
	import ConfirmInviteBtnContainer from './c_inviting_dialog/ConfirmInviteBtnContainer.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import { watch } from 'runed';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	interface Props {
		maxCoAuthorsCount: number;
		primaryAuthorId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		vokiId: string;
		updateParentCoAuthors: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}

	let {
		maxCoAuthorsCount,
		primaryAuthorId,
		coAuthorIds,
		invitedForCoAuthorUserIds,
		vokiId,
		updateParentCoAuthors
	}: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;
	let searchedUsers = $state<UserPreviewWithInvitesSettings[]>([]);
	let searchBarInputVal = $state('');
	export function open() {
		searchBarInputVal = '';
		searchedUsers = [];
		savingErrs = [];
		isLoadingSave = false;
		dialog.open();
	}
	let usersChosenToInvite: UserPreviewWithInvitesSettings[] = $state([]);

	function addUserToInvite(user: UserPreviewWithInvitesSettings) {
		if (!usersChosenToInvite.some((u) => u.id === user.id)) {
			usersChosenToInvite = [...usersChosenToInvite, user];
		}
	}

	function removeUserFromToInvite(user: UserPreviewWithInvitesSettings) {
		usersChosenToInvite = usersChosenToInvite.filter((u) => u.id !== user.id);
	}

	function isUserInListToInvite(userId: string) {
		return usersChosenToInvite.some((u) => u.id === userId);
	}
	function getUserInviteStateForVoki(
		userId: string
	): 'PrimaryAuthor' | 'CoAuthor' | 'AlreadyInvited' | 'CandidateToInvite' {
		if (userId === primaryAuthorId) {
			return 'PrimaryAuthor';
		} else if (coAuthorIds.includes(userId)) {
			return 'CoAuthor';
		} else if (invitedForCoAuthorUserIds.includes(userId)) {
			return 'AlreadyInvited';
		} else {
			return 'CandidateToInvite';
		}
	}
	async function confirmUsersInvite() {
		savingErrs = [];
		isLoadingSave = true;
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			coAuthorIds: string[];
			invitedForCoAuthorUserIds: string[];
		}>(
			`/vokis/${vokiId}/invite-co-authors`,
			RJO.POST({
				userIds: usersChosenToInvite.map((u) => u.id)
			})
		);
		isLoadingSave = false;
		if (response.isSuccess) {
			console.log(response.data);
			updateParentCoAuthors(response.data.coAuthorIds, response.data.invitedForCoAuthorUserIds);
			dialog.close();
			savingErrs = [];
		} else {
			savingErrs = response.errs;
		}
	}
	let savingErrs: Err[] = $state([]);
	let isLoadingSave = $state(false);
	watch(
		() => [searchBarInputVal, usersChosenToInvite],
		() => {
			savingErrs = [];
		}
	);
</script>

<DialogWithCloseButton dialogId="co-author-inviting-dialog" bind:this={dialog}>
	<UserSearchBar bind:searchBarInputVal bind:searchedUsers />
	<p1 class="co-authors-count"
		>Co-authors (including invited) count: {coAuthorIds.length +
			invitedForCoAuthorUserIds.length}/{maxCoAuthorsCount}</p1
	>
	<SearchedUsersListDisplay
		isInputEmpty={StringUtils.isNullOrWhiteSpace(searchBarInputVal)}
		userOptions={searchedUsers}
		{isUserInListToInvite}
		addToListToInvite={(u) => addUserToInvite(u)}
		removeFromListToInvite={(u) => removeUserFromToInvite(u)}
		getUserInviteStateForVoki={(userId) => getUserInviteStateForVoki(userId)}
	/>
	<ConfirmInviteBtnContainer
		{usersChosenToInvite}
		isLoading={isLoadingSave}
		onInviteButtonClick={confirmUsersInvite}
	/>
	<div class="errs-container">
		<DefaultErrBlock errList={savingErrs} />
	</div>
	<label class="note"
		>Note: invited user will see the type, name, cover and other main details of this Voki</label
	>
</DialogWithCloseButton>

<style>
	:global(#co-author-inviting-dialog > .dialog-content) {
		display: grid;
		width: 44rem;
		padding: 2rem 2rem;
		grid-template-rows: auto auto 32rem auto auto auto;
	}

	.co-authors-count {
		margin: 0.25rem 0 0;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
	}
	.errs-container {
		min-height: 1.5rem;
		margin: 0.25rem 0;
	}
	.note {
		margin-top: auto;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
		text-align: center;
	}
</style>
