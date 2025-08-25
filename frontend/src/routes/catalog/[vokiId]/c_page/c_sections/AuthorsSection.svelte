<script lang="ts">
	import { UsersStore } from '$lib/ts/stores/users-store-svelte';
	import AuthorView from './c_authors_section/AuthorView.svelte';

	let { primaryAuthorId, coAuthorIds }: { primaryAuthorId: string; coAuthorIds: string[] } =
		$props<{ primaryAuthorId: string; coAuthorIds: string[] }>();
</script>

<div class="authors-section">
	<label class="by-label">by: </label>
	{#await UsersStore.Get(primaryAuthorId)}
		loading user...
	{:then user}
		{#if user}
			<AuthorView {user} />
		{:else}
			<label>error in loading <a href="/authors/{primaryAuthorId}">user</a></label>
		{/if}
	{/await}
	{#if coAuthorIds.length > 0}
		and
		<div class="co-authors-container">
			{#each coAuthorIds as coAuthorId}
				<div class="co-author">
					<a href="/authors/{coAuthorId}">{coAuthorId}</a>
				</div>
			{/each}
		</div>
	{/if}
</div>

<style>
	.by-label {
		font-size: 1rem;
		font-weight: 450;
		margin-right: 0.5rem;
	}
	.authors-section {
		margin-top: 0.25rem;
		display: flex;
		flex-direction: row;
		align-items: center;
	}
</style>
