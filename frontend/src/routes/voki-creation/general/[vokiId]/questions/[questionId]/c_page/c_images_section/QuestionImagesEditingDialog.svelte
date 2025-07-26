<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import type { Err } from '$lib/ts/err';
	import GeneralVokiAddQuestionImage from './c_images_dialog/GeneralVokiAddQuestionImage.svelte';

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
		// const response = await vokiCreationApi.updateVokiTags(vokiId, chosenTags);
		// if (response.isSuccess) {
		// 	updateParent(response.data.newTags);
		// 	dialogElement.close();
		// } else {
		// 	errs = response.errs;
		// }
	}
	export function open() {
		errs = [];
		dialogElement.open();
	}
	function addImg(img: string) {}
	function removeImg(img: string) {}
</script>

<DialogWithCloseButton bind:this={dialogElement} dialogId="voki-creation-question-images-dialog">
	<h1 class="subheading">Edit question images</h1>
	<div class="imgs-container">
		{#each images as img}{/each}
		{#if images.length < maxImagesCount}
			<GeneralVokiAddQuestionImage {questionId} {vokiId} addImage={addImg} />
		{/if}
	</div>
	<PrimaryButton onclick={() => saveData()}>Save</PrimaryButton>
</DialogWithCloseButton>
