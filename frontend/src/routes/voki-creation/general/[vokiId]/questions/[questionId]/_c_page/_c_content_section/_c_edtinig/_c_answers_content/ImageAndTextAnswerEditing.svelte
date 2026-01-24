<script lang="ts">
	import { watch } from 'runed';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import type { AnswerDataImageAndText } from '../../../../types';
	import GeneralVokiCreationAnswerDisplayImage from '../../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';

	interface Props {
		answer: AnswerDataImageAndText;
		updateOnChange: (newAnswer: AnswerDataImageAndText) => void;
	}
	let { answer, updateOnChange }: Props = $props();
	let text = $state(answer.text);
	let image = $state(answer.image);
	watch(
		() => [text, image],
		() => {
			updateOnChange({ ...answer, text, image });
		}
	);
	let uploadingErrs = $state<Err[]>([]);
	let isLoading = $state(false);

	async function handleInputChange(event: Event) {
		uploadingErrs = [];
		const files = (event.target as HTMLInputElement).files;
		if (!files || files?.length == 0) {
			uploadingErrs = [{ message: 'No file selected' }];
			return;
		}
		const file = files?.[0];
		if (!file.type.startsWith('image/')) {
			uploadingErrs = [{ message: 'Selected file is not an image' }];
			return;
		}
		isLoading = true;
		const formData = new FormData();
		formData.append('file', file);
		const response = await StorageBucketMain.uploadTempImage(file);
		if (response.isSuccess) {
			image = response.data;
		} else {
			uploadingErrs = response.errs;
		}
		isLoading = false;
	}
</script>

<div class="answer-content">
	<div class="main">
		<AnswerEditingTextArea bind:text />
		<div class="image-part">
			{#if isLoading}
				<CubesLoader sizeRem={4} color="var(--primary)" />
			{:else if image}
				<GeneralVokiCreationAnswerDisplayImage src={image} maxWidth={20} />
				<label class="img-button unselectable">
					<span>Change image</span>
					<input type="file" accept="image/*" onchange={handleInputChange} hidden />
				</label>
			{:else}
				<label class="img-button unselectable">
					<svg><use href="#add-image-icon" /></svg>
					<span>Add image</span>
					<input type="file" accept="image/*" onchange={handleInputChange} hidden />
				</label>
			{/if}
		</div>
	</div>
	{#if uploadingErrs.length > 0}
		<DefaultErrBlock errList={uploadingErrs} />
	{/if}
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
		height: 100%;
		padding: 0;
	}

	.main {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.image-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		min-width: 12rem;
		transition:
			height 0.12s ease,
			width 0.12s ease;
	}

	.img-button {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		padding: 0.375rem 0;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 420;
		text-align: center;
		cursor: pointer;
	}

	.img-button > svg {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}

	.img-button:hover {
		background-color: var(--primary-hov);
	}

	.answer-content > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}
</style>
