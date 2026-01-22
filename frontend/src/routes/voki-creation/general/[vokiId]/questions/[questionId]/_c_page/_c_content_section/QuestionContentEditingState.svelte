<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import VokiCreationSaveAndCancelButtons from '../../../../../../_c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';

	interface Props {
		savedContent: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		cancelEditing: () => void;
		updateParent: (newContent: GeneralVokiCreationQuestionContent) => void;
	}
	let { savedContent, questionId, vokiId, cancelEditing, updateParent }: Props = $props();
	let savingErrs = $state<Err[]>([]);
	async function saveChanges() {}
</script>

<DefaultErrBlock errList={savingErrs} class="question-content-err-block" />
<VokiCreationSaveAndCancelButtons onCancel={() => cancelEditing()} onSave={() => saveChanges()} />
