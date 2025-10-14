<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { TextareaAutosize } from 'runed';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import VokiCreationDefaultButton from '../../../../../c_shared/VokiCreationDefaultButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	let {
		text,
		questionId,
		vokiId
	}: {
		text: string;
		questionId: string;
		vokiId: string;
	} = $props<{
		text: string;
		questionId: string;
		vokiId: string;
	}>();

	let textarea = $state<HTMLTextAreaElement>(null!);
	let newText = $state(text);
	let isEditing = $state(false);
	let savingErrs = $state<Err[]>([]);

	new TextareaAutosize({ element: () => textarea, input: () => newText });

	function startEditing() {
		newText = text;
		isEditing = true;
		savingErrs = [];
	}
	async function saveChanges() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ newText: string }>(
			`/vokis/${vokiId}/questions/${questionId}/update-text`,
			RJO.PATCH({ newText: newText })
		);
		if (response.isSuccess) {
			text = response.data.newText;
			isEditing = false;
			savingErrs = [];
		} else {
			savingErrs = response.errs;
		}
	}
</script>

{#if isEditing}
	<VokiCreationFieldName fieldName="Question text:" />

	<textarea
		class="text-input"
		bind:this={textarea}
		bind:value={newText}
		name={StringUtils.rndStr()}
	/>
	{#if savingErrs.length > 0}
		<DefaultErrBlock errList={savingErrs} className="question-text-err-block" />
	{/if}
	<VokiCreationSaveAndCancelButtons
		onCancel={() => (isEditing = false)}
		onSave={() => saveChanges()}
	/>
{:else}
	<p class="question-text-p">
		<VokiCreationFieldName fieldName="Question text:" />
		<label class="question-text-value">{text}</label>
	</p>
	<VokiCreationDefaultButton text="Edit text" onclick={startEditing} />
{/if}

<style>
	.text-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.375rem;
		margin-top: 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.25rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.text-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.text-input:focus {
		outline-color: var(--primary);
	}

	:global(.err-block.question-text-err-block) {
		margin-top: 0.5rem;
	}

	.question-text-p {
		width: 100%;
	}

	.question-text-value {
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
		text-decoration: none;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
