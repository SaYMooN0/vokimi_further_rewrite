<script lang="ts">
	import type { GeneralVokiAnswerTextOnly, GeneralVokiAnswerTypeData } from '../../types';

	function isChecked(id: string) {
		return chosenAnswersMap.get(id) ?? false;
	}

	function toggle(id: string) {
		let current = chosenAnswersMap.get(id)!;
		chosenAnswersMap.set(id, !current);
	}
	let {
		answers,
		chosenAnswersMap
	}: {
		answers: { typeData: GeneralVokiAnswerTextOnly; id: string }[];
		chosenAnswersMap: Map<string, boolean>;
	} = $props<{
		answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
		chosenAnswersMap: Map<string, boolean>;
	}>();
</script>

<div class="answers-container">
	{#each answers as answer}
		<div class="answer" class:chosen={isChecked(answer.id)} onclick={() => toggle(answer.id)}>
			{answer.typeData.text}
		</div>
	{/each}
</div>

<style>
	.answer {
		padding: 2rem;
		margin: 1rem 0;
		box-shadow: var(--shadow-xs);
	}

	.answer.chosen {
		background-color: var(--secondary);
	}
</style>
