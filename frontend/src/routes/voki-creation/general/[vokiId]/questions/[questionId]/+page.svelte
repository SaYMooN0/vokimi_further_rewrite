<script lang="ts">
	import type { PageProps } from './$types';
	import UnableToLoad from '../../../../c_shared/UnableToLoad.svelte';
	import QuestionTextSection from './c_page/QuestionTextSection.svelte';
	import VokiCreationBasicHeader from '../../../../c_shared/VokiCreationBasicHeader.svelte';
	import QuestionImagesSection from './c_page/QuestionImagesSection.svelte';
	import QuestionSettingsSection from './c_page/QuestionSettingsSection.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<VokiCreationBasicHeader header="Voki question editing" />
	<QuestionTextSection text={data.data.text} questionId={data.questionId!} vokiId={data.vokiId!} />
	<QuestionImagesSection
		images={data.data.images}
		questionId={data.questionId!}
		vokiId={data.vokiId!}
	/>
	<QuestionSettingsSection
		shuffleAnswers={data.data.shuffleAnswers}
		minAnswers={data.data.minAnswersCount}
		maxAnswers={data.data.maxAnswersCount}
		questionId={data.questionId!}
		vokiId={data.vokiId!}
	/>
{/if}
