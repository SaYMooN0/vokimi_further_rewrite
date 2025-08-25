<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationDefaultButton from '../../../../../c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import QuestionImagesEditingDialog from './c_images_section/QuestionImagesEditingDialog.svelte';

	let { images, questionId, vokiId }: { images: string[]; questionId: string; vokiId: string } =
		$props<{ images: string[]; questionId: string; vokiId: string }>();
	let dialogElement = $state<QuestionImagesEditingDialog>()!;
</script>

<QuestionImagesEditingDialog
	bind:this={dialogElement}
	{questionId}
	{vokiId}
	updateParent={(newImages) => (images = newImages)}
/>
<div class="field">
	<VokiCreationFieldName fieldName="Images:" />
	{#if images.length === 0}
		<FieldNotSetLabel text="No images selected" />
	{/if}
</div>
<VokiCreationDefaultButton text="Edit images" onclick={() => dialogElement.open(images)} />

<style>
	.field {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin-top: 1.5rem;
	}
</style>
