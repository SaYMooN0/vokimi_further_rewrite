<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiType } from '$lib/ts/voki-type';
	import GoToVokiCreationLink from '../../../c_shared/GoToVokiCreationLink.svelte';

	interface Props {
		vokiType: VokiType;
		vokiId: string;
		vokiName: string;
		vokiCover: string;
		closeDialog: () => void;
	}

	let { vokiType, vokiId, vokiName, vokiCover, closeDialog }: Props = $props();
</script>

<div class="confirmed-content">
	<img class="cover" src={StorageBucketMain.fileSrc(vokiCover)} />
	<p class="main-text">You now a co-author of <span class="voki-name">{vokiName}</span> Voki</p>
</div>
<div class="buttons unselectable">
	<GoToVokiCreationLink {vokiId} {vokiType} />
	<button class="close-btn" onclick={() => closeDialog()}>Close</button>
</div>

<style>
	.confirmed-content {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		align-items: center;
		width: 100%;
		margin: auto 0;
	}
	.cover {
		max-height: 14rem;
		min-height: 10rem;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		border-radius: var(--voki-cover-border-radius);
		box-shadow: var(--shadow-xs);
	}
	.main-text {
		line-height: 1.375;
		color: var(--text);
		text-indent: 1em;
		font-size: 1.25rem;
		font-weight: 475;
		text-align: justify;
		overflow-wrap: anywhere;
		text-wrap: pretty;
		max-height: 20rem;
		overflow-y: auto;
	}
	.main-text > .voki-name {
		color: var(--muted-foreground);
		font-weight: 500;
		border-radius: 0.5rem;
		font-size: 1.375rem;
		text-indent: 0;
		text-decoration: underline;
		margin-bottom: 1rem;
	}
	.buttons {
		margin-top: auto;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		align-items: center;
	}
	.close-btn {
		font-size: 1rem;
		border: none;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		width: fit-content;
		padding: 0.25rem 2rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		border-radius: 0.25rem;
	}
	.close-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
