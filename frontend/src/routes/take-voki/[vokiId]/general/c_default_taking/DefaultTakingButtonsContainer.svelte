<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import type { GeneralVokiTakingResultData } from '../types';
	import type { DefaultGeneralVokiTakingState } from './default-general-voki-taking-state.svelte';
	interface Props {
		vokiTakingState: DefaultGeneralVokiTakingState;
		vokiTakingErrs: (Err & { questionOrder?: number })[];
		onResultReceived: (receivedResult: GeneralVokiTakingResultData) => void;
	}
	let { vokiTakingState, vokiTakingErrs = $bindable(), onResultReceived }: Props = $props();

	let isNextBtnInactive = $derived(vokiTakingState.isCurrentQuestionLast());
	let isPrevBtnInactive = $derived(vokiTakingState.isCurrentQuestionFirst());

	let showFinishBtn = $derived(vokiTakingState.isCurrentQuestionLast());
	async function finishBtnPressed() {
		const response = await vokiTakingState.finishTakingAndReceiveResult();
		if (response.isSuccess) {
			onResultReceived(response.data);
		} else if (response.errs.length > 0) {
			vokiTakingErrs = response.errs;
		} else {
			vokiTakingErrs = [{ message: 'Something went wrong' }];
		}
	}
	function onNextButtonClicked() {
		if (vokiTakingErrs.length > 0) {
			vokiTakingErrs = [];
		}
		vokiTakingState.goToNextQuestion();
	}
	function onPrevButtonClicked() {
		if (vokiTakingErrs.length > 0) {
			vokiTakingErrs = [];
		}
		vokiTakingState.goToPreviousQuestion();
	}
</script>

<div class="btns-container">
	<button
		class="next-prev-btns"
		class:reduced={showFinishBtn}
		class:inactive={isPrevBtnInactive}
		onclick={onPrevButtonClicked}>Previous</button
	>
	<button
		class="finish-btn"
		class:hidden={!showFinishBtn}
		disabled={!showFinishBtn}
		onclick={() => finishBtnPressed()}>Finish</button
	>
	<button
		class="next-prev-btns"
		class:reduced={showFinishBtn}
		class:inactive={isNextBtnInactive}
		onclick={onNextButtonClicked}>Next</button
	>
</div>

<style>
	.btns-container {
		display: grid;
		justify-content: center;
		align-items: center;
		padding: 0 5rem;
		grid-template-columns: auto 1fr auto;
	}

	.next-prev-btns {
		width: 9rem;
		height: 2.25rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.75px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
	}

	.next-prev-btns:hover {
		background-color: var(--primary-hov);
	}

	.next-prev-btns.inactive {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		cursor: not-allowed;
	}

	.next-prev-btns.reduced {
		border-radius: 0.25rem;
		transform: scale(0.85);
	}

	.finish-btn {
		width: 10rem;
		height: 2.5rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		letter-spacing: 1px;
		transition: all 0.12s ease-in;
		transform: scale(1);
		cursor: pointer;
		justify-self: center;
	}

	.finish-btn.hidden {
		opacity: 0;
		transform: scale(0);
	}
</style>
