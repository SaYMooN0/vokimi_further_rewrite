<script lang="ts">
	interface Props {
		managerIds: string[];
		coAuthorIds: Set<string>;
		primaryAuthorId: string;
	}
	let { managerIds, coAuthorIds, primaryAuthorId }: Props = $props();
	function managersThatWereCoAuthors() {
		return managerIds.filter((managerId) => coAuthorIds.has(managerId));
	}
	function managersWithNoCoAuthors() {
		return managerIds.filter((managerId) => !coAuthorIds.has(managerId));
	}
</script>

<div class="column-names">manager | was an author</div>
<div>
	<div class="row" title="primary author">
		primary author
		<!-- check with primary color -->
		<svg><use href="#common-check-icon" /></svg>
	</div>
	{#each managersThatWereCoAuthors() as m}
		author
		<svg><use href="#common-check-icon" /></svg>
	{/each}
</div>
