<script lang="ts">
	import type { PageProps } from './$types';
	import QuestionTextSection from './_c_page/QuestionTextSection.svelte';
	import VokiCreationBasicHeader from '../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import QuestionImagesSection from './_c_page/QuestionImagesSection.svelte';
	import QuestionAnswerSettingsSection from './_c_page/QuestionAnswerSettingsSection.svelte';
	import VokiCreationPageLoadingErr from '../../../../_c_shared/VokiCreationPageLoadingErr.svelte';
	import { GeneralVokiCreationSpecificQuestionPageState } from './general-voki-creation-specific-question-page-state.svelte';
	import {
		setVokiCreationCurrentPageState,
		setVokiCreationCurrentPageStateAsUnableToLoad
	} from '../../../../voki-creation-page-context';

	let { data }: PageProps = $props();

	let pageState: GeneralVokiCreationSpecificQuestionPageState | undefined = $state(undefined);
	if (data.isSuccess) {
		pageState = new GeneralVokiCreationSpecificQuestionPageState(
			data.data.text,
			data.data.imageSet,
			{
				shuffleAnswers: data.data.shuffleAnswers,
				minAnswersCount: data.data.minAnswersCount,
				maxAnswersCount: data.data.maxAnswersCount
			}
		);
		// svelte-ignore state_referenced_locally
		setVokiCreationCurrentPageState(pageState);
	} else {
		setVokiCreationCurrentPageStateAsUnableToLoad();
	}
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else if pageState}
	<div class="question-page">
		<VokiCreationBasicHeader header="Voki question editing" />
		<QuestionTextSection
			savedText={pageState.savedText}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
			updateSavedText={(newText) => (pageState.savedText = newText)}
			bind:isEditing={pageState.isEditingQuestionText}
		/>
		<QuestionImagesSection
			savedImageSet={pageState.savedImageSet}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
			updateSavedImageSet={(newImageSet) => (pageState.savedImageSet = newImageSet)}
			bind:isEditing={pageState.isEditingQuestionImages}
		/>
		<QuestionAnswerSettingsSection
			savedAnswerSettings={pageState.savedAnswerSettings}
			questionId={data.questionId!}
			vokiId={data.vokiId!}
			updateSavedAnswerSettings={(newSettings) => {
				pageState.savedAnswerSettings.shuffleAnswers = newSettings.shuffleAnswers;
				pageState.savedAnswerSettings.minAnswersCount = newSettings.minAnswersCount;
				pageState.savedAnswerSettings.maxAnswersCount = newSettings.maxAnswersCount;
			}}
			bind:isEditing={pageState.isEditingQuestionAnswerSettings}
		/>
		type specific content
	</div>
{:else}
	<VokiCreationPageLoadingErr
		vokiId={data.vokiId!}
		errs={[{ message: 'Could not initialize page state' }]}
	/>
{/if}

<style>
	.question-page {
		display: flex;
		flex-direction: column;
	}
</style>
