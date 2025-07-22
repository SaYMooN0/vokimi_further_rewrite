<script lang="ts">
	import AuthorInviteDisplay from './c_authors/AuthorInviteDisplay.svelte';
	import type { VokiAuthorsInfo } from './c_authors/types';
	import CoAuthorInviteDialog from './c_authors/CoAuthorInviteDialog.svelte';
	import NoCoAuthorsMessage from './c_authors/NoCoAuthorsMessage.svelte';
	import CoAuthorUserDisplay from './c_authors/CoAuthorUserDisplay.svelte';
	import AuthorsEmptySlotsDisplay from './c_authors/AuthorsEmptySlotsDisplay.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	let { authorsInfo, vokiId }: { authorsInfo: VokiAuthorsInfo; vokiId: string } = $props<{
		authorsInfo: VokiAuthorsInfo;
		vokiId: string;
	}>();

	let dialog = $state<CoAuthorInviteDialog>()!;
</script>

<CoAuthorInviteDialog bind:this={dialog} {vokiId} updateParent={(info) => (authorsInfo = info)} />
<div class="authors-tab-container">
	<h1 class="authors-h">Primary author</h1>
	<div class="primary-author-user">
		<img
			class="profile-picture"
			src={StorageBucketMain.fileSrc(`/profile-pictures/${authorsInfo.primaryAuthorId}`)}
		/>
		<a href={`/users/${authorsInfo.primaryAuthorId}`} class="username"
			>{authorsInfo.primaryAuthorId}</a
		>
	</div>
	{#if authorsInfo.coAuthorIds.length === 0}
		<NoCoAuthorsMessage openAuthorsInviteDialog={() => dialog.open()} />
	{:else}
		<h1 class="authors-h">Co-authors ({authorsInfo.coAuthorIds.length})/5</h1>
		{#each authorsInfo.coAuthorIds as coauthorId}
			<CoAuthorUserDisplay userId={coauthorId} />
		{/each}
		{#if authorsInfo.invitedCoAuthorIds.length != 0}
			<h1 class="authors-h">Co-authors invites</h1>
			{#each authorsInfo.invitedCoAuthorIds as invitedCoAuthorIds}
				<AuthorInviteDisplay userId={invitedCoAuthorIds} />
			{/each}
		{/if}
		<AuthorsEmptySlotsDisplay
			coAuthorsCount={authorsInfo.coAuthorIds.length}
			invitedCoAuthorsCount={authorsInfo.invitedCoAuthorIds.length}
			openAuthorsInviteDialog={() => dialog.open()}
		/>
	{/if}
</div>

<style>
	.authors-tab-container {
		display: flex;
		flex-direction: column;
	}

	.authors-h {
		color: var(--muted-foreground);
		font-size: 2rem;
		font-weight: 450;
	}
</style>
