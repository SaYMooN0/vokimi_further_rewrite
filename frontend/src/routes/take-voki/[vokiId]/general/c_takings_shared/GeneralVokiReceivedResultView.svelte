<script lang="ts">
	import type { GeneralVokiTakingResultData } from '../types';
	interface Props {
		result: GeneralVokiTakingResultData;
		allowToSeeResultsList: boolean;
		vokiId: string;
	}
	let { result, allowToSeeResultsList, vokiId }: Props = $props();
</script>

<div>{JSON.stringify(result)}</div>

{#if allowToSeeResultsList}
	<a href="/take-voki/{vokiId}/results">See results list</a>
{:else}
	<div>Not allowed to see results list</div>
{/if}
