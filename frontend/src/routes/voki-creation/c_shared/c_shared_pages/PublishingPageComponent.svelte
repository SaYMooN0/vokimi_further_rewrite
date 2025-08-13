<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { type VokiPublishingIssue } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { getVokiCreationPageApiService } from '../../voki-creation-page-context';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiPublishingIssuesList from './c_publishing/VokiPublishingIssuesList.svelte';
	import NoVokiPublishingIssues from './c_publishing/NoVokiPublishingIssues.svelte';
	import VokiPublishedDialog from './c_publishing/VokiPublishedDialog.svelte';

	let { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	const vokiCreationApi = getVokiCreationPageApiService();

	let pageState = $state<PageState>({ name: 'Message' });

	type PageState =
		| { name: 'Message' }
		| { name: 'Loading' }
		| { name: 'Error'; errs: Err[] }
		| { name: 'Fetched'; issues: VokiPublishingIssue[] };

	async function loadPublishingIssues() {
		pageState = { name: 'Loading' };
		const response = await vokiCreationApi.checkForPublishingIssues(vokiId);
		if (response.isSuccess) {
			pageState = { name: 'Fetched', issues: response.data.issues };
			console.log(pageState);
		} else {
			pageState = { name: 'Error', errs: response.errs };
			console.log(pageState);
		}
	}
	let vokiPublishedDialog = $state<VokiPublishedDialog>()!;
</script>

<VokiPublishedDialog bind:this={vokiPublishedDialog} />
{#if pageState.name === 'Message'}
	<div class="msg-container">
		<label class="warning-label">Warning</label>
		<p class="warning-text">
			Publishing your Voki will make it public — anyone can view it and take it. After publishing,
			most parts can’t be changed, so make sure it’s ready. Click the button below to run the check.
			We’ll scan your Voki for issues and show you if anything needs fixing.
		</p>
		<PrimaryButton onclick={() => loadPublishingIssues()} class="check-for-issues-btn"
			>Check for issues</PrimaryButton
		>
	</div>
{:else if pageState.name === 'Loading'}
	<div class="msg-container loading">
		<CubesLoader sizeRem={5} />
		<label>Searching for issues</label>
	</div>
{:else if pageState.name === 'Error'}
	<div class="msg-container error">
		<DefaultErrBlock errList={pageState.errs} />
		<PrimaryButton onclick={() => loadPublishingIssues()} class="refetch">Refetch</PrimaryButton>
	</div>
{:else if pageState.issues.length != 0}
	<VokiPublishingIssuesList
		issues={pageState.issues}
		{vokiId}
		refetch={() => loadPublishingIssues()}
		onPublishedSuccessfully={(publishedData) => vokiPublishedDialog.open(publishedData)}
	/>
{:else}
	<NoVokiPublishingIssues
		{vokiId}
		onPublishedSuccessfully={(publishedData) => vokiPublishedDialog.open(publishedData)}
		showNewIssuesOnIssuesFound={(issuesList: VokiPublishingIssue[]) => {
			pageState = { name: 'Fetched', issues: issuesList };
		}}
	/>
{/if}

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 52rem;
		height: 18rem;
		padding: 2rem;
		margin: 0 auto;
		margin-top: 8rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow);
		animation: var(--default-fade-in-animation);
	}

	.warning-label {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 1px;
	}

	.warning-text {
		margin-top: 1rem;
		font-size: 1.25rem;
		font-weight: 420;
		text-align: justify;
		letter-spacing: 0.25px;
		text-indent: 0.5em;
	}

	.msg-container > :global(.check-for-issues-btn) {
		margin-top: auto;
	}

	.msg-container.loading {
		justify-content: center;
		gap: 2rem;
	}

	.msg-container.loading > label {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 1px;
	}

	.msg-container.error {
		border: 0.125rem dashed var(--err-foreground);
		background-color: var(--back);
		box-shadow: var(--err-shadow);
	}

	.msg-container.error > :global(.refetch) {
		margin-top: auto;
	}
</style>
