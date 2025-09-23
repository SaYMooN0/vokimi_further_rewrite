<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess && data.response.data}
	<div class="view-result">
		{#if data.response.data.image}
			<img
				class="result-image"
				src={StorageBucketMain.fileSrc(data.response.data.image)}
				alt={`Result image for ${data.response.data.name}`}
			/>
		{/if}

		<h1 class="result-name">{data.response.data.name}</h1>
		<p class="result-text">{data.response.data.text}</p>
		{#if data.response.data.ResultsVisibility === 'Anyone'}
			<a class="see-all-btn" href={`/take-voki/${data.vokiId}/general/results/all`}
				>View all ({data.resultsCount}) results</a
			>
		{:else}
			<AuthView>
				{#snippet authenticated()}
					<a class="see-all-btn" href={`/take-voki/${data.vokiId}/general/results/all`}
						>View all ({data.resultsCount}) results</a
					>
				{/snippet}
			</AuthView>
		{/if}
		<AuthView>
			{#snippet authenticated()}
				<a class="see-received-btn" href={`/take-voki/${data.vokiId}/general/results/received`}
					>See my received results</a
				>
			{/snippet}
		</AuthView>
	</div>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load voki result "
		errs={data.response.errs}
		additionalParams={[
			{ name: 'vokiId', value: data.vokiId },
			{ name: 'resultId', value: data.resultId }
		]}
	/>
{/if}

<style>
	.view-result {
		min-width: 50rem;
		width: calc(100% - 10rem);
		margin: 2rem auto;
		padding: 2rem;
		background: var(--back);
		border-radius: 1.5rem;
		box-shadow: var(--shadow-md), var(--shadow-xs);
		text-align: center;
		display: flex;
		flex-direction: column;
		gap: 1.5rem;
	}

	.result-image {
		max-height: 30rem;
		max-width: 55rem;
		border-radius: 1rem;
		object-fit: contain;
		margin: 0 auto;
		box-shadow: var(--shadow-xs);
	}

	.result-name {
		width: auto;
		height: auto;
		font-size: 1.75rem;
		font-weight: 600;
		margin: 0;
		color: var(--primary);
	}

	.result-text {
		width: 100%;
		display: block;
		text-align: justify;
		font-size: 1.375rem;
		line-height: 1.1;
		font-weight: 450;
		color: var(--text);
		text-indent: 1rem;
	}

	.see-all-btn {
		margin-top: 1rem;
		align-self: center;
		width: fit-content;
		padding: 0.25rem 2rem;
		background: var(--primary);
		color: var(--primary-foreground);
		border-radius: 0.375rem;
		font-size: 1.375rem;
		font-weight: 400;
		transition: all 0.12s ease;
		letter-spacing: 0.5px;
		cursor: pointer;
	}

	.see-all-btn:hover {
		background: var(--primary-hov);
	}

	.see-received-btn {
		align-self: center;
		width: fit-content;
		padding: 0.125rem 1rem;
		border-radius: 0.375rem;
		background-color: var(--back-main);
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		text-decoration: underline;
		transition: letter-spacing 0.2s ease;
		cursor: pointer;
	}

	.see-received-btn:hover {
		background-color: var(--muted);
		letter-spacing: 0.5px;
	}
</style>
