<script lang="ts">
	import { toast } from 'svelte-sonner';
	import type { DefaultGeneralVokiTakingState } from './default-general-voki-taking-state.svelte';
	let { vokiTakingState }: { vokiTakingState: DefaultGeneralVokiTakingState } = $props<{
		vokiTakingState: DefaultGeneralVokiTakingState;
	}>();

	let isNextBtnInactive = $derived(vokiTakingState.isCurrentQuestionLast());
	let isPrevBtnInactive = $derived(vokiTakingState.isCurrentQuestionFirst());

	function finishBtnPressed() {
		toast.error('Not implemented yet');
	}
	let showFinishBtn = $derived(vokiTakingState.isCurrentQuestionLast());
</script>

<div class="btns-container">
	<button
		class="next-prev-btns"
		class:reduced={showFinishBtn}
		class:inactive={isPrevBtnInactive}
		onclick={() => vokiTakingState.goToPreviousQuestion()}>Previous</button
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
		onclick={() => vokiTakingState.goToNextQuestion()}>Next</button
	>
</div>

<style>
	.btns-container {
		display: grid;
		grid-template-columns: auto 1fr auto;
		align-items: center;
		justify-content: center;
		padding: 0 5rem;
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
		cursor: pointer;
		letter-spacing: 0.75px;
		transition: transform 0.12s ease-in;
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
		transform: scale(0.85);
		border-radius: 0.25rem;
	}

	.finish-btn {
		justify-self: center;
		width: 10rem;
		height: 2.5rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		transition: all 0.12s ease-in;
		transform: scale(1);
		cursor: pointer;
		letter-spacing: 1px;
	}

	.finish-btn.hidden {
		opacity: 0;
		transform: scale(0);
	}
</style>
