<script lang="ts">
	let {
		vokiId,
		questionId,
		questionOrder,
		questionsCount
	}: {
		vokiId: string;
		questionId: string;
		questionOrder: number;
		questionsCount: number;
	} = $props<{
		vokiId: string;
		questionId: string;
		questionOrder: number;
		questionsCount: number;
	}>();
	async function moveUpInOrder() {}
	async function moveDownInOrder() {}
</script>

<div class="change-order-btns">
	{#if questionOrder != 0}
		<svg onclick={() => moveUpInOrder()}><use href="#caret-up-icon" /></svg>
	{/if}
	{#if questionOrder != questionsCount - 1}
		<svg onclick={() => moveDownInOrder()}><use href="#caret-down-icon" /></svg>
	{/if}
</div>

<style>
	.change-order-btns {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
	}

	.change-order-btns svg {
		width: 2rem;
		height: 2rem;
		border-radius: 0.25rem;
		color: var(--secondary-foreground);
		cursor: pointer;
	}

	.change-order-btns svg:hover {
		background-color: var(--secondary);
		color: var(--primary);
	}
</style>
