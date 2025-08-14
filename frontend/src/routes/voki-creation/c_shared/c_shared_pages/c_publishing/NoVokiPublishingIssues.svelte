<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type {
		VokiPublishingIssue,
		VokiSuccessfullyPublishedData
	} from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import { getVokiCreationPageApiService } from '../../../voki-creation-page-context';

	const vokiCreationApi = getVokiCreationPageApiService();
	const {
		vokiId,
		showNewIssuesOnIssuesFound,
		onPublishedSuccessfully
	}: {
		vokiId: string;
		showNewIssuesOnIssuesFound: (issues: VokiPublishingIssue[]) => void;
		onPublishedSuccessfully: (vokiData: VokiSuccessfullyPublishedData) => void;
	} = $props<{
		vokiId: string;
		showNewIssuesOnIssuesFound: (issues: VokiPublishingIssue[]) => void;
		onPublishedSuccessfully: (vokiData: VokiSuccessfullyPublishedData) => void;
	}>();

	async function publishVokiButtonPressed() {
		const response = await vokiCreationApi.publish(vokiId);
		if (response.isSuccess) {
			console.log(response.data);
			if ('issues' in response.data) {
				toast.error('During publishing some publishing issues were found. Please check them');
				showNewIssuesOnIssuesFound(response.data.issues);
			} else {
				onPublishedSuccessfully(response.data);
			}
		} else {
			publishingErrs = response.errs;
		}
	}
	let publishingErrs = $state<Err[]>([]);
</script>

<div class="no-issues-container">
	<label class="no-issues-label"
		>This voki has no publishing issues and is ready to be published</label
	>
	<DefaultErrBlock errList={publishingErrs} />
	<PrimaryButton onclick={() => publishVokiButtonPressed()}>Publish this voki</PrimaryButton>
</div>

<style>
	.no-issues-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 52rem;
		height: 18rem;
		padding: 2rem;
		margin: 0 auto;
		margin-top: 8rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-2xl);
		animation: var(--default-fade-in-animation);
	}
	.no-issues-label {
		font-size: 2rem;
		text-align: center;
		margin-bottom: auto;
		font-weight: 520;
	text-wrap: balance;
	letter-spacing: 0.25px;
	}
	.no-issues-container > :global(.primary-btn) {
		margin-top: 1rem;
	}
</style>
