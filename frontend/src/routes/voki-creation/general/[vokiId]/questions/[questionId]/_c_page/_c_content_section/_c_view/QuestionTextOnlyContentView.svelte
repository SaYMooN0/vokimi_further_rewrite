<script lang="ts">
	import type { AnswerDataTextOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';
	import QuestionContentViewAnswersList from './_c_shared/QuestionContentViewAnswersList.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'TextOnly' }>;
		resultsIdToName: Record<string, string>;
	}
	let { content, resultsIdToName }: Props = $props();
</script>

<div class="answer-content">
	<QuestionContentViewAnswersList
		answers={content.answers}
		{resultsIdToName}
		createSnippet={answerContent}
	/>
</div>
{#snippet answerContent(answer: AnswerDataTextOnly)}
	<div class="answer-content">
		<AnswersViewTextDisplay text={answer.text} />
	</div>
{/snippet}

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}
</style>
