<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';
	import AcceptInviteDialogConfirmationState from './c_accept_invite_dialog/AcceptInviteDialogConfirmationState.svelte';
	import AcceptInviteDialogConfirmedState from './c_accept_invite_dialog/AcceptInviteDialogConfirmedState.svelte';

	type DialogState =
		| { name: 'NoInviteSelected' }
		| { name: 'ConfirmMessage'; invite: InviteForVokiCoAuthorData }
		| { name: 'Confirmed'; vokiName: string; vokiId: string; vokiType: string };

	let dialogState: DialogState = $state<DialogState>({ name: 'NoInviteSelected' });
	let dialog = $state<DialogWithCloseButton>()!;

	export function open(invite: InviteForVokiCoAuthorData) {
		dialogState = { name: 'ConfirmMessage', invite };
		dialog.open();
	}

	function closeDialog() {
		dialog.close();
		dialogState = { name: 'NoInviteSelected' };
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="accept-invite-dialog">
	{#if dialogState.name === 'ConfirmMessage'}
		<AcceptInviteDialogConfirmationState
			invite={dialogState.invite}
			changeStateToConfirmed={(inv) => {
				dialogState = {
					name: 'Confirmed',
					vokiName: inv.vokiName,
					vokiId: inv.vokiId,
					vokiType: inv.vokiType
				};
			}}
			{closeDialog}
		/>
	{:else if dialogState.name === 'Confirmed'}
		<AcceptInviteDialogConfirmedState
			vokiType={dialogState.vokiType}
			vokiId={dialogState.vokiId}
			vokiName={dialogState.vokiName}
			{closeDialog}
		/>
	{:else if dialogState.name === 'NoInviteSelected'}
		<div class="empty">
			<p class="invite-not-selected">Invite to accept is not selected</p>
			<div class="buttons">
				<button class="btn secondary" onclick={() => closeDialog()}>Close</button>
			</div>
		</div>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#accept-invite-dialog > .dialog-content) {
		position: relative;
		height: 32rem;
		width: 48rem;
	}


	.empty {
		display: grid;
		gap: 0.875rem;
	}

	.invite-not-selected {
		color: var(--muted-foreground);
	}

	
</style>
