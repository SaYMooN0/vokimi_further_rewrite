<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiAuth } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';

	export function open() {
		dialog.open();
		responseErrs = [];
	}
	async function logout() {
		if (AuthStore.Get().isAuthenticated === true) {
			const response = await ApiAuth.fetchVoidResponse('/logout', RequestJsonOptions.POST({}));
			if (response.isSuccess) {
				AuthStore.GetWithForceRefresh();
			} else {
				responseErrs = response.errs;
				return;
			}
		}
		goto('/');
	}
	let responseErrs = $state<Err[]>([]);
	let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="logout-confirmation-dialog">
	<h1 class="title">Are you sure you want to log out?</h1>
	<p class="unsaved-changes-msg">
		After logging out you will be redirected to the home page. If you have any unsaved changes on
		the current page save them before logging out otherwise they will be lost
	</p>
	<div class="errs-container">
		{#if responseErrs.length > 0}
			<h1 class="main-err-msg">Something went wrong while logging out</h1>
			<DefaultErrBlock errList={responseErrs} />
		{/if}
	</div>
	<div class="actions">
		<button class="cancel-btn" onclick={() => dialog.close()}>Cancel </button>
		<button class="logout-btn" onclick={() => logout()}>Logout</button>
	</div>
</DialogWithCloseButton>

<style>
	:global(#logout-confirmation-dialog > .dialog-content) {
		width: 40rem;
		min-height: 20rem;
	}
	.title {
		margin: 0;
		color: var(--muted-foreground);
		font-size: 2rem;
		font-weight: 600;
	}
	.unsaved-changes-msg {
		margin: 1rem 0 0;
		color: var(--secondary-foreground);
		font-size: 1.125em;
		line-height: 1.2;
		text-align: center;
		letter-spacing: 0.25px;
		font-weight: 450;
		text-wrap: balance;
	}
	.errs-container {
		margin-top: auto;
		min-height: 5rem;
		margin-bottom: 2rem;
		display: block;
		width: 100%;
	}
	.main-err-msg {
		color: var(--err-foreground);
		font-size: 1.25rem;
		font-weight: 500;
		margin-bottom: 0.25rem;
	}
	.actions {
		width: 100%;
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 2rem;
		padding: 0 2rem;
	}
	.logout-btn,
	.cancel-btn {
		padding: 0.5rem 0;
		border: none;
		border-radius: 0.25rem;
		font-size: 1rem;
		font-weight: 500;
		transition: 0.15s ease;
		cursor: pointer;
	}

	.logout-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.logout-btn:hover {
		background-color: var(--primary-hov);
	}

	.cancel-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.cancel-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
