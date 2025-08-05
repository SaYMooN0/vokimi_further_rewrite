<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';

	let {
		image = $bindable(),
		errs = $bindable(),
		vokiId,
		resultId
	}: { image: string | null; errs: Err[]; vokiId: string; resultId: string } = $props<{
		image: string | null;
		errs: Err[];
		vokiId: string;
		resultId: string;
	}>();
	async function handleInputChange(event: Event) {
		errs = [];
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			const file = files?.[0];
			if (!file.type.startsWith('image/')) {
				return;
			}
			isLoadingImage = true;
			const formData = new FormData();
			formData.append('file', file);
			const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ imageKey: string }>(
				`/vokis/${vokiId}/results/${resultId}/upload-image`,
				{ method: 'POST', body: formData }
			);
			if (response.isSuccess) {
				image = response.data.imageKey;
			} else {
				errs = response.errs;
			}
			isLoadingImage = false;
		}
	}
	async function removeResultImage() {}
	let isLoadingImage = $state(false);
</script>

{#if isLoadingImage}
	<div class="loading-container">
		<CubesLoader sizeRem={3} />
	</div>
{:else if image}
	<img class="result-image" src={StorageBucketMain.fileSrc(image)} alt="result" />
	<label for="result-image-{resultId}" class="img-btn change-btn unselectable">Change image</label>
	<input
		type="file"
		id="result-image-{resultId}"
		accept=".jpg,.png,.webp"
		hidden
		onchange={handleInputChange}
	/>
	<button class="img-btn remove-btn unselectable" onclick={() => removeResultImage()}
		>Remove image</button
	>
{:else}
	<label class="upload-button unselectable">
		<svg><use href="#add-image-icon" /></svg>
		<span>Add image</span>
		<input type="file" accept=".jpg,.png,.webp" onchange={handleInputChange} hidden />
	</label>
{/if}

<style>
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
	.upload-button {
		height: 6rem;
		width: 6rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		gap: 0.25rem;
		font-size: 1rem;
		border: none;
		border-radius: 0.75rem;
		color: var(--secondary-foreground);
        font-weight:450;
	}
	.upload-button > svg {
		height: 2rem;
		width: 2rem;
		color: inherit;
        stroke-width: 1.3;
	}
	.upload-button:hover {
		color: var(--accent-foreground);
		background-color: var(--accent);
		box-shadow: none;
	}
</style>
