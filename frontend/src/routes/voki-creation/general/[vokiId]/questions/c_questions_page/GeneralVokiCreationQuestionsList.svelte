<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { toast } from 'svelte-sonner';
	import type { QuestionBriefInfo } from '../types';
	import GeneralVokiCreationQuestionItem from './GeneralVokiCreationQuestionItem.svelte';
	import VokiCreationBasicHeader from '../../../../c_shared/VokiCreationBasicHeader.svelte';

	let { questionsProps, vokiId }: { questionsProps: QuestionBriefInfo[]; vokiId: string } = $props<{
		questionsProps: QuestionBriefInfo[];
		vokiId: string;
	}>();
	let questions = $state(questionsProps);
	async function moveQuestionUpInOrder(questionId: string) {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			questions: QuestionBriefInfo[];
		}>(`/vokis/${vokiId}/questions/${questionId}/move-up-in-order`, RequestJsonOptions.PATCH({}));
		if (response.isSuccess) {
			questions = response.data.questions;
		} else {
			toast.error(response.errs[0].message);
		}
	}
	async function moveQuestionDownInOrder(questionId: string) {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			questions: QuestionBriefInfo[];
		}>(`/vokis/${vokiId}/questions/${questionId}/move-down-in-order`, RequestJsonOptions.PATCH({}));
		if (response.isSuccess) {
			questions = response.data.questions;
		} else {
			toast.error(response.errs[0].message);
		}
	}
	async function deleteQuestion(questionId: string) {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			questions: QuestionBriefInfo[];
		}>(`/vokis/${vokiId}/questions/${questionId}/delete`, RequestJsonOptions.DELETE({}));
		if (response.isSuccess) {
			questions = response.data.questions;
		} else {
			toast.error(response.errs[0].message);
		}
	}
</script>

<VokiCreationBasicHeader header={`Voki questions (${questions.length})`} />
<div class="questions">
	{#each questions as question}
		<GeneralVokiCreationQuestionItem
			{vokiId}
			{question}
			questionsCount={questions.length}
			moveQuestionUpInOrder={() => {
				moveQuestionUpInOrder(question.id);
			}}
			moveQuestionDownInOrder={() => {
				moveQuestionDownInOrder(question.id);
			}}
			deleteQuestion={() => {
				deleteQuestion(question.id);
			}}
		/>
	{/each}
</div>

<style>
	.questions {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
	}
</style>
