<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';

	interface Props {
		image: string;
		setUploadingErrs: (errs: Err[]) => void;
		vokiId: string;
		questionId: string;
		answerId: string;
	}
	let { image = $bindable(), setUploadingErrs, vokiId, questionId, answerId }: Props = $props();
	let isLoading = $state(false);
	async function uploadImage(file: File) {
		if (!file.type.startsWith('image/')) {
			return;
		}
		isLoading = true;
		setUploadingErrs([]);
		const formData = new FormData();
		formData.append('file', file);
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			image = response.data;
		} else {
			setUploadingErrs(response.errs);
		}
		isLoading = false;
	}

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			await uploadImage(files[0]);
		}
	}

	async function ondrop(event: DragEvent) {
		event.preventDefault();
		isDragging = false;
		if (event.dataTransfer?.files?.length) {
			await uploadImage(event.dataTransfer.files[0]);
		}
	}

	function ondragover(event: DragEvent) {
		event.preventDefault();
		isDragging = Array.from(event.dataTransfer?.items || []).some((item) =>
			item.type.startsWith('image/')
		);
	}

	function ondragleave() {
		isDragging = false;
	}
	let isDragging = $state(false);
</script>

<div
	class="color-input-container"
	class:loading={isLoading}
	class:dragging={isDragging}
	ondrop={(e) => ondrop(e)}
	{ondragover}
	{ondragleave}
>
	{#if isLoading}
		<CubesLoader sizeRem={4} />
	{:else}
		<p>Drag and drop image or click the button below</p>
		<label class="upload-button unselectable" class:dragging={isDragging}>
			<svg><use href="#add-image-icon" /></svg>
			<span>Add image</span>
			<input type="file" accept="image/*" onchange={handleInputChange} hidden />
		</label>
	{/if}
</div>

<style>
	.color-input-container {
		width: 100%;
		min-height: 8rem;
		max-height: 40rem;
		height: 100%;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		border: 0.125rem dashed var(--muted);
		border-radius: 1rem;
		text-align: center;
		gap: 0.75rem;
		transition: all 0.12s ease-in;
	}
	.color-input-container.loading {
		background-color: var(--secondary);
		border: none;
	}

	.color-input-container.dragging:not(.loading) {
		border-color: var(--primary);
		background-color: var(--secondary);
	}

	.color-input-container.loading,
	.color-input-container:not(.loading) {
		animation: var(--default-fade-in-animation);
	}

	.upload-button {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.375rem 1.25rem;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.25px;
		transition: inherit;
		cursor: pointer;
	}

	.upload-button svg {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 2;
		fill: var(--primary-foreground);
	}

	.upload-button:hover {
		background-color: var(--primary-hov);
	}

	.upload-button.dragging {
		background-color: var(--primary-hov);
		transform: scale(1.06);
	}

	p {
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 500;
	}
</style>
