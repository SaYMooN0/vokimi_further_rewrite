<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import DialogQuestionImageView from './c_images_dialog/DialogQuestionImageView.svelte';
	import GeneralVokiAddQuestionImage from './c_images_dialog/GeneralVokiAddQuestionImage.svelte';
	import QuestionHasNoImages from './c_images_dialog/QuestionHasNoImages.svelte';

	let {
		images,
		questionId,
		vokiId,
		updateParent
	}: {
		images: string[];
		questionId: string;
		vokiId: string;
		updateParent: (images: string[]) => void;
	} = $props<{
		images: string[];
		questionId: string;
		vokiId: string;
		updateParent: (images: string[]) => void;
	}>();

	let dialogElement = $state<DialogWithCloseButton>()!;
	let errs = $state<Err[]>([]);
	const maxImagesCount = 5;
	async function saveData() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ newImages: string[] }>(
			`/vokis/${vokiId}/questions/${questionId}/update-images`,
			RequestJsonOptions.PATCH({ newImages: images })
		);
		if (response.isSuccess) {
			updateParent(response.data.newImages);
			dialogElement.close();
		} else {
			errs = response.errs;
		}
	}
	export function open() {
		errs = [];
		dialogElement.open();
	}
	async function uploadAndAddImg(file: File): Promise<void> {
		const formData = new FormData();
		formData.append('file', file);

		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ imageKey: string }>(
			`/vokis/${vokiId}/questions/${questionId}/upload-image`,
			RequestJsonOptions.POST(formData)
		);
		if (response.isSuccess) {
			images.push(response.data.imageKey);
		} else {
			errs = response.errs;
		}
	}
	function removeImg(img: string) {
		images = images.filter((i) => i != img);
	}
</script>

<DialogWithCloseButton bind:this={dialogElement} dialogId="voki-creation-question-images-dialog">
	<h1 class="subheading">Question images</h1>
	{#if images.length == 0}
		<QuestionHasNoImages {questionId} {vokiId} uploadImage={uploadAndAddImg} />
	{:else}
		<div class="imgs-container">
			{#each images as img}
				<DialogQuestionImageView {img} {removeImg} />
			{/each}
			{#if images.length < maxImagesCount}
				<GeneralVokiAddQuestionImage {questionId} {vokiId} uploadImage={uploadAndAddImg} />
			{/if}
		</div>
	{/if}
	{#if errs.length > 0}
		<DefaultErrBlock errList={errs} />
	{/if}
	<PrimaryButton onclick={() => saveData()}>Save</PrimaryButton>
</DialogWithCloseButton>

<style>
	.subheading {
		margin: 0;
		font-size: 1.75rem;
		font-weight: 650;
		text-align: center;
		margin-bottom: 1rem;
	}
	:global(#voki-creation-question-images-dialog .dialog-content) {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
	:global(#voki-creation-question-images-dialog .dialog-content > .primary-btn) {
		align-self: center;
	}
</style>
