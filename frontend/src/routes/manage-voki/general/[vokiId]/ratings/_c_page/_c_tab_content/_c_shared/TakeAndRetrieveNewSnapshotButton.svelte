<script lang="ts">
	import type { Err } from '$lib/ts/err';

	interface Props {
		onTakeAndRetrieveRatingsSnapshotBtnClicked: () => void;
		snapshotsRetrievingState: { name: 'ok' } | { name: 'loading' } | { name: 'errs'; errs: Err[] };
	}
	let { onTakeAndRetrieveRatingsSnapshotBtnClicked, snapshotsRetrievingState }: Props = $props();
	function onBtnClick() {
		if (snapshotsRetrievingState.name === 'ok') {
			onTakeAndRetrieveRatingsSnapshotBtnClicked();
		}
	}
</script>

<button
	class="btn primary"
	type="button"
	onclick={onBtnClick}
	disabled={snapshotsRetrievingState.name === 'loading'}
>
	{#if snapshotsRetrievingState.name === 'loading'}
		<div class="spinner" />
	{:else}
		Take and retrieve new snapshot
	{/if}
</button>

<style>
	.spinner {
		width: 1.25rem;
		height: 1.25rem;
		border-radius: 50%;
		border: 0.1875rem solid var(--primary-foreground);
		border-top: 0.1875rem solid transparent;
		animation: spin 0.6s linear infinite;
	}

	@keyframes spin {
		from {
			transform: rotate(0deg);
		}
		to {
			transform: rotate(360deg);
		}
	}
</style>
