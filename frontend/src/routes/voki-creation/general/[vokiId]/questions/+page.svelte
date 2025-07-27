<script lang="ts">
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';
	import NoQuestions from './c_questions_page/NoQuestions.svelte';
	import QuestionInitializingDialog from './c_questions_page/QuestionInitializingDialog.svelte';
	import VokiTakingProcessSettingsSection from './c_questions_page/VokiTakingProcessSettingsSection.svelte';

	

	import GeneralVokiCreationQuestionItem from './c_questions_page/GeneralVokiCreationQuestionItem.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import VokiCreationBasicHeader from '../../../c_shared/VokiCreationBasicHeader.svelte';

	let { data }: PageProps = $props();
	let questionInitializingDialog = $state<QuestionInitializingDialog>()!;
	const maxQuestionsCount = 100;
</script>



{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<QuestionInitializingDialog bind:this={questionInitializingDialog} vokiId={data.vokiId!} />
	{#if data.data.questions.length === 0}
		<NoQuestions openQuestionInitializingDialog={() => questionInitializingDialog.open()} />
	{:else}
		<VokiTakingProcessSettingsSection vokiId={data.vokiId!} settings={data.data.settings} />
		<VokiCreationBasicHeader header={`Questions (${data.data.questions.length})`} />
		<div class="questions">
			{#each data.data.questions as question}
				<GeneralVokiCreationQuestionItem
					vokiId={data.vokiId!}
					{question}
					questionsCount={data.data.questions.length}
				/>
			{/each}
		</div>
		{#if data.data.questions.length < maxQuestionsCount}
		<div class="add-new-question-btn-container">
			<PrimaryButton onclick={() => questionInitializingDialog.open()} 
				>Add new question</PrimaryButton
			>
		</div>
		{/if}
	{/if}
{/if}

<style>
	
	.questions {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
	}

	.add-new-question-btn-container {
		display: flex;
		justify-content: center;
	}
		.add-new-question-btn-container > :global(.primary-btn) {
		margin: 1.25rem auto;
		
	}
</style>
