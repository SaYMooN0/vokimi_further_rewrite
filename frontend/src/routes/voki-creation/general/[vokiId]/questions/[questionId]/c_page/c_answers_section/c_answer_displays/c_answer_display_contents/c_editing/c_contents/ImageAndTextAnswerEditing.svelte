<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import type { AnswerDataImageAndText } from '../../../../../../types';
	import AnswerEditingTextArea from './c_shared/AnswerEditingTextArea.svelte';

	interface Props {
		answer: AnswerDataImageAndText;
	}
	let { answer = $bindable() }: Props = $props();
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
			answer.image = response.data;
		} else {
			uploadingErrs = response.errs;
		}
		isLoading = false;
	}
</script>

<div class="answer-content">
	<div class="main">
		<AnswerEditingTextArea bind:text={answer.text} />
		<div class="image-part">
			{#if isLoading}
				<div class="loading">
					<CubesLoader sizeRem={4} />
				</div>
			{:else if answer.image}
				<div class="img-selected">
					<img src={StorageBucketMain.fileSrcWithVersion(answer.image)} />
					<label class="change-img-btn unselectable">
						<span>Change image</span>
						<input type="file" accept="image/*" onchange={handleInputChange} hidden />
					</label>
				</div>
			{:else}
				<label class="upload-button unselectable">
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
		padding: 0 2rem;
	}

	.main {
		display: grid;
		grid-template-columns: 1fr 18rem;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.answer-content > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}
</style>
