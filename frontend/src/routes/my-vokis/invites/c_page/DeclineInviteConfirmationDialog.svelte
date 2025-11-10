<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';

	let inviteToDecline: InviteForVokiCoAuthorData | undefined = $state();
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = $state(false);
	let errs = $state<Err[]>([]);

	export function open(invite: InviteForVokiCoAuthorData) {
		inviteToDecline = invite;
		dialog.open();
	}

	async function declineInvite() {
		errs = [];
		if (!inviteToDecline) {
			errs = [{ message: 'Invite to accept is not selected' }];
			return;
		}

		isLoading = true;
		const response = await ApiVokiCreationCore.fetchVoidResponse(
			`/vokis/${inviteToDecline.vokiId}/decline-co-author-invite`,
			RJO.PATCH({})
		);
		isLoading = false;

		if (response.isSuccess) {
			closeDialog();
			toast.success('Invite declined');
		} else {
			errs = response.errs;
		}
	}

	function closeDialog() {
		dialog.close();
		isLoading = false;
		inviteToDecline = undefined;
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="decline-invite-dialog">
	{#if inviteToDecline}
    {:else}
    {/if}
</DialogWithCloseButton>
