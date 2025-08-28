<script lang="ts">
	import type { GeneralVokiTakingAnswerData } from '../types';

	interface Props {
		answersList: GeneralVokiTakingAnswerData[];
		selectedAnswersMap: Map<string, boolean>;
	}
	let incorrectAnswersCountMessage = $state('');
</script>

{#if incorrectAnswersCountMessage != ''}
	<div class="incorrect-answers-count-message">{incorrectAnswersCountMessage}</div>
{/if}

<style>
	.incorrect-answers-count-message {
		background-color: var(--err-back);
		color: var(--err-foreground);
		border: var(--err-foreground) 0.125rem solid;
	}
</style>
