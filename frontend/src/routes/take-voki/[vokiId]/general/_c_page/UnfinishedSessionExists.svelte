<script lang="ts">
	import GeneralVokiUnfinishedSessionView from '$lib/components/voki_types_specific/general/GeneralVokiUnfinishedSessionView.svelte';

	interface Props {
		vokiId: string;
		questionsWithSavedAnswersCount: number;
		sessionId: string;
		startedAt: Date;
		totalQuestionsCount: number;
	}
	let { questionsWithSavedAnswersCount, sessionId, startedAt, totalQuestionsCount, vokiId }: Props =
		$props();

	let takeVokiPageLink = $derived(`/take-voki/${vokiId}/general`);
</script>

<GeneralVokiUnfinishedSessionView
	sessionData={{
		vokiId,
		sessionId,
		startedAt,
		questionsWithSavedAnswersCount,
		totalQuestionsCount
	}}
	{takeVokiPageLink}
/>
