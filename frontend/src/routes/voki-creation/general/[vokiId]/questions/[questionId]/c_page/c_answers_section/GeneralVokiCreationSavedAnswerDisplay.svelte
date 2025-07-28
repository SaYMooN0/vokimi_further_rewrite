<script lang="ts">
	import type { QuestionAnswerData } from '../../../types';

	let {
		vokiId,
		QuestionId,
		answer
	}: { vokiId: string; QuestionId: string; answer: QuestionAnswerData } = $props<{
		vokiId: string;
		QuestionId: string;
		answer: QuestionAnswerData;
	}>();
</script>
