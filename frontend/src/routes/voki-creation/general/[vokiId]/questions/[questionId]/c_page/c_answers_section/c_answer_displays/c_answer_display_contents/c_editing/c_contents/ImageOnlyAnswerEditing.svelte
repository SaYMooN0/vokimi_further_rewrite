<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import type { AnswerDataImageOnly } from '../../../../../../types';

	interface Props {
		answer: AnswerDataImageOnly;
	}
	let { answer = $bindable() }: Props = $props();
	let isLoading = $state(false);
	let isDragging = $state(false);
	let uploadingErrs = $state<Err[]>([]);

	async function uploadImage(file: File) {
		if (!file.type.startsWith('image/')) {
			return;
		}
		isLoading = true;
		uploadingErrs = [];
		const formData = new FormData();
		formData.append('file', file);
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			answer.image = response.data;
		} else {
			uploadingErrs = response.errs;
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
</script>

<div class="answer-content">
	{#if isLoading}
		<div class="loading">
			<CubesLoader sizeRem={4} />
		</div>
	{:else if answer.image}
		<div class="img-selected">
			<label class="change-img-btn unselectable">
				<span>Change image</span>
				<input type="file" accept="image/*" onchange={handleInputChange} hidden />
			</label>
			<img src={StorageBucketMain.fileSrcWithVersion(answer.image)} />
		</div>
	{:else}
		<div
			class="img-input-container"
			class:dragging={isDragging}
			ondrop={(e) => ondrop(e)}
			{ondragover}
			{ondragleave}
		>
			<p>Drag and drop image or click the button below</p>
			<label class="upload-button unselectable" class:dragging={isDragging}>
				<svg><use href="#add-image-icon" /></svg>
				<span>Add image</span>
				<input type="file" accept="image/*" onchange={handleInputChange} hidden />
			</label>
		</div>
	{/if}
	{#if uploadingErrs.length > 0}
		<DefaultErrBlock errList={uploadingErrs} />
	{/if}
</div>

<style>
	.answer-content {
		width: 100%;
		height: 100%;
		padding: 0 2rem;
		display: flex;
		align-items: center;
		flex-direction: column;
	}

	.answer-content > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}
	.img-selected {
		width: 100%;
		height: 100%;
		display: flex;
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 1rem;
	}
	img {
		object-fit: contain;
		max-height: 16rem;
		max-width: 28rem;
		border-radius: 0.5rem;
	}
	.change-img-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
		border-radius: 0.5rem;
		padding: 0.375rem 0.75rem;
		text-align: center;
		font-size: 1.125rem;
		font-weight: 450;
		height: fit-content;
	}
	.change-img-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.loading,
	.img-input-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;

		width: 100%;
		height: 100%;
		min-height: 8rem;
		max-height: 40rem;
		animation: var(--default-fade-in-animation);
	}

	.img-input-container {
		gap: 0.75rem;
		border: 0.125rem dashed var(--muted);
		border-radius: 1rem;
		text-align: center;
		transition: all 0.12s ease-in;
	}

	.loading {
		background-color: var(--secondary);
	}

	.img-input-container.dragging {
		border-color: var(--primary);
		background-color: var(--secondary);
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
		margin: 0 1rem;
	}
</style>
