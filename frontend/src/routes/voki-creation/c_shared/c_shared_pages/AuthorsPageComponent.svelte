<script lang="ts">
	import AuthorInviteDisplay from "./c_authors/AuthorInviteDisplay.svelte";
	import AuthorUserDisplay from "./c_authors/AuthorUserDisplay.svelte";
	import type { VokiAuthorsInfo } from "./c_authors/types";
	import CoAuthorInviteDialog from "./c_authors/CoAuthorInviteDialog.svelte";
	import NoCoAuthorsMessage from "./c_authors/NoCoAuthorsMessage.svelte";

    
	let { authorsInfo, vokiId }: { authorsInfo: VokiAuthorsInfo; vokiId: string } = $props<{
		authorsInfo: VokiAuthorsInfo;
		vokiId: string;
	}>();
	let emptySlots: number[] = [];
	let dialog = $state<CoAuthorInviteDialog>()!;
</script>

<CoAuthorInviteDialog bind:this={dialog} {vokiId} updateParent={(info) => (authorsInfo = info)} />
<div class="authors-tab-container">
	<h1 class="authors-h">Primary author</h1>
	<AuthorUserDisplay userId={authorsInfo.primaryAuthorId} />
	<h1 class="authors-h">Co-authors ({authorsInfo.coAuthorIds.length})/5</h1>
	{#if authorsInfo.coAuthorIds.length === 0}
		<NoCoAuthorsMessage openAuthorsInviteDialog={() => dialog.open()} />
	{:else}
		{#each authorsInfo.coAuthorIds as coauthorId}
			<AuthorUserDisplay userId={coauthorId} />
		{/each}
		{#if authorsInfo.invitedCoAuthorIds.length != 0}
			<h1 class="authors-h">Co-authors invites</h1>
			{#each authorsInfo.invitedCoAuthorIds as invitedCoAuthorIds}
				<AuthorInviteDisplay userId={invitedCoAuthorIds} />
			{/each}
		{/if}
		{#each emptySlots as _}
			<div class="empty-co-author-slot">
				<button class="invite-new-author" onclick={() => dialog.open()}>Invite new co-author</button
				>
			</div>
		{/each}
	{/if}
</div>

<style>
	.authors-tab-container {
		display: flex;
		flex-direction: column;
	}
	.authors-h {
		font-weight: 450;
	}
</style>
