<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	const { uploadImage }: { uploadImage: (image: File) => Promise<void> } = $props<{
		uploadImage: (image: File) => Promise<void>;
	}>();

	async function handleInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files?.length) {
			const file = files?.[0];
			if (!file.type.startsWith('image/')) {
				return;
			}
			isLoading = true;
			await uploadImage(file);
			isLoading = false;
		}
	}
	let isLoading = $state(false);
</script>

{#if isLoading}
	<div class="loading-container">
		<CubesLoader sizeRem={4} />
		<h1>Uploading image...</h1>
	</div>
{/if}
<label class="upload-button unselectable">
	<svg><use href="#add-image-icon" /></svg>
	<span>Add image</span>
	<input type="file" accept="image/*" onchange={handleInputChange} hidden />
</label>

<style>
	.loading-container {
		aspect-ratio: 1/1;
		padding: 0 1rem;
		background-color: var(--secondary);
		animation: fade-in 0.06s ease-in-out forwards;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		border-radius: 1rem;
	}
	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		font-weight: 550;
		white-space: nowrap;
		letter-spacing: 0.5px;
		margin: 1rem 0 2rem 0;
	}
	.upload-button {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		padding: 0 1rem;
		height: 10rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		border-radius: 0.375rem;
		cursor: pointer;
		font-weight: 450;
		transition: inherit;
		font-size: 1.25rem;
		letter-spacing: 0.25px;
		white-space: nowrap;
	}
	.upload-button svg {
		height: 3rem;
		width: 3rem;
		stroke-width: 1.5;
	}
	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
</style>
