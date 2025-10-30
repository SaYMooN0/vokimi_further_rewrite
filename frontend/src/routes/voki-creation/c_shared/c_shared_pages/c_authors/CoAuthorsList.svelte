<script lang="ts">
	interface Props {
		viewerId: string;
		coAuthorIds: string[];
		invitedForCoAuthorUserIds: string[];
		primaryAuthorId: string;
	}
	let { viewerId, coAuthorIds, invitedForCoAuthorUserIds: invitedCoAuthorIds, primaryAuthorId }: Props = $props();
</script>

<div class="all-co-authors-list">
	{#each coAuthorIds.sort((a, b) => a.localeCompare(b)) as userId}
		<div class:highlight={viewerId === userId}>
			userid {#if primaryAuthorId === viewerId}
				<button class="action-btn">drop</button>
			{/if}
		</div>
	{/each}
	{#if coAuthorIds.length > 0 && invitedCoAuthorIds.length > 0}
		<h2 class="invited-subheading">Invited co-authors({invitedCoAuthorIds.length})</h2>
	{/if}
	{#each invitedCoAuthorIds.sort((a, b) => a.localeCompare(b)) as userId}
		<div>
			userid
			{#if primaryAuthorId === viewerId}
				<button class="action-btn">Cancel invite</button>
			{/if}
		</div>
	{/each}
</div>
<!-- <div class="user-display">
	<img class="profile-picture" src={StorageBucketMain.fileSrc(`/profile-pictures/${userId}`)} />
	<a href={`/users/${userId}`} class="username">{userId}</a>
</div> -->
