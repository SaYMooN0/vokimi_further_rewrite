<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import DefaultTakingCurrentQuestionView from './c_default_taking/DefaultTakingCurrentQuestionView.svelte';
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakingData, GeneralVokiTakingResultData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(takingData);
	let receivedResult: GeneralVokiTakingResultData | null = $state(null);
	let navigationErrs = $state<Err[]>([]);
	let isButtonLoading = $state(false);
	async function onFinishButtonClicked() {
		isButtonLoading = true;
		const result = await vokiTakingState.finishTakingAndReceiveResult();
		if (result.isSuccess) {
			navigationErrs = [];
			receivedResult = result.data;
		} else {
			navigationErrs = result.errs;
		}
		isButtonLoading = false;
	}
	async function onNextButtonClicked() {
		isButtonLoading = true;
		navigationErrs = await vokiTakingState.goToNextQuestion();
		isButtonLoading = false;
	}
</script>

{#if receivedResult === null}
	{#if vokiTakingState.currentQuestion}
		{JSON.stringify(vokiTakingState.currentQuestion)}
		{#if vokiTakingState.isCurrentQuestionLast()}
			<button class:loading={isButtonLoading} onclick={() => onFinishButtonClicked()}>Finish</button
			>
		{:else}
			<button class:loading={isButtonLoading} onclick={() => onNextButtonClicked()}
				>Go to next question</button
			>
		{/if}
	{:else}
		<h1>Question error</h1>
	{/if}
{:else}
	<GeneralVokiReceivedResultView
		result={receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
