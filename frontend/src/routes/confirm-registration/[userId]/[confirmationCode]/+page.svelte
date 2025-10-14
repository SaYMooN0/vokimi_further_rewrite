<script lang="ts">
	import { page } from '$app/state';
	import { ErrUtils } from '$lib/ts/err';
	import { ApiAuth, RJO } from '$lib/ts/backend-communication/backend-services';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { ResponseVoidResult } from '$lib/ts/backend-communication/result-types';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { goto } from '$app/navigation';

	let userId: string | undefined = page.params.userId;
	let confirmationCode: string | undefined = page.params.confirmationCode;

	async function confirmRegistration(): Promise<ResponseVoidResult> {
		if (StringUtils.isNullOrWhiteSpace(userId)) {
			return {
				isSuccess: false,
				errs: [{ message: 'Invalid confirmation link. UserId is missing' }]
			};
		}

		if (StringUtils.isNullOrWhiteSpace(confirmationCode)) {
			return {
				isSuccess: false,
				errs: [{ message: 'Invalid confirmation link. Confirmation code is missing' }]
			};
		}

		const response = await ApiAuth.fetchVoidResponse(
			'/confirm-registration',
			RJO.POST({ userId, confirmationCode })
		);
		if (response.isSuccess) {
			await AuthStore.GetWithForceRefresh();
			goto('/basic-profile-setup');
		}
		return response;
	}
</script>

<div class="view-container">
	{#await confirmRegistration()}
		<h1 class="loading-h">Confirming your email...</h1>
	{:then response}
		{#if response.isSuccess}
			<h2>You have successfully confirmed your email</h2>
			<a href="/">Go to the home page</a>
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
