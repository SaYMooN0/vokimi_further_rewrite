<script lang="ts">
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import VokiCreationSectionHeader from '../../../c_shared/VokiCreationSectionHeader.svelte';
	import type { PageProps } from './$types';
	import NoQuestions from './c_questions_page/NoQuestions.svelte';
	import QuestionInitializingDialog from './c_questions_page/QuestionInitializingDialog.svelte';
	import VokiTakingProcessSettingsSection from './c_questions_page/VokiTakingProcessSettingsSection.svelte';

	let { data }: PageProps = $props();
	let questionInitializingDialog = $state<QuestionInitializingDialog>()!;
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<QuestionInitializingDialog bind:this={questionInitializingDialog} vokiId={data.vokiId!} />
	<div class="questions-tab-container">
		{#if data.data.questions.length === 0}
			<NoQuestions openQuestionInitializingDialog={() => questionInitializingDialog.open()} />
		{:else}
			<VokiTakingProcessSettingsSection vokiId={data.vokiId!} settings={data.data.settings} />
			<VokiCreationSectionHeader header={`Questions (${data.data.questions.length})`} />
			<div class="questios">
				{#each data.data.questions as question}
					<div class="question">
						<p>{question.text}</p>
					</div>
				{/each}
			</div>
		{/if}
	</div>
{/if}

<style>
	.questions-tab-container {
		display: flex;
		flex-direction: column;
		width: 100%;
	}
</style>
