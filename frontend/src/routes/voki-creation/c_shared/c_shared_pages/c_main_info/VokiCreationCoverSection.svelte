<script lang="ts">
	import { toast } from 'svelte-sonner';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { getVokiCreationPageApiService } from '../../../voki-creation-page-context';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	let { cover, vokiId }: { cover: string; vokiId: string } = $props<{
		cover: string;
		vokiId: string;
	}>();
	let version = $state(0);
	async function handleImageInputChange(event: Event) {
		const input = event.target as HTMLInputElement;
		if (input.files && input.files.length > 0) {
			isLoading = true;
			const response = await vokiCreationApi.updateVokiCover(vokiId, input.files[0]);
			if (response.isSuccess) {
				cover = response.data.newCover;
				version++;
			} else {
				toast.error("Couldn't update voki cover");
			}
			isLoading = false;
		}
	}
	const vokiCreationApi = getVokiCreationPageApiService();

	async function changeImageToDefault() {
		isLoading = true;
		const response = await vokiCreationApi.setVokiCoverToDefault(vokiId);
		if (response.isSuccess) {
			cover = response.data.newCover;
		} else {
			toast.error("Couldn't set voki cover to default");
		}
		isLoading = false;
	}
	let isLoading = $state(false);
</script>

<div class="img-container">
	{#if isLoading}
		<div class="loading-container">
			<CubesLoader sizeRem={5} />
			<label>Please wait</label>
		</div>
	{:else}
		<img src={StorageBucketMain.fileSrcWithVersion(cover, version)} alt="voki cover" />
		<label for="voki-cover-input" class="img-btn change-btn">Change cover</label>
		<input
			type="file"
			id="voki-cover-input"
			accept=".jpg,.png,.webp"
			hidden
			onchange={handleImageInputChange}
		/>
		<button class="img-btn set-default-btn" onclick={changeImageToDefault}>Set to default </button>
	{/if}
</div>

<style>
	.img-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 30rem;
	}
	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		animation: var(--default-fade-in-animation);
	}
	.loading-container > label {
		color: var(--secondary-foreground);
		font-size: 1.5rem;
		font-weight: 570;
		letter-spacing: 0.5px;
		white-space: nowrap;
	}

	.img-container img {
		width: 100%;
		border-radius: 1rem;
		object-fit: fill;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		animation: var(--default-fade-in-animation);
	}

	.img-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		font-weight: 420;
		letter-spacing: 0.2px;
		box-shadow: var(--shadow);
		cursor: pointer;
	}

	.change-btn {
		margin-top: 1rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.set-default-btn {
		margin-top: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.set-default-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
