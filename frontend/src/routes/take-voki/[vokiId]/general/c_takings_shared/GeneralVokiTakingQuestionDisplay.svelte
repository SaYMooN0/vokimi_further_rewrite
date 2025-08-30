<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	interface Props {
		text: string;
		images: string[];
		minAnswersCount: number;
		maxAnswersCount: number;
		totalQuestionsCount: number;
		questionNumber: number;
	}
	let {
		text,
		images,
		minAnswersCount,
		maxAnswersCount,
		totalQuestionsCount,
		questionNumber
	}: Props = $props();
</script>

<div class="question-display-container">
	<label class="question-num">
		Question #{questionNumber} out of {totalQuestionsCount}
	</label>
	<h2 class="question-text">{text}</h2>
	<div class="images-container">
		{#each images as image}
			<img src={StorageBucketMain.fileSrc(image)} alt="question-img" />
		{/each}
	</div>
	<label class="answers-count-label">
		{#if minAnswersCount === maxAnswersCount && minAnswersCount === 1}
			Choose 1 answer
		{:else}
			Choose from {minAnswersCount} to {maxAnswersCount} answers
		{/if}
	</label>
</div>

<style>
	.question-display-container {
		display: flex;
		flex-direction: column;
		align-items: center;
	}

	.question-num {
		color: var(--muted-foreground);
		font-size: 1.125rem;
	}

	.question-text {
		margin: 4px 0;
		color: var(--text);
		font-size: 24px;
		font-weight: 500;
	}

	.images-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		gap: 1rem;
	}

	.images-container img {
		width: 100%;
		min-width: 20rem;
		max-width: 38rem;
		min-height: 15rem;
		max-height: 25rem;
		border-radius: 0;
		object-fit: contain;
	}

	.answers-count-label {
		margin: 0;
		color: var(--text);
		font-size: 18px;
	}
</style>
