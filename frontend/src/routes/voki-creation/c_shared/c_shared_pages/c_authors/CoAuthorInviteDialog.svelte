<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { UserViewData } from '$lib/ts/users';
	import CoAuthorInvitedMessage from './c_inviting_dialog/CoAuthorInvitedMessage.svelte';
	import type { VokiAuthorsInfo } from './types';
	import UserSearchBar from './c_inviting_dialog/UserSearchBar.svelte';
	import { ApiVokiCreationCore } from '$lib/ts/backend-communication/backend-services';

	let { vokiId, updateParent }: { vokiId: string; updateParent: (info: VokiAuthorsInfo) => void } =
		$props<{ vokiId: string; updateParent: (info: VokiAuthorsInfo) => void }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let errs: Err[] = $state([]);
	let searchedUsers = $state<UserViewData[]>([]);
	let isAlreadyInvited = $state(false);
	let invitedUserId = $state('');
	export function open() {
		isAlreadyInvited = false;
		errs = [];
		dialog.open();
	}
	async function inviteUser(userId: string) {
		const response = await ApiVokiCreationCore.fetchJsonResponse<VokiAuthorsInfo>(
			`/vokis/${vokiId}/invite-co-author`,
			RequestJsonOptions.POST({ newCoAuthorId: userId })
		);
		if (response.isSuccess) {
			updateParent(response.data);
			isAlreadyInvited = true;
			invitedUserId = userId;
		} else {
			errs = response.errs;
		}
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
					{user.username}
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
