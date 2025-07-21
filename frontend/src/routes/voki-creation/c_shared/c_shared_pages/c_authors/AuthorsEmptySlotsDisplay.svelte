<script lang="ts">
	const {
		coAuthorsCount,
		invitedCoAuthorsCount,
		openAuthorsInviteDialog
	}: {
		coAuthorsCount: number;
		invitedCoAuthorsCount: number;
		openAuthorsInviteDialog: () => void;
	} = $props<{
		coAuthorsCount: number;
		invitedCoAuthorsCount: number;
		openAuthorsInviteDialog: () => void;
	}>();
	let remainingSlots = 5 - coAuthorsCount - invitedCoAuthorsCount;
	let emptySlots: number[] = Array.from({ length: remainingSlots }, (_, i) => i + 1);
</script>

{#each emptySlots as _}
	<div class="empty-co-author-slot">
		<button class="invite-new-author" onclick={() => openAuthorsInviteDialog()}
			>Invite new co-author</button
		>
	</div>
{/each}
