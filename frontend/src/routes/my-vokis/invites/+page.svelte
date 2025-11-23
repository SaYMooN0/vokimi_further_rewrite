<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { onMount } from 'svelte';
	import MyVokisPageInitialLoading from '../c_shared/MyVokisPageInitialLoading.svelte';
	import MyVokisPageUnexpectedStateAfterLoading from '../c_shared/MyVokisPageUnexpectedStateAfterLoading.svelte';
	import { registerCurrentPageApi } from '../my-vokis-page-context';
	import { MyVokiInvitesPageState } from './my-voki-invites-page-state.svelte';
	import InviteForCoAuthorDisplay from './c_page/InviteForCoAuthorDisplay.svelte';
	import AcceptInviteConfirmationDialog from './c_page/AcceptInviteConfirmationDialog.svelte';
	import DeclineInviteConfirmationDialog from './c_page/DeclineInviteConfirmationDialog.svelte';

	const pageState = new MyVokiInvitesPageState();
	onMount(() => {
		const registerPageApi = registerCurrentPageApi();

		registerPageApi({
			forceRefetch: () => pageState.forceRefetch(),
			get isLoading() {
				return pageState.loadingState.state === 'loading';
			}
		});
	});

	let acceptInviteDialog = $state<AcceptInviteConfirmationDialog>()!;
	let declineInviteDialog = $state<DeclineInviteConfirmationDialog>()!;
</script>

{#if pageState.loadingState.state === 'loading'}
	<MyVokisPageInitialLoading loadingText="loading your invites" />
{:else if pageState.loadingState.state === 'errs'}
	<DefaultErrBlock errList={pageState.loadingState.errs} />
{:else if pageState.loadingState.state === 'loaded'}
	{#if pageState.loadingState.invites.length === 0}
		<h1>You don't have any invites</h1>
	{:else}
		<AcceptInviteConfirmationDialog bind:this={acceptInviteDialog} />
		<DeclineInviteConfirmationDialog
			bind:this={declineInviteDialog}
			updateParent={(newInvites) => pageState.updateByInviteIds(newInvites)}
		/>
		<div class="invites-container">
			{#each pageState.loadingState.invites as inv}
				<InviteForCoAuthorDisplay
					invite={inv}
					onAccept={() => acceptInviteDialog.open(inv)}
					onDecline={() => declineInviteDialog.open(inv)}
				/>
			{/each}
		</div>
	{/if}
{:else}
	<MyVokisPageUnexpectedStateAfterLoading reloadPage={pageState.forceRefetch} />
{/if}

<style>
	.invites-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>
