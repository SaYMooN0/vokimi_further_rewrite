<script lang="ts">
	import { page } from '$app/state';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { ErrUtils } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { getSignInDialogOpenFunction } from '../../../c_layout/ts_layout_contexts/sign-in-dialog-context';
	import { goto } from '$app/navigation';
	import { ApiAuth } from '$lib/ts/backend-communication/backend-services';

	let userId: string = page.params.userId;
	let confirmationCode: string = page.params.confirmationCode;
	const openSignInDialog = getSignInDialogOpenFunction();

	async function confirmRegistration() {
		return ApiAuth.fetchVoidResponse(
			'/confirm-registration',
			RequestJsonOptions.POST({ userId, confirmationCode })
		);
	}
	function openDialogToLogin() {
		goto('/');
		openSignInDialog('login');
	}
</script>

<div class="view-container">
	{#await confirmRegistration()}
		<h1 class="loading-h">Confirming your email...</h1>
	{:then response}
		{#if response.isSuccess}
			<h1>You have successfully confirmed your email</h1>
			<PrimaryButton onclick={() => openDialogToLogin()} class="login-btn"
				>Now you can log into your account</PrimaryButton
			>
		{:else}
			<h1 class="error-h">An error has occurred during confirmation</h1>
			<div class="err-view">
				{response.errs[0].message}
				{#if ErrUtils.hasNonEmptyDetails(response.errs[0])}
					<p>Details: {response.errs[0].details}</p>
				{/if}
				{#if ErrUtils.hasSpecifiedCode(response.errs[0])}
					<p>Code: {response.errs[0].code}</p>
				{/if}
			</div>
		{/if}
	{/await}
</div>

<style>
	.view-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		margin: 10rem auto 0;
	}

	h1 {
		font-size: 2.5rem;
		font-weight: 500;
		text-align: center;
		letter-spacing: 0.5px;
	}

	.error-h {
		color: var(--err-foreground);
	}

	.err-view {
		padding: 0.75rem 1rem;
		margin-top: 1rem;
		border-radius: 0.5rem;
		background-color: var(--err-back);
		color: var(--err-foreground);
		font-size: 1.25rem;
	}

	.err-view p {
		margin: 0.5rem 0 0 0.25rem;
		color: var(--err-foreground);
	}

	.view-container > :global(.login-btn) {
		margin-top: 2rem;
	}
</style>
