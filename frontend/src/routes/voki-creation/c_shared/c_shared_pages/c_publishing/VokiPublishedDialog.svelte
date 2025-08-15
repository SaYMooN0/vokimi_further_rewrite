<script lang="ts">
	import BaseDialog from '$lib/components/dialogs/BaseDialog.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiSuccessfullyPublishedData } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiPublishedDIalogConfettiIcon from './c_published_dialog/VokiPublishedDIalogConfettiIcon.svelte';

	let dialog = $state<BaseDialog>()!;
	let vokiData = $state<VokiSuccessfullyPublishedData>();

	export function open(data: VokiSuccessfullyPublishedData) {
		vokiData = data;
		dialog.open();
	}
</script>

<BaseDialog bind:this={dialog} dialogId="voki-published-dialog">
	{#if vokiData}
		<h1 class="title">Congratulations <VokiPublishedDIalogConfettiIcon /></h1>
		<img class="cover" src={StorageBucketMain.fileSrc(vokiData.cover)} />
		<p class="message">
			Voki
			<span>{vokiData!.name}</span>
			has been successfully published
		</p>
		<a href={`/catalog/${vokiData!.id}`} class="view-button">View published Voki page</a>
	{:else}
		<h1 class="title">Error</h1>
		<p class="message">Something went wrong, please try again</p>
	{/if}
</BaseDialog>

<style>
	:global(#voki-published-dialog .dialog-content) {
		width: 38rem;
		min-height: 34rem;
	}

	.title {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		width: 100%;
		color: var(--primary);
		font-size: 2rem;
		font-weight: 500;
		letter-spacing: 0.25px;
	}

	.title > :global(svg) {
		width: 2.5rem;
		height: 2.5rem;
		margin-bottom: 0.25rem;
		color: var(--primary);
		stroke-width: 2;
	}

	.cover {
		height: 18rem;
		margin-top: 2rem;
		border-radius: 0.75rem;
		box-shadow: var(--shadow-xs);
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.message {
		margin: 1rem 0 2rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 450;
		letter-spacing: 0.25px;
	}

	.message > span {
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 520;
		letter-spacing: 0.4px;
		word-break: break-all;
	}

	.view-button {
		padding: 0.25rem 1.25rem;
		margin: 0 auto;
		margin-top: auto;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.5rem;
		font-weight: 450;
		text-align: center;
		letter-spacing: 0.5px;
		transition: all 0.12s ease-in-out;
		cursor: pointer;
	}

	.view-button:hover {
		padding: 0.25rem 1.5rem;
		background-color: var(--primary-hov);
		letter-spacing: 1px;
	}
</style>
