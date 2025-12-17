<script lang="ts">
	interface Props {
		managerIds: string[];
		openManagersDialog: () => void;
		coauthorIds: string[];
	}
	let { managerIds, openManagersDialog, coauthorIds }: Props = $props();
	function primaryAuthorOnly() {
		return managerIds.length === 0;
	}
	function allManagersAreAllCoAuthors() {
		return managerIds.every((managerId) => coauthorIds.includes(managerId));
	}
</script>

managed by:
<div
	class="managers-chip"
	onclick={() => openManagersDialog()}
	class:primary-author-only={primaryAuthorOnly()}
	class:all-managers-are-all-co-authors={allManagersAreAllCoAuthors()}
>
	{#if primaryAuthorOnly()}
		only the primary author
	{:else if allManagersAreAllCoAuthors()}
		co-authors
	{:else}
		{managerIds.length} managers
	{/if}
</div>
