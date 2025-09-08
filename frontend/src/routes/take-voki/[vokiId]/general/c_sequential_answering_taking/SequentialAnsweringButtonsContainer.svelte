<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import type { GeneralVokiTakingResultData } from '../types';
	import type { SequentialAnsweringGeneralVokiTakingState } from './sequential-answering-general-voki-taking-state.svelte';
	interface Props {
		vokiTakingState: SequentialAnsweringGeneralVokiTakingState;
		showResultOnVokiTakingFinished: (result: GeneralVokiTakingResultData) => void;
	}
	let { vokiTakingState, showResultOnVokiTakingFinished }: Props = $props();
	let navigationErrs = $state<Err[]>([]);
	let isButtonLoading = $state(false);
	async function onFinishButtonClicked() {
		isButtonLoading = true;
		const result = await vokiTakingState.finishTakingAndReceiveResult();
		if (result.isSuccess) {
			navigationErrs = [];
			showResultOnVokiTakingFinished(result.data);
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

{#if vokiTakingState.isCurrentQuestionLast()}
	<button class:loading={isButtonLoading} onclick={() => onFinishButtonClicked()}>Finish</button>
{:else}
	<button class:loading={isButtonLoading} onclick={() => onNextButtonClicked()}
		>Go to next question</button
	>
{/if}
