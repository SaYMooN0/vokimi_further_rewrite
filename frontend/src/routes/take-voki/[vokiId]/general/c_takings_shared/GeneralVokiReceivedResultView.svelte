<script lang="ts">
	import type { GeneralVokiTakenReceivedVokiResult } from '../types';
	interface Props {
		result: GeneralVokiTakenReceivedVokiResult;
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
