<script lang="ts">
	import GeneralVokiResultPreviewImage from '../../c_pages_shared/GeneralVokiResultPreviewImage.svelte';

	interface Props {
		result: {
			id: string;
			name: string;
			image: string | null;
			takings: { id: string; start: Date; finish: Date }[];
		};
		vokiId: string;
	}
	let isOpen = $state(false);
	let { result, vokiId }: Props = $props();
</script>

<li class="result">
	<div class="result-img">
		<GeneralVokiResultPreviewImage resultImage={result.image} resultName={result.name} />
	</div>
	<div class="result-main">
		<h3 class="result-name" title={result.name}>{result.name}</h3>
		<div class="buttons-container">
			<a class="result-page-link" href={`/take-voki/${vokiId}/general/results/${result.id}`}
				>Result page</a
			>

			<button
				class="toggle-takings-btn"
				class:open={isOpen}
				onclick={() => (isOpen = !isOpen)}
				title="Show all my takings with this result"
			>
				Received {result.takings.length}
				{result.takings.length === 1 ? 'time' : 'times'}
				<svg aria-hidden="true">
					<use href="#caret-up-icon" />
				</svg>
			</button>
		</div>

		<div class="takings-container" class:open={isOpen}>
			<div class="takings-column-names">
				<span>Start</span>
				<span>Finish</span>
				<span>Duration</span>
			</div>

			{#each result.takings as t}
				<div>
					<span>{t.start}</span>
					<span>{t.finish}</span>
					<span>{(t.start, t.finish)}</span>
				</div>
			{/each}
		</div>
	</div>
</li>

<style>
	.result {
		box-shadow: var(--shadow-xs);
        border-radius: 1rem;
		padding: 0.75rem;
		grid-template-columns: 8rem 1fr;
		gap: 1rem;
		display: grid;
	}
	.result-img {
		height: fit-content;
	}

	.result-main {
		display: grid;
		gap: 0.5rem;
		align-content: start;
	}

	.result-name {
		margin: 0.125rem 0 0 0;
		font-size: 1.125rem;
		line-height: 1.25;
		font-weight: 650;
        text-indent: 0.5em;
        word-break: normal;
		overflow-wrap: anywhere;
	}
	.buttons-container {
		display: flex;
		flex-direction: row;
		justify-content: right;
		gap: 0.75rem;
	}
	.buttons-container > * {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		height: 2rem;
		font-weight: 550;
		font-size: 0.875rem;
		border-radius: 0.375rem;
		padding: 0 1rem;
		border: none;
		width: fit-content;
		transition: background-color 0.12s ease;
	}
	.buttons-container > *:active {
		transform: scale(0.985);
	}
	.result-page-link {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}
	.result-page-link:hover {
		background: var(--accent);
		color: var(--accent-foreground);
	}
	.toggle-takings-btn {
		gap: 0.25rem;
		min-width: 10rem;
		background: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
		user-select: none;
	}
	.toggle-takings-btn:hover {
		background-color: var(--primary-hov);
	}
	.toggle-takings-btn svg {
		display: inline-block;
		height: 1.125rem;
		width: 1.125rem;
		transform: rotate(0deg);
		transition: transform 0.15s ease;
	}
	.toggle-takings-btn.open svg {
		transform: rotate(-180deg);
	}
	.takings-container {
		max-height: 0;
		overflow: hidden;
		transition:
			max-height 0.2s ease,
			opacity 0.2s ease,
			padding-top 0.2s ease;
		opacity: 0;
		margin-top: 0.5rem;
	}
	.takings-container.open {
		max-height: 30rem;
		opacity: 1;
		padding-top: 0.5rem;
	}

	.takings-column-names {
		display: grid;
		grid-template-columns: 1fr 1fr 1fr;
		gap: 0.5rem;
		margin-bottom: 0.375rem;
		justify-items: center;
		align-content: center;
		border-top: 0.125rem solid var(--secondary);
		border-bottom: 0.125rem solid var(--secondary) !important;
	}
	.takings-column-names > span {
		color: var(--secondary-foreground);
		font-weight: 550;
		font-size: 1rem;
	}
	.takings-container > div {
		display: grid;
		grid-template-columns: 1fr 1fr 1fr;
		gap: 0.5rem;
		padding: 0.375rem 0.25rem;
		font-weight: 550;
		font-size: 1rem;
		color: var(--text);
	}
</style>
