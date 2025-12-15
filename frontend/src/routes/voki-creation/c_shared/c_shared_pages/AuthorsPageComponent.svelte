<script lang="ts">
	import PrimaryAuthorDisplay from './c_authors/PrimaryAuthorDisplay.svelte';
	import CoAuthorsList from './c_authors/CoAuthorsList.svelte';
	import InviteCoAuthorsMessage from './c_authors/InviteCoAuthorsMessage.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import type { CoAuthorsPageState } from './c_authors/co-authors-page-state.svelte';
	import VokiManagersSection from './c_authors/VokiManagersSection.svelte';
	interface Props {
		pageState: CoAuthorsPageState;
		vokiCreationDate: Date;
	}
	let { pageState, vokiCreationDate }: Props = $props();
</script>

<AuthView>
	{#snippet children(authState)}
		{#if authState.name === 'authenticated'}
			<div class="authors-tab-container">
				<PrimaryAuthorDisplay
					viewerId={authState.userId}
					creationDate={vokiCreationDate}
					primaryAuthorId={pageState.primaryAuthorId}
				/>
				<CoAuthorsList
					viewerId={authState.userId}
					isViewerPrimaryAuthor={pageState.primaryAuthorId === authState.userId}
					{pageState}
				/>
				<InviteCoAuthorsMessage
					isViewerPrimaryAuthor={pageState.primaryAuthorId === authState.userId}
					{pageState}
				/>
				{#if pageState.coAuthorIds.length + pageState.invitedForCoAuthorUserIds.length > 0}
					<VokiManagersSection
						isViewerPrimaryAuthor={pageState.primaryAuthorId === authState.userId}
						viewerId={authState.userId}
						expectedManagers={pageState.expectedManagers}
						vokiCoAuthors={pageState.coAuthorIds}
						updateManagersSetting={(newManagers) => (pageState.expectedManagers = newManagers)}
					/>
				{/if}
			</div>
		{:else if authState.name === 'loading'}
			<div>Loading...</div>
		{:else}
			<h3>Something went wrong please reload the page</h3>
		{/if}
	{/snippet}
</AuthView>

<style>
	.authors-tab-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		padding-top: 1rem;
	}
</style>
