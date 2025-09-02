<script lang="ts">
	import { toast } from 'svelte-sonner';
	import GeneralVokiTakingButtonsContainer from '../c_takings_shared/GeneralVokiTakingButtonsContainer.svelte';
	import type { DefaultGeneralVokiTakingState } from './default-general-voki-taking-state.svelte';
	let { vokiTakingState }: { vokiTakingState: DefaultGeneralVokiTakingState } = $props<{
		vokiTakingState: DefaultGeneralVokiTakingState;
	}>();

	let nextBtnState: 'inactive' | 'active' = $derived(
		vokiTakingState.isCurrentQuestionLast() ? 'inactive' : 'active'
	);
	function nextQuestionBtnPressed() {
		const err = vokiTakingState.goToNextQuestion();
		if (err.length > 0) {
			toast.error(err[0].message);
		}
	}
	let prevBtnState: 'inactive' | 'active' = $derived(
		vokiTakingState.isCurrentQuestionFirst() ? 'inactive' : 'active'
	);
	function prevQuestionBtnPressed() {
		const err = vokiTakingState.goToPreviousQuestion();
		if (err.length > 0) {
			toast.error(err[0].message);
		}
	}

	function finishBtnPressed() {
		toast.error('Not implemented yet');
	}
</script>

<GeneralVokiTakingButtonsContainer
	{nextBtnState}
	onNextBtnClick={nextQuestionBtnPressed}
	{prevBtnState}
	onPrevBtnClick={prevQuestionBtnPressed}
	showFinishBtn={false}
	onFinishBtnClick={() => finishBtnPressed()}
/>
