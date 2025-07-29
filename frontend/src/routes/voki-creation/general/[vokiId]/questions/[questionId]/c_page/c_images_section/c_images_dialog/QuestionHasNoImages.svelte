<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	const {
		questionId,
		vokiId,
		uploadImage
	}: { questionId: string; vokiId: string; uploadImage: (image: File) => Promise<void> } = $props<{
		questionId: string;
		vokiId: string;
		uploadImage: (image: File) => Promise<void>;
	}>();
	let isLoading = $state(false);
	async function handleFile(file: File) {
		if (!file.type.startsWith('image/')) {
			return;
		}
		isLoading = true;
		await uploadImage(file);
		isLoading = false;
	}

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			await handleFile(files[0]);
		}
	}

	async function ondrop(event: DragEvent) {
		event.preventDefault();
		isDragging = false;
		if (event.dataTransfer?.files?.length) {
			await handleFile(event.dataTransfer.files[0]);
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
	class="no-images"
	class:loading={isLoading}
	class:dragging={isDragging}
	ondrop={(e) => ondrop(e)}
	{ondragover}
	{ondragleave}
>
	{#if isLoading}
		<div class="loading-container">
			<CubesLoader sizeRem={5} />
		</div>
	{:else}
		<h1>This question has no images yet</h1>
		<p>Drag and drop image or click the button below</p>

		<label class="upload-button unselectable" class:dragging={isDragging}>
			<svg><use href="#add-image-icon" /></svg>
			<span>Add image</span>
			<input type="file" accept="image/*" onchange={handleInputChange} hidden />
		</label>
	{/if}
</div>

<style>
	.no-images {
		border: 0.125rem dashed var(--muted);
		border-radius: 1rem;
		width: 36rem;
		height: 16rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		text-align: center;
		cursor: pointer;
		transition: all 0.12s ease-in;
	}
	.no-images.dragging:not(.loading) {
		border-color: var(--primary);
		background-color: var(--secondary);
	}
	.loading-container {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}

	.upload-button {
		display: inline-flex;
		width: fit-content;
		align-items: center;
		gap: 0.5rem;
		margin-top: 1rem;
		padding: 0.375rem 1.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		border-radius: 0.375rem;
		cursor: pointer;
		font-weight: 450;
		transition: inherit;
		font-size: 1.125rem;
		letter-spacing: 0.25px;
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
	h1 {
		margin-bottom: 0.5rem;
		font-size: 1.375rem;
		font-weight: 700;
		color: var(--text);
		transition: inherit;
	}
	.no-images.dragging h1 {
		letter-spacing: 0.5px;
	}
	p {
		color: var(--secondary-foreground);
		font-weight: 440;
		margin-bottom: 1rem;
	}
</style>
