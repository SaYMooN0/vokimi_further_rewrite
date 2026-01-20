<script lang="ts">
	import type { PageProps } from './$types';
	import QuestionTextSection from './_c_page/QuestionTextSection.svelte';
	import VokiCreationBasicHeader from '../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import QuestionImagesSection from './_c_page/QuestionImagesSection.svelte';
	import QuestionAnswerSettingsSection from './_c_page/QuestionAnswerSettingsSection.svelte';
	import VokiCreationPageLoadingErr from '../../../../_c_shared/VokiCreationPageLoadingErr.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<div class="question-page">
		<VokiCreationBasicHeader header="Voki question editing" />
		<QuestionTextSection
			text={data.data.text}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
		/>
		<QuestionImagesSection
			imageSet={data.data.imageSet}
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
		type specific content
	</div>
{/if}

<style>
	.question-page {
		display: flex;
		flex-direction: column;
	}
</style>
