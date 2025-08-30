<script lang="ts">
	import type { PageProps } from './$types';
	import AuthorsSection from './c_page/c_sections/AuthorsSection.svelte';
	import CoverSection from './c_page/c_sections/CoverSection.svelte';
	import MainDetailsSection from './c_page/c_sections/MainDetailsSection.svelte';
	import NameSection from './c_page/c_sections/NameSection.svelte';
	import VokiPageAboutTab from './c_page/c_tabs/VokiPageAboutTab.svelte';
	import VokiPageCommentsTab from './c_page/c_tabs/VokiPageCommentsTab.svelte';
	import VokiPageRatingsTab from './c_page/c_tabs/VokiPageRatingsTab.svelte';
	import TabLinksContainer from './c_page/TabLinksContainer.svelte';
	import VokiNotLoaded from './c_page/VokiNotLoaded.svelte';
	import { VokiPageState } from './voki-page-state.svelte';

	let { data }: PageProps = $props();
	let pageState = new VokiPageState(
		data.vokiId,
		data.currentTab,
		data.response.isSuccess ? data.response.data.ratingsCount : 0,
		data.response.isSuccess ? data.response.data.commentsCount : 0
	);
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
				isAgeRestricted={data.response.data.isAgeRestricted}
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
						<VokiPageRatingsTab />
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
</style>
