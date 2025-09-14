<script lang="ts">
  import type { GeneralVokiAnswerTextOnly, GeneralVokiAnswerTypeData } from '../../types';
  import { type AnswerRef, answersKeyboardNav } from './answers-keyboard-nav.svelte';
  import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';
  import GeneralTakingAnswerText from './c_shared/GeneralTakingAnswerText.svelte';

  let {
    answers,
    isMultipleChoice,
    isAnswerChosen,
    chooseAnswer
  }: {
    answers: { typeData: GeneralVokiAnswerTextOnly; id: string }[];
    isMultipleChoice: boolean;
    isAnswerChosen: (answerId: string) => boolean;
    chooseAnswer: (answerId: string) => void;
  } = $props<{
    answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
    isMultipleChoice: boolean;
    isAnswerChosen: (answerId: string) => boolean;
    chooseAnswer: (answerId: string) => void;
  }>();

  let container: HTMLDivElement = $state<HTMLDivElement>()!;
</script>

<div
  class="answers-container"
  bind:this={container}
  use:answersKeyboardNav={{
    answers: answers as AnswerRef[],
    chooseAnswer,
    focusOnMount: true,
    useSpacebarToChoose: true
  }}
  tabindex="-1"
  role={isMultipleChoice ? 'group' : 'radiogroup'}
  aria-label="Answer choices"
>
  {#each answers as answer}
    <div
      class="answer"
      class:chosen={isAnswerChosen(answer.id)}
      onclick={() => chooseAnswer(answer.id)}
      tabindex="0"
      role={isMultipleChoice ? 'checkbox' : 'radio'}
      aria-checked={isAnswerChosen(answer.id)}
    >
      <GeneralTakingAnswerChosenIndicator
        {isMultipleChoice}
        isChosen={isAnswerChosen(answer.id)}
      />
      <GeneralTakingAnswerText text={answer.typeData.text} />
    </div>
  {/each}
</div>

<style>
	.answers-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	.answer {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.5rem 1rem;
		border-radius: 0.5rem;
		grid-template-columns: auto 1fr;
	}
</style>
