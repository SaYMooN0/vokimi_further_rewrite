<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationDefaultButton from '../../../../../c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import type { GeneralVokiCreationQuestionImageSet } from '../types';
	import QuestionImagesEditingDialog from './c_images_section/QuestionImagesEditingDialog.svelte';

	interface Props {
		imageSet: GeneralVokiCreationQuestionImageSet;
		questionId: string;
		vokiId: string;
	}
	let { imageSet, questionId, vokiId }: Props = $props();
	let dialogElement = $state<QuestionImagesEditingDialog>()!;
</script>

<QuestionImagesEditingDialog
	bind:this={dialogElement}
	{questionId}
	{vokiId}
	updateParent={(newImageSet) => (imageSet = newImageSet)}
/>
<div class="field">
	<VokiCreationFieldName fieldName="Images:" />
	{#if imageSet.keys.length === 0}
		<FieldNotSetLabel text="No images selected" />
	{/if}
</div>
<VokiCreationDefaultButton text="Edit images" onclick={() => dialogElement.open(imageSet)} />

<style>
	.field {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin-top: 1.5rem;
	}
</style>
