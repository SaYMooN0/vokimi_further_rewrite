<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import type { GeneralVokiResultsVisibility } from '../../../types';

	interface Props {
		resultsCount: number;
		resultsVisibility: GeneralVokiResultsVisibility;
		vokiId: string;
	}
	let { resultsCount, resultsVisibility, vokiId }: Props = $props();
</script>

{#if resultsVisibility === 'Anyone'}
	<a class="see-all-btn" href={`/take-voki/${vokiId}/general/results/all`}
		>View all ({resultsCount}) results</a
	>
{:else}
	<AuthView>
		{#snippet authenticated()}
			<a class="see-all-btn" href={`/take-voki/${vokiId}/general/results/all`}
				>View all ({resultsCount}) results</a
			>
		{/snippet}
	</AuthView>
{/if}
<AuthView>
	{#snippet authenticated()}
		<a class="see-received-btn" href={`/take-voki/${vokiId}/general/results/received`}
			>See my received results</a
		>
	{/snippet}
</AuthView>

<style>
	.see-all-btn {
		width: fit-content;
		padding: 0.25rem 2rem;
		margin-top: 1rem;
		border-radius: 0.375rem;
		background: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.375rem;
		font-weight: 400;
		letter-spacing: 0.5px;
		transition: all 0.12s ease;
		cursor: pointer;
		align-self: center;
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
