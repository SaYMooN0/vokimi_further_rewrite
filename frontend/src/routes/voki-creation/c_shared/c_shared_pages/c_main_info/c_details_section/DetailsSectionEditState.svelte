<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { TextareaAutosize } from 'runed';
	import { getVokiCreationPageApiService } from '../../../../voki-creation-page-context';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import MainInfoSectionSaveAndCancelButtons from '../c_sections_shared/MainInfoSectionSaveAndCancelButtons.svelte';
	import { LanguageUtils } from '$lib/ts/language';
	import DefaulCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';

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
	let isAgeRestricted = $state(details.isAgeRestricted);
	let savingErrs = $state<Err[]>([]);
	const vokiCreationApi = getVokiCreationPageApiService();
	new TextareaAutosize({ element: () => descriptionTextarea, input: () => description });

	async function saveChanges() {
		const response = await vokiCreationApi.updateVokiDetails(vokiId, {
			description,
			language,
			isAgeRestricted
		});
		if (response.isSuccess) {
			updateParent(response.data);
			savingErrs = [];
            cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<VokiCreationFieldName fieldName="Description:" />
<textarea
	class="description-input"
	bind:this={descriptionTextarea}
	bind:value={description}
	name={StringUtils.rndStr()}
/>
<DefaultErrBlock errList={savingErrs} />
<p class="field-p">
	<VokiCreationFieldName fieldName="Language:" />
	<select bind:value={language}>
		{#each LanguageUtils.values() as lang}
			<option value={lang}>{LanguageUtils.name(lang)}</option>
		{/each}
	</select>
</p>
<p class="field-p">
    <VokiCreationFieldName fieldName="Age restriction:" />
    <DefaultCheckBox bind:checked={isAgeRestricted} />
</p>

<MainInfoSectionSaveAndCancelButtons onCancel={cancelEditing} onSave={() => saveChanges()} />

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
</style>
