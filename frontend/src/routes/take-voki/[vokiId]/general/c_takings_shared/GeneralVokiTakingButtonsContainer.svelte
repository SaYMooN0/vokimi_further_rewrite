<script lang="ts">
	type BtnViewState = 'hidden' | 'inactive' | 'active';
	interface Props {
		nextBtnState: BtnViewState;
		onNextBtnClick: () => void;
		prevBtnState: BtnViewState;
		onPrevBtnClick: () => void;
		showFinishBtn: boolean;
		onFinishBtnClick: () => void;
	}
	let {
		nextBtnState,
		onNextBtnClick,
		prevBtnState,
		onPrevBtnClick,
		showFinishBtn,
		onFinishBtnClick
	}: Props = $props();
</script>

<div class="btns-container">
	{#if prevBtnState === 'hidden'}
		<div class="dummy">What were you expecting to see here?</div>
	{:else}
		<button
			class="next-prev-btns"
			class:reduced={showFinishBtn}
			class:inactive={prevBtnState === 'inactive'}
			onclick={onPrevBtnClick}>Previous</button
		>
	{/if}
	<button
		class="finish-btn"
		onclick={onFinishBtnClick}
		class:hidden={!showFinishBtn}
		disabled={!showFinishBtn}>Finish</button
	>
	{#if nextBtnState === 'hidden'}
		<div class="dummy">What were you expecting to see here?</div>
	{:else}
		<button
			class="next-prev-btns"
			class:reduced={showFinishBtn}
			class:inactive={nextBtnState === 'inactive'}
			onclick={onNextBtnClick}>Next</button
		>
	{/if}
</div>

<style>
	.btns-container {
		display: grid;
		grid-template-columns: 1fr 1fr 1fr;
		align-items: center;
	}

	.dummy {
		font-size: 0.5rem;
		opacity: 0;
	}

	.next-prev-btns {
		width: 8rem;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		cursor: pointer;
	}

	.next-prev-btns:hover {
		background-color: var(--primary-hov);
	}

	.next-prev-btns.inactive {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		cursor: not-allowed;
	}

	.next-prev-btns.reduced {
		transform: scale(0.9);
	}

	.finish-btn {
		width: 10rem;
		height: 2.5rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		transition: all 0.12s ease-in;
		transform: scale(1);
		cursor: pointer;
	}

	.finish-btn.hidden {
		opacity: 0;
		transform: scale(0);
	}
</style>
