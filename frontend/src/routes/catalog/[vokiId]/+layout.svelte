<script lang="ts">
	import type { LayoutProps } from './$types';
	import AuthorsSection from './c_layout/c_sections/AuthorsSection.svelte';
	import CoverSection from './c_layout/c_sections/CoverSection.svelte';
	import MainDetailsSection from './c_layout/c_sections/MainDetailsSection.svelte';
	import NameSection from './c_layout/c_sections/NameSection.svelte';
	import VokiNotLoaded from './c_layout/VokiNotLoaded.svelte';

	let { data, children }: LayoutProps = $props();
</script>

{#if !data.response.isSuccess}
	<VokiNotLoaded errs={data.response.errs} vokiId={data.vokiId!} />
{:else}
	<div class="voki-layout-container">
		

		<div class="main-content-container">
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
			<div class="page-content-container">
				{@render children()}
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
	.voki-layout-container {
		width: 100%;
		display: grid;
		grid-template-columns:  1fr auto;
		gap: 0.5rem;
		margin-top: 1rem;
	}
</style>
