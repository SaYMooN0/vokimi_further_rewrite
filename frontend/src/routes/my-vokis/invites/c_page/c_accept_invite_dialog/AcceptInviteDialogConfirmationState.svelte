<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
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
	export function reset() {
		isLoading = false;
		errs = [];
	}
</script>

{#if isLoading}
	<div class="loading-backdrop" aria-hidden="true">
		<div class="loading-content">
			<CubesLoader sizeRem={4} speedSec={1.75}  color= 'var(--primary)'/>
			<p class="loading-text">We are adding you to Voki co-authors<br /> Please wait a bit</p>
		</div>
	</div>
{/if}
<p class="main-text">
	Are you sure you want to join
	<BasicUserDisplay userId={invite.primaryAuthorId} interactionLevel="WholeComponentLink" />
	in the creation of <span class="voki-name">{invite.vokiName}</span> Voki as a co-author?
</p>

<DefaultErrBlock errList={errs} class="confirm-invite-errs-block" />

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
		line-height: 1.375;
		color: var(--text);
		text-indent: 1em;
		font-size: 1.25rem;
		text-wrap: pretty;
		font-weight: 475;
		text-align: justify;
		margin: auto 0;

	}
	.main-text > :global(.user-display) {
		display: inline-grid;
		vertical-align: middle;
		--profile-pic-width: 2.375rem;
		margin: 0.125rem 0.25rem;
	}
	.main-text > .voki-name {
		color: var(--muted-foreground);
		font-weight: 500;
		border-radius: 0.5rem;
		font-size: 1.375rem;
		text-indent: 0;
		text-decoration: underline;
	}
	:global(.confirm-invite-errs-block) {
		margin: 0.5rem 0;
	}
	.buttons {
		margin-top: auto;
		display: flex;
		gap: 0.75rem;
		justify-content: flex-end;
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
		display: grid;
		place-items: center;
		align-items: center;
		align-content: center;
		justify-items: center;
		justify-content: center;
		height: 100%;
		width: 100%;
		inset: 0;
		border-radius: inherit;
		background: rgba(40, 40, 40, 0.06);
		backdrop-filter: blur(1px);

		animation: var(--default-fade-in);
	}
	.loading-content {
		background-color: var(--back);
		display: flex;
		align-items: center;
		justify-content: center;
		flex-direction: column;
		width: fit-content;
		margin-bottom: 1rem;
		gap: 1.5rem;

		border-radius: 1rem;
		padding: 2rem 2rem;
	}
	.loading-text {
		font-size: 1.375rem;
		letter-spacing: 0.5px;
		color: var(--muted-foreground);
		text-align: center;
		font-weight: 475;
		line-height: 1.125;
	}
</style>
