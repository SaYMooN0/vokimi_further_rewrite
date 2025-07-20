<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { GeneralVokiAnswerType } from '$lib/ts/voki';

	let { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let selectedAnswersType = $state<GeneralVokiAnswerType>('TextOnly');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
	}
	async function submitCreate() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ id: string }>(
			`/vokis/${vokiId}/questions/add-new`,
			RequestJsonOptions.POST({ questionAnswersType: selectedAnswersType })
		);
		if (response.isSuccess) {
			goto(`/voki-creation/general/${vokiId}/questions/${response.data.id}`);
		} else {
			errs = response.errs;
		}
	}
</script>

<DialogWithCloseButton dialogId="general-voki-question-initializing-dialog" bind:this={dialog}>
	<h1 class="subheading">Choose new answers type for the new question</h1>
	<div class="types-container">
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'TextOnly'}
			onclick={() => (selectedAnswersType = 'TextOnly')}
		>
			Text only
		</div>
		<div></div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'ImageOnly'}
			onclick={() => (selectedAnswersType = 'ImageOnly')}
		>
			Image only
		</div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'ImageAndText'}
			onclick={() => (selectedAnswersType = 'ImageAndText')}
		>
			ImageAndText
		</div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'ColorOnly'}
			onclick={() => (selectedAnswersType = 'ColorOnly')}
		>
			Color only
		</div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'ColorAndText'}
			onclick={() => (selectedAnswersType = 'ColorAndText')}
		>
			Color and Text
		</div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'AudioOnly'}
			onclick={() => (selectedAnswersType = 'AudioOnly')}
		>
			Audio only
		</div>
		<div
			class="type-card"
			class:selected={selectedAnswersType === 'AudioAndText'}
			onclick={() => (selectedAnswersType = 'AudioAndText')}
		>
			Audio and Text
		</div>
	</div>
	<DefaultErrBlock errList={errs} />

	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>

<style>
	.subheading {
		width: 100%;
		color: var(--text);
		text-align: center;
		font-size: 1.75rem;
		font-weight: 550;
	}
	.types-container {
		display: grid;
		grid-template-columns: auto auto auto auto;
		grid-template-rows: 3rem 3rem;
		gap: 2rem;
		grid-auto-flow: column;
	}
	.type-card {
		text-align: center;
		border-radius: 0.5rem;
		font-weight: bold;
		cursor: pointer;
		transition: background-color 0.2s ease;
		background-color: var(--secondary);
	}
	.type-card.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
