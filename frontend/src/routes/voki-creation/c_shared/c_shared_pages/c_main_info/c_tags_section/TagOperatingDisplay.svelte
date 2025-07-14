<script lang="ts">
	let {
		tag,
		isTagAdded,
		isTagRemovingState,
		btnOnClick
	}: {
		tag: string;
		isTagAdded: boolean;
		isTagRemovingState: boolean;
		btnOnClick: () => void;
	} = $props<{
		tag: string;
		isTagAdded: boolean;
		isTagRemovingState: boolean;
		btnOnClick: () => void;
	}>();
</script>

<div class="tag-display">
	<span>#{tag}</span>
	{#if isTagRemovingState}
		<svg class="remove-btn" onclick={() => btnOnClick()}>
			<use href="#common-cross-icon" />
		</svg>
	{:else if isTagAdded}
		<svg>
			<use href="#common-check-icon" />
		</svg>
	{:else}
		<svg class="add-btn" onclick={() => btnOnClick()} viewBox="0 0 24 24" fill="none">
			<use href="#common-plus-icon" />
		</svg>
	{/if}
</div>

<style>
	.tag-display {
		display: grid;
		align-items: center;
		width: 100%;
		padding: 0.25rem 0.5rem;
		border-radius: 0.25rem;
		background-color: var(--back-second);
		box-sizing: border-box;
		grid-template-columns: 1fr auto;
	}

	.tag-display span {
		font-size: 1.25rem;
		word-break: break-all;
	}

	.tag-display svg {
		height: 1.5rem;
		padding: 0.25rem;
		border-radius: 16%;
		background-color: var(--gray);
		color: var(--back-main);
		transition: all 0.08s ease-in;
		cursor: pointer;
		aspect-ratio: 1/1;
		box-sizing: border-box;
	}

	.add-btn:hover {
		background-color: var(--accent-main);
	}

	.remove-btn:hover {
		background-color: var(--err-red);
	}
</style>
