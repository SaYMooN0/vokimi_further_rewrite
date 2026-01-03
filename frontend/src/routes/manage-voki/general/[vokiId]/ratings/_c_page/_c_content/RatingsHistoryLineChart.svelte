<script lang="ts">
	import { RatingsHistoryState } from './rating-history-state.svelte';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();
	const componentState = new RatingsHistoryState(vokiId);
</script>

{#if componentState.allSnapshotsState.type === 'ok'}
	<p>ok</p>
{:else if componentState.allSnapshotsState.type === 'loading'}
	<p>loading</p>
{:else if componentState.allSnapshotsState.type === 'errs'}
	<p>err</p>
{:else}
	<p>something went wrong, unexpected state</p>
{/if}
