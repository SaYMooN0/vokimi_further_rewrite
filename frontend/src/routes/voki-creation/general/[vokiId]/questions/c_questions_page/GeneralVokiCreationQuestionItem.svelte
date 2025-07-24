<script lang="ts">
	import type { QuestionBriefInfo } from '../types';

	let { question, questionsCount }: { question: QuestionBriefInfo; questionsCount: number } =
		$props<{ question: QuestionBriefInfo; questionsCount: number }>();
</script>

<div class="question">
	<div class="change-order-btns">
		{#if question.orderInVoki != 0}
			<svg><use href="#caret-up-icon" /></svg>
		{/if}
		{#if question.orderInVoki != questionsCount}
			<svg><use href="#caret-down-icon" /></svg>
		{/if}
	</div>
	<p>{question.text}</p>
	<div class="buttons-container">
		<button class="edit-btn">Edit</button>
		<button class="delete-btn">
			<svg><use href="#common-trash-can-icon" /></svg>
		</button>
	</div>
</div>

<style>
	.question {
		display: flex;
		flex-direction: row;
		box-shadow: var(--shadow);
		border-radius: 1rem;
		padding: 0.5rem 1rem;
	}
	.change-order-btns {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
	}

	.change-order-btns svg {
		cursor: pointer;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		height: 2rem;
		width: 2rem;
		border-radius: 0.25rem;
	}
	.change-order-btns svg:hover {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}
	.buttons-container {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
	}
	.buttons-container button {
		height: 2rem;
		outline: none;
		border: none;
	}
    .edit-btn {
        padding: 0rem 1rem;
    }
	.delete-btn {
        width: 2rem;

		background-color: var(--err-foreground);
	}
	.delete-btn svg {
		height: 1.5rem;
		width: 1.5rem;
		color: var(--primary-foreground);
	}
</style>
