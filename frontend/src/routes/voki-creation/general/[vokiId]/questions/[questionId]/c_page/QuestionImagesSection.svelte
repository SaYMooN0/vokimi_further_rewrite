<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import QuestionImagesEditingDialog from './c_images_section/QuestionImagesEditingDialog.svelte';

	let { images, questionId, vokiId }: { images: string[]; questionId: string; vokiId: string } =
		$props<{ images: string[]; questionId: string; vokiId: string }>();
	let dialogElement = $state<QuestionImagesEditingDialog>()!;
</script>

<QuestionImagesEditingDialog
	bind:this={dialogElement}
	{images}
	{questionId}
	{vokiId}
	updateParent={(newImages) => (images = newImages)}
/>
<div class="question-images-section">
	<VokiCreationFieldName fieldName="Images:" />
	<VokiCreationDefaultButton text="Edit images" onclick={() => dialogElement.open()} />
</div>

<style>
	.question-images-section {
		margin-top: 1.5rem;
	}
</style>
