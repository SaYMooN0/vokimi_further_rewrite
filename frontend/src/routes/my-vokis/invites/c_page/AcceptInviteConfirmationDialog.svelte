<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';

	type DialogState =
		| { name: 'NoInviteSelected' }
		| { name: 'ConfirmMessage'; invite: InviteForVokiCoAuthorData }
		| { name: 'Confirmed'; vokiName: string; vokiId: string; vokiType: string };

	let dialogState: DialogState = $state<DialogState>({ name: 'NoInviteSelected' });
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = $state(false);
	let errs = $state<Err[]>([]);

	export function open(invite: InviteForVokiCoAuthorData) {
		dialogState = { name: 'ConfirmMessage', invite };
		dialog.open();
	}

	async function acceptInvite() {
		errs = [];
		if (dialogState.name !== 'ConfirmMessage') {
			errs = [{ message: 'Invite to accept is not selected' }];
			return;
		}

		isLoading = true;
		const response = await ApiVokiCreationCore.fetchVoidResponse(
			`/vokis/${dialogState.invite.vokiId}/accept-co-author-invite`,
			RJO.PATCH({})
		);
		isLoading = false;

		if (response.isSuccess) {
			dialogState = {
				name: 'Confirmed',
				vokiName: dialogState.invite.vokiName,
				vokiId: dialogState.invite.vokiId,
				vokiType: dialogState.invite.vokiType
			};
		} else {
			errs = response.errs;
		}
	}

	function closeDialog() {
		dialog.close();
		isLoading = false;
		dialogState = { name: 'NoInviteSelected' };
	}

	function vokiCreationLink(data: { vokiId: string; vokiType: string }) {
		return `/voki-creation/${StringUtils.pascalToKebab(data.vokiType)}/${data.vokiId}`;
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="accept-invite-dialog">
	{#if dialogState.name === 'ConfirmMessage'}
		<p class="main-text">
			Are you sure you want to join
			<BasicUserDisplay userId={dialogState.invite.primaryAuthorId} />
			in the creation of <span class="voki-name">{dialogState.invite.vokiName}</span> Voki as a co-author?
		</p>

		<DefaultErrBlock errList={errs} />

		<div class="buttons">
			<button class="btn secondary" disabled={isLoading} onclick={() => closeDialog()}>
				Cancel
			</button>
			<button
				class="btn primary"
				disabled={isLoading}
				aria-busy={isLoading}
				onclick={() => acceptInvite()}
			>
				Confirm
			</button>
		</div>

		{#if isLoading}
			<div class="loading-backdrop" aria-hidden="true">
				<span class="loading-text">Processing...</span>
			</div>
		{/if}
	{:else if dialogState.name === 'NoInviteSelected'}
		<div class="empty">
			<p class="invite-not-selected">Invite to accept is not selected</p>
			<div class="buttons">
				<button class="btn secondary" onclick={() => closeDialog()}>Close</button>
			</div>
		</div>
	{:else if dialogState.name === 'Confirmed'}
		<p class="main-text">
			You now a co-author of <span class="voki-name">{dialogState.vokiName}</span> Voki
		</p>
		<div class="buttons">
			<a
				class="link-to-voki"
				href={vokiCreationLink(dialogState)}
				aria-label="Open Voki creation page"
			>
				Open Voki creation page
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
					<path
						d="M9 6.65032C9 6.65032 15.9383 6.10759 16.9154 7.08463C17.8924 8.06167 17.3496 15 17.3496 15M16.5 7.5L6.5 17.5"
						stroke="currentColor"
						stroke-linecap="round"
						stroke-linejoin="round"
					/>
				</svg>
			</a>

			<button class="" onclick={() => closeDialog()}>Close</button>
		</div>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#accept-invite-dialog > .dialog-content) {
		position: relative;
		height: 32rem;
		width: 48rem;
	}

	.main-text {
		line-height: 1.5;
		color: var(--text);
		text-indent: 1em;
		font-size: 1.25rem;
		text-wrap: pretty;
		font-weight: 450;
	}
	.main-text > :global(.user-display) {
		display: inline-grid;
		vertical-align: middle;
		margin: 0 0.25rem;
		--profile-pic-width: 2.5rem;
	}
	.main-text > .voki-name {
		color: var(--muted-foreground);
		font-weight: 500;
		background-color: var(--muted);
		padding: 0.125rem 0.5rem;
		border-radius: 0.5rem;
	}
	.buttons {
		display: flex;
		gap: 0.75rem;
		justify-content: flex-end;
		margin-top: 2rem;
		width: 100%;
	}

	.btn {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		width: 7.5rem;
		padding: 0.375rem 1rem;
		border: none;
		font-size: 1rem;
		cursor: pointer;
		border-radius: 0.25rem;
		font-weight: 450;
		letter-spacing: 0.25px;
	}

	.btn.primary {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover:where(:not([disabled])) {
		background-color: var(--primary-hov);
	}
	.btn.secondary {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.btn.secondary:hover:where(:not([disabled])) {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.link-to-voki {
		display: inline-block;
		width: max-content;
		padding: 0.5rem 0.75rem;
		border-radius: calc(var(--radius) - 0.125rem);
		background: var(--back);
		color: var(--accent-foreground);
		box-shadow: var(--shadow-xs);
	}

	.empty {
		display: grid;
		gap: 0.875rem;
	}

	.invite-not-selected {
		color: var(--muted-foreground);
	}

	.loading-backdrop {
		position: absolute;
		inset: 0;
		display: grid;
		place-items: center;
		background: rgba(0, 0, 0, 0.04);
		border-radius: var(--radius);
		backdrop-filter: blur(0.15rem);
		animation: var(--default-fade-in);
	}

	.loading-text {
		margin-top: 0.5rem;
		font-size: 2rem;
		letter-spacing: 0.5px;
		color: var(--muted-foreground);
	}
</style>
