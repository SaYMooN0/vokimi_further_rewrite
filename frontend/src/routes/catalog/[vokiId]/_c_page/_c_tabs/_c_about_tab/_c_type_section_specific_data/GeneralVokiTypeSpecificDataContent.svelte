<script lang="ts">
	import type { GeneralVokiTypeSpecificData } from '../../../../types';
	import TypeSpecificDataBooleanField from './_c_shared/TypeSpecificDataBooleanField.svelte';

	interface Props {
		data: GeneralVokiTypeSpecificData;
	}
	let { data }: Props = $props();
</script>
{JSON.stringify(data)}
d
<!-- <TypeSpecificDataBooleanField fieldName="Any audio content:" value={data.anyAudios} /> -->
<!-- <TypeSpecificDataBooleanField
	fieldName="Force sequential answering:"
	value={data.forceSequentialAnswering}
/>
<TypeSpecificDataBooleanField fieldName="Shuffle questions:" value={data.shuffleQuestions} /> -->
