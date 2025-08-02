<script lang="ts">
	import type { PageProps } from './$types';
	import UnableToLoad from '../../../../c_shared/UnableToLoad.svelte';
	import QuestionTextSection from './c_page/QuestionTextSection.svelte';
	import VokiCreationBasicHeader from '../../../../c_shared/VokiCreationBasicHeader.svelte';
	import QuestionImagesSection from './c_page/QuestionImagesSection.svelte';
	import QuestionAnswerSettingsSection from './c_page/QuestionAnswerSettingsSection.svelte';
	import QuestionAnswersSection from './c_page/QuestionAnswersSection.svelte';

	let { data }: PageProps = $props();
	let questionAnswers = $state(data.data?.answers.sort((a, b) => a.order - b.order) ?? []);
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<div class="question-page">
		<VokiCreationBasicHeader header="Voki question editing" />
		<QuestionTextSection
			text={data.data.text}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
		/>
		<QuestionImagesSection
			images={data.data.images}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
		/>
		<QuestionAnswerSettingsSection
			shuffleAnswers={data.data.shuffleAnswers}
			minAnswers={data.data.minAnswersCount}
			maxAnswers={data.data.maxAnswersCount}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
		/>
		<QuestionAnswersSection
			answers={questionAnswers}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
			answersType={data.data.answersType}
		/>
	</div>
{/if}

<style>
	.question-page {
		display: flex;
		flex-direction: column;
	}
</style>
