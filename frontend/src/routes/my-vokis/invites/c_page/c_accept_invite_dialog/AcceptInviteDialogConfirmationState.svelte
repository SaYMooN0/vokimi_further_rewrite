<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import type { InviteForVokiCoAuthorData } from '../../my-voki-invites-page-state.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		errs: Err;
	}
	let { invite, errs }: Props = $props();
</script>

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
