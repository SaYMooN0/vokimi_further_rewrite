<script lang="ts">
	import { getVokiCreationPageApiService } from '../../../voki-creation-page-context';
	import type {
		VokiPublishingIssue,
		VokiSuccessfullyPublishedData
	} from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import VokiCreationBasicHeader from '../../VokiCreationBasicHeader.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';

	const {
		issues,
		refetch,
		vokiId,
		onPublishedSuccessfully
	}: {
		issues: VokiPublishingIssue[];
		refetch: () => void;
		vokiId: string;
		onPublishedSuccessfully: (data: VokiSuccessfullyPublishedData) => void;
	} = $props<{
		issues: VokiPublishingIssue[];
		refetch: () => void;
		vokiId: string;
		onPublishedSuccessfully: (data: VokiSuccessfullyPublishedData) => void;
	}>();
	const problems = issues.filter((issue) => issue.type === 'Problem');
	const warnings = issues.filter((issue) => issue.type === 'Warning');
	const vokiCreationApi = getVokiCreationPageApiService();

	async function ignoreWarningsAndPublish() {
		const response = await vokiCreationApi.publishWithWarningsIgnored(vokiId);
		if (response.isSuccess) {
			onPublishedSuccessfully(response.data);
		} else {
			errs = response.errs;
		}
	}
	let errs = $state<Err[]>([]);
</script>

<VokiCreationBasicHeader header={`Voki publishing issues (${issues.length})`} />
<ReloadButton onclick={() => refetch()} />
<div class="all-issues-container">
	{#each problems as problem}
		<div class="issue problem">
			<div class="type">
				<svg><use href="#error-icon" /></svg>
				Problem
			</div>
			<div class="source">{problem.source}</div>
			<div class="message">{problem.message}</div>
			<div class="recommendation">{problem.fixRecommendation}</div>
		</div>
	{/each}

	{#each warnings as warning}
		<div class="issue warning">
			<div class="type">
				<svg><use href="#warning-icon" /></svg>
				Warning
			</div>
			<div class="source">{warning.source}</div>
			<div class="message">{warning.message}</div>
			<div class="recommendation">{warning.fixRecommendation}</div>
		</div>
	{/each}
</div>
<DefaultErrBlock class="publishing-err-block" errList={errs} />
{#if problems.length > 0}
	<p class="fix-msg">Please fix all problems before publishing</p>
{:else if warnings.length > 0}
	<button class="ignore-and-publish-btn" onclick={() => ignoreWarningsAndPublish()}
		>Ignore warnings and publish</button
	>
{:else}
	<ErrView err={{ message: 'An error has occurred, please reload the page' }} />
{/if}

<style>
	.all-issues-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		max-height: 68vh;
		overflow-y: auto;
	}

	.issue {
		display: grid;
		align-items: center;
		width: 100%;
		grid-template-columns: auto 12rem 1fr 1fr;
	}

	.problem > .type {
		background-color: var(--err-back);
		color: var(--err-foreground);
	}

	.warning > .type {
		background-color: var(--warn-back);
		color: var(--warn-foreground);
	}

	.type {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		width: 100%;
		width: 8rem;
		padding: 0.25rem 0.5rem;
		border-radius: 8rem;
		font-size: 1.125rem;
		font-weight: 400;
	}

	.type > svg {
		width: 1.25rem;
		height: 1.25rem;
		color: inherit;
		stroke-width: 2;
	}

	.fix-msg {
		width: fit-content;
		padding: 0.25rem 1.75rem;
		margin: 1rem auto 0.25rem;
		border-radius: 1rem;
		background-color: var(--accent);
		color: var(--accent-foreground);
		font-size: 1.25rem;
		font-weight: 500;
		letter-spacing: 0.25px;
	}

	:global(.publishing-err-block) {
		margin-top: 1rem;
	}

	.ignore-and-publish-btn {
		display: block;
		width: fit-content;
		padding: 0.125rem 0.375rem;
		margin: 1rem auto;
		border: none;
		border-radius: 1rem;
		border-radius: 0;
		background-color: transparent;
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 500;
		transition: all 0.08s ease-in-out;
		border-bottom: 0.125rem solid var(--muted-foreground);
	}

	.ignore-and-publish-btn:hover {
		padding: 0.125rem 1rem;
		border-color: var(--primary);
		color: var(--primary);
		letter-spacing: 0.25px;
		cursor: pointer;
	}
</style>
