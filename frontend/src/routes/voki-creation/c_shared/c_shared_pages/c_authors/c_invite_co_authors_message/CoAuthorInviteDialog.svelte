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
		vokiId: string;
		updateParentCoAuthors: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void;
	}

	let { vokiId, updateParentCoAuthors: updateParentCoAuthors }: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;
	let errs: Err[] = $state([]);
	let searchedUsers = $state<UserProfilePreview[]>([]);
	let isAlreadyInvited = $state(false);
	let invitedUserId = $state('');
	export function open() {
		isAlreadyInvited = false;
		errs = [];
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
	{#if isAlreadyInvited}
		<CoAuthorInvitedMessage userId={invitedUserId} />
		<PrimaryButton onclick={() => dialog.close()}>Ok</PrimaryButton>
	{:else}
		<UserSearchBar bind:searchedUsers setErrs={(e) => (errs = e)} />
		<div class="users-list">
			{#each searchedUsers as user}
				<div class="user">
					{JSON.stringify(user)}
					<button class="invite-btn" onclick={() => inviteUser(user.userId)}>Invite</button>
				</div>
			{/each}
		</div>
		<label class="note"
			>Note: invited user will see the type, name, cover and other main details of this voki</label
		>
	{/if}
</DialogWithCloseButton>

<style>
</style>
