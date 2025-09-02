<script lang="ts">
	import type { GeneralVokiAnswerType } from '$lib/ts/voki';
	import type { GeneralVokiAnswerTypeData } from '../types';
	import GeneralVokiTakingAnswersDisplay from './c_answers_display/GeneralVokiTakingTextOnlyAnswers.svelte';

	interface Props {
		answerType: GeneralVokiAnswerType;
		answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
		chosenAnswersMap: Map<string, boolean>;
	}
	let { answerType, answers, chosenAnswersMap }: Props = $props();
</script>

{#if answerType === 'TextOnly'}
	<GeneralVokiTakingAnswersDisplay {answers} {chosenAnswersMap} />
{:else}
	<h1>Unknown answer type</h1>
{/if}
