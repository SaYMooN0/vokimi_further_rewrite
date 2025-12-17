<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { TextareaAutosize } from 'runed';
	import { getVokiCreationPageContext } from '../../../../voki-creation-page-context';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { LanguageUtils } from '$lib/ts/language';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import DefaultLanguageInput from '$lib/components/inputs/DefaultLanguageSelect.svelte';
	import DefaultLanguageSelect from '$lib/components/inputs/DefaultLanguageSelect.svelte';

	const {
		vokiId,
		details,
		updateParent,
		cancelEditing
	}: {
		vokiId: string;
		details: VokiDetails;
		updateParent: (details: VokiDetails) => void;
		cancelEditing: () => void;
	} = $props<{
		vokiId: string;
		details: VokiDetails;
		updateParent: (details: VokiDetails) => void;
		cancelEditing: () => void;
	}>();

	let descriptionTextarea = $state<HTMLTextAreaElement>(null!);
	let description = $state(details.description);
	let language = $state(details.language);
	let hasMatureContent = $state(details.hasMatureContent);
	let savingErrs = $state<Err[]>([]);
	const vokiCreationCtx = getVokiCreationPageContext();

	async function saveChanges() {
		const response = await vokiCreationCtx.vokiCreationApi.updateVokiDetails(vokiId, {
			description,
			language,
			hasMatureContent
		});
		if (response.isSuccess) {
			updateParent(response.data);
			savingErrs = [];
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
	new TextareaAutosize({ element: () => descriptionTextarea, input: () => description });
</script>

<VokiCreationFieldName fieldName="Description:" />
<textarea
	class="description-input"
	bind:this={descriptionTextarea}
	bind:value={description}
	name={StringUtils.rndStr()}
/>
<p class="field-p">
	<VokiCreationFieldName fieldName="Language:" />
	<DefaultLanguageSelect bind:value={language} />
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Mature content:" />
	<DefaultCheckBox bind:checked={hasMatureContent} />
</p>
<DefaultErrBlock errList={savingErrs} />
<VokiCreationSaveAndCancelButtons onCancel={cancelEditing} onSave={() => saveChanges()} />

<style>
	.description-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.125rem 0.375rem;
		margin-top: 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.description-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.description-input:focus {
		outline-color: var(--primary);
	}

	.field-p {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		margin-top: 1rem;
	}
</style>
