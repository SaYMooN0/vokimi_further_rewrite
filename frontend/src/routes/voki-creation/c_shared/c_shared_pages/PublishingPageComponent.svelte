<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import {
		ApiVokiCreationGeneral,
		type VokiPublishingIssue
	} from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import { getVokiCreationPageApiService } from '../../voki-creation-page-context';

	let { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	const vokiCreationApi = getVokiCreationPageApiService();

	let state = $state<PageState>({ name: 'Message' });

	type PageState =
		| { name: 'Message' }
		| { name: 'Loading' }
		| { name: 'Error'; errs: Err[] }
		| { name: 'Fetched'; issues: VokiPublishingIssue[] };
	async function publishBtnPressed() {
		if (state.name != 'Message') {
			toast.error('Please reload the page and try again');
		}
		state = { name: 'Loading' };
		const response = await vokiCreationApi.checkForPublishingIssues(vokiId);
		if (response.isSuccess) {
			state = { name: 'Fetched', issues: response.data.issues };
		}
	}
</script>

{#if state.name === 'Message'}
	<div class="msg-container">
		<label class="warning-label">Warning</label>
		<p class="warning-text">
			Publishing your Voki will make it public — anyone can view it and take it. After publishing,
			most parts can’t be changed, so make sure it’s ready. Click the button below to run the check.
			We’ll scan your Voki for issues and show you if anything needs fixing.
		</p>
		<PrimaryButton onclick={() => publishBtnPressed()} class="check-for-issues-btn"
			>Check for issues</PrimaryButton
		>
	</div>
{:else if state.name === 'Loading'}
	<div class="msg-container loading">
		<CubesLoader sizeRem={5} />
		<label>Searching for issues</label>
	</div>
{/if}

<style>
	.msg-container {
		margin: 0 auto;
		margin-top: 8rem;
		display: flex;
		align-items: center;
		flex-direction: column;
		border-radius: 1rem;
		box-shadow: var(--shadow);
		background-color: var(--secondary);
		height: 18rem;
		width: 52rem;
		animation: var(--default-fade-in-animation);
		padding: 2rem;
	}
	.warning-label {
		font-weight: 550;
		letter-spacing: 1px;
		font-size: 1.75rem;
		color: var(--secondary-foreground);
	}

	.warning-text {
		font-size: 1.25rem;
		text-indent: 0.5em;
		margin-top: 1rem;
		font-weight: 420;
		letter-spacing: 0.25px;
		text-align: justify;
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
</style>
