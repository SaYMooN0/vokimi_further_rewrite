<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { InviteForVokiCoAuthorData } from '../../my-voki-invites-page-state.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		changeStateToConfirmed: (inviteData: InviteForVokiCoAuthorData) => void;
		closeDialog: () => void;
	}
	let { invite, changeStateToConfirmed, closeDialog }: Props = $props();

	let isLoading = $state(false);
	let errs = $state<Err[]>([]);

	async function acceptInvite() {
		errs = [];
		isLoading = true;
		const response = await ApiVokiCreationCore.fetchVoidResponse(
			`/vokis/${invite.vokiId}/accept-co-author-invite`,
			RJO.PATCH({})
		);
		isLoading = false;

		if (response.isSuccess) {
			changeStateToConfirmed(invite);
		} else {
			errs = response.errs;
		}
	}
</script>

{#if isLoading}
	<div class="loading-backdrop" aria-hidden="true">
		<span class="loading-text">Processing...</span>
	</div>
{/if}
<p class="main-text">
	Are you sure you want to join
	<BasicUserDisplay userId={invite.primaryAuthorId} />
	in the creation of <span class="voki-name">{invite.vokiName}</span> Voki as a co-author?
</p>

<DefaultErrBlock errList={errs} />

<div class="buttons">
	<button class="btn secondary" disabled={isLoading} onclick={() => closeDialog()}> Cancel </button>
	<button
		class="btn primary"
		disabled={isLoading}
		aria-busy={isLoading}
		onclick={() => acceptInvite()}
	>
		Confirm
	</button>
</div>

<style>
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
	.loading-backdrop {
		position: absolute;
		inset: 0;
		display: grid;
		place-items: center;
		background: rgba(0, 0, 0, 0.04);
		backdrop-filter: blur(0.125rem);
		animation: var(--default-fade-in);
	}

	.loading-text {
		margin-top: 0.5rem;
		font-size: 2rem;
		letter-spacing: 0.5px;
		color: var(--muted-foreground);
	}
</style>
