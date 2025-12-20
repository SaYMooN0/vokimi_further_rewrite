<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	interface Props {
		managerIds: string[];
		coAuthorIds: Set<string>;
		primaryAuthorId: string;
	}
	let { managerIds, coAuthorIds, primaryAuthorId }: Props = $props();
	function managersThatWereCoAuthors() {
		return managerIds.filter((managerId) => coAuthorIds.has(managerId));
	}
	function managersThatWereNotCoAuthors() {
		return managerIds.filter((managerId) => !coAuthorIds.has(managerId));
	}
	let dialog = $state<DialogWithCloseButton>()!;
	export function open() {
		dialog.open();
	}
</script>

<DialogWithCloseButton bind:this={dialog} subheading="Voki managers">
	<div class="managers-list-row">
		<span class="column-name">Manager</span>
		<span class="column-name">Was user an author</span>
	</div>
	<div class="managers-list">
		<div class="managers-list-row" title="primary author">
			<BasicUserDisplay userId={primaryAuthorId} interactionLevel="WholeComponentLink" />
			<svg class="check-icon primary-author"><use href="#common-check-icon" /></svg>
		</div>
		<div class="sep"></div>
		{#each managersThatWereCoAuthors() as m}
			<div class="managers-list-row" title={m}>
				<BasicUserDisplay userId={m} interactionLevel="WholeComponentLink" />
				<svg class="check-icon"><use href="#common-check-icon" /></svg>
			</div>
		{/each}
		<div class="sep"></div>
		{#each managersThatWereNotCoAuthors() as m}
			<div class="managers-list-row" title={m}>
				<BasicUserDisplay userId={m} interactionLevel="WholeComponentLink" />
				<div class="empty-div"></div>
			</div>
		{/each}
	</div>
</DialogWithCloseButton>

<style>
	.managers-list {
		display: flex;
		flex-direction: column;
		overflow-y: auto;
	}
	.managers-list-row {
		display: grid;
		grid-template-columns: 1fr 4rem;
	}
	.check-icon {
		width: 2rem;
		aspect-ratio: 1;
	}
	.primary-author {
		color: var(--primary);
	}
	.empty-div {
		width: 2rem;
	}
</style>
