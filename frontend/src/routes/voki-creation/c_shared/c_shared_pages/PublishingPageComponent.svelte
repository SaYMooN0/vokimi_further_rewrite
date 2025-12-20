<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { type VokiPublishingIssue } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { getVokiCreationPageContext } from '../../voki-creation-page-context';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiPublishingIssuesList from './c_publishing/VokiPublishingIssuesList.svelte';
	import NoVokiPublishingIssues from './c_publishing/NoVokiPublishingIssues.svelte';
	import VokiPublishedDialog from './c_publishing/ConfirmVokiPublishingDialog.svelte';
	import ConfirmVokiPublishingDialog from './c_publishing/ConfirmVokiPublishingDialog.svelte';
	interface Props {
		vokiId: string;
		initialIssues: VokiPublishingIssue[];
		primaryAuthorId: string;
		coAuthorIds: string[];
	}
	let { vokiId, initialIssues, primaryAuthorId, coAuthorIds }: Props = $props();
	const vokiCreationCtx = getVokiCreationPageContext();

	let pageState = $state<PageState>({ name: 'ok', issues: initialIssues });

	type PageState =
		| { name: 'loading' }
		| { name: 'error'; errs: Err[] }
		| { name: 'ok'; issues: VokiPublishingIssue[] };

	async function loadPublishingIssues() {
		pageState = { name: 'loading' };
		const response = await vokiCreationCtx.vokiCreationApi.checkForPublishingIssues(vokiId);
		if (response.isSuccess) {
			pageState = { name: 'ok', issues: response.data.issues };
		} else {
			pageState = { name: 'error', errs: response.errs };
		}
	}
	let confirmVokiPublishedDialog = $state<ConfirmVokiPublishingDialog>()!;
</script>

<ConfirmVokiPublishingDialog bind:this={confirmVokiPublishedDialog} />

{#if pageState.name === 'loading'}
	<div class="msg-container loading">
		<CubesLoader sizeRem={5} color="var(--primary)" />
		<label>Searching for issues</label>
	</div>
{:else if pageState.name === 'error'}
	<div class="msg-container error">
		<DefaultErrBlock errList={pageState.errs} />
		<PrimaryButton onclick={() => loadPublishingIssues()} class="refetch">Refetch</PrimaryButton>
	</div>
{:else if pageState.name === 'ok' && pageState.issues.length != 0}
	<VokiPublishingIssuesList
		issues={pageState.issues}
		{vokiId}
		refetch={() => loadPublishingIssues()}
		onPublishedSuccessfully={(publishedData) => vokiPublishedDialog.open(publishedData)}
	/>
{:else if pageState.name === 'ok' && pageState.issues.length === 0}
	<NoVokiPublishingIssues
		{vokiId}
		onPublishedSuccessfully={(publishedData) => vokiPublishedDialog.open(publishedData)}
		showNewIssuesOnIssuesFound={(issuesList: VokiPublishingIssue[]) => {
			pageState = { name: 'ok', issues: issuesList };
		}}
	/>
{:else}
	<h2>Something went wrong. Please refresh the page</h2>
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
