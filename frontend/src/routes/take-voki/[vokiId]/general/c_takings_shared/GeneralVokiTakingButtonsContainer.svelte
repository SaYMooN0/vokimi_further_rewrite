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
	<button
		class="finish-btn"
		onclick={onFinishBtnClick}
		class:hidden={!showFinishBtn}
		disabled={!showFinishBtn}>Finish</button
	>
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
</div>

<style>
	.btns-container {
		display: grid;
		grid-template-columns: 1fr 1fr 1fr;
		align-items: center;
	}
	.dummy {
		opacity: 0;
		font-size: 0.5rem;
	}

	.next-prev-btns {
		background-color: var(--primary);
		height: 2rem;
		width: 8rem;
		font-size: 1.25rem;
		font-weight: 450;
		border: none;
		border-radius: 0.25rem;
		color: var(--primary-foreground);
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
		background-color: var(--primary);
		height: 2.5rem;
		width: 10rem;
		font-size: 1.25rem;
		font-weight: 450;
		border: none;
		border-radius: 0.25rem;
		color: var(--primary-foreground);
		cursor: pointer;
		transform: scale(1);
		transition: all 0.12s ease-in;
	}
	.finish-btn.hidden {
		opacity: 0;
		transform: scale(0);
	}
</style>
