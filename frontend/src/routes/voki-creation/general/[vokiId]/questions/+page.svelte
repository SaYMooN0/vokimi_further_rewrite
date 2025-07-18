<script lang="ts">
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import VokiCreationFieldName from '../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSectionHeader from '../../../c_shared/VokiCreationSectionHeader.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

<!-- {#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else} -->
	<div class="questions-tab-container">
		<VokiCreationSectionHeader header="Question settings" />
		<p class="field-p">
			<VokiCreationFieldName fieldName="Questions order:" />
			{data.data.settings.shuffleQuestions ? 'Shuffled' : 'Ordered'}
		</p>
		<p class="field-p">
			<VokiCreationFieldName fieldName="Answering mode:" />
			{data.data.settings.forceSequentialAnswering ? 'Sequential' : 'Free'}
		</p>
		<VokiCreationSectionHeader header={`Questions (${data.data.questions.length})`} />
	</div>
<!-- {/if} -->

<style>
	.questions-tab-container {
		display: flex;
		width: 100%;
	}
</style>
