<script lang="ts">
	import { browser } from '$app/environment';
	import { onDestroy, onMount } from 'svelte';
	import type { PageProps } from './$types';
	import CoverSection from './c_page/c_sections/CoverSection.svelte';
	import MainDetailsSection from './c_page/c_sections/MainDetailsSection.svelte';
	import NameSection from './c_page/c_sections/NameSection.svelte';
	import VokiPageAboutTab from './c_page/c_tabs/VokiPageAboutTab.svelte';
	import VokiPageCommentsTab from './c_page/c_tabs/VokiPageCommentsTab.svelte';
	import VokiPageRatingsTab from './c_page/c_tabs/VokiPageRatingsTab.svelte';
	import TabLinksContainer from './c_page/TabLinksContainer.svelte';
	import VokiNotLoaded from './c_page/VokiNotLoaded.svelte';
	import { VokiPageState } from './voki-page-state.svelte';
	import { VokiCatalogVisitMarkerCookie } from '$lib/ts/cookies/voki-catalog-visit-marker-cookie';
	import AuthorsSection from './c_page/c_sections/AuthorsSection.svelte';

	let { data }: PageProps = $props();
	let pageState = new VokiPageState(
		data.vokiId,
		data.currentTab,
		data.response.isSuccess ? data.response.data.ratingsCount : 0,
		data.response.isSuccess ? data.response.data.commentsCount : 0
	);

	let refreshTimer: number | undefined;
	onMount(() => {
		if (browser) {
			VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);

			refreshTimer = window.setInterval(() => {
				VokiCatalogVisitMarkerCookie.markSeenFor5Mins(data.vokiId);
			}, 60_000);
		}
	});
	onDestroy(() => {
		if (refreshTimer) {
			clearInterval(refreshTimer);
		}
	});
</script>

{#if !data.response.isSuccess}
	<VokiNotLoaded errs={data.response.errs} vokiId={data.vokiId!} />
{:else}
	<div class="voki-page-container">
		<div class="main-content">
			<NameSection name={data.response.data.name} />
			<AuthorsSection
				primaryAuthorId={data.response.data.primaryAuthorId}
				coAuthorIds={data.response.data.coAuthorIds}
			/>
			<MainDetailsSection
				type={data.response.data.type}
				language={data.response.data.language}
				hasMatureContent={data.response.data.hasMatureContent}
			/>
			<div class="tabs-section">
				<TabLinksContainer
					currentTab={pageState.currentTab}
					changeTab={(newTab) => (pageState.currentTab = newTab)}
					commentsCount={pageState.commentsCount}
					ratingsCount={pageState.ratingsCount}
				/>
				<div class="current-tab">
					{#if pageState.currentTab === 'about'}
						<VokiPageAboutTab
							vokiId={pageState.vokiId}
							tags={data.response.data.tags}
							description={data.response.data.description}
							publicationDate={data.response.data.publicationDate}
						/>
					{:else if pageState.currentTab === 'comments'}
						<VokiPageCommentsTab />
					{:else if pageState.currentTab === 'ratings'}
						<VokiPageRatingsTab
							tabData={pageState.ratingsTabData}
							fetchTabData={async () => await pageState.fetchRatingsTabData()}
							saveNewUserRating={async (value) => await pageState.saveNewUserRating(value)}
						/>
					{:else}
						<h1>Error</h1>
					{/if}
				</div>
			</div>
		</div>
		<CoverSection
			vokiId={data.response.data.id}
			cover={data.response.data.cover}
			usersWithAccessToManage={[data.response.data.primaryAuthorId]}
			vokiType={data.response.data.type}
			authenticatedOnlyTaking={true}
		/>
	</div>
{/if}

<style>
	.voki-page-container {
		display: grid;
		gap: 1rem;
		width: 100%;
		margin-top: 2rem;
		grid-template-columns: 1fr auto;
	}

	.main-content {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.tabs-section {
		display: flex;
		flex-direction: column;
		padding: 0 0.5rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs);
	}

	.current-tab {
		margin: 0.75rem 0;
	}

	.current-tab > :global(*) {
		animation: 0.25s tab-fade-in;
	}

	@keyframes tab-fade-in {
		from {
			opacity: 0.2;
		}

		to {
			opacity: 1;
		}
	}
</style>
