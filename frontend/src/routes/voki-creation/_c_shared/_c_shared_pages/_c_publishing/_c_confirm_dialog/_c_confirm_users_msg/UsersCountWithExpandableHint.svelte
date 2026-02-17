<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		userIds: string[];
	}
	let { userIds }: Props = $props();
</script>

<div class="count">
	{userIds.length}
	<div class="hint">
		{#each userIds as userId}
			<BasicUserDisplay
				{userId}
				interactionLevel={'UniqueNameGotoOnClick'}
				class="co-author-display"
			/>
		{/each}
	</div>
</div>

<style>
	.count {
		position: relative;
		display: inline-flex;
		align-items: center;
		justify-content: center;
		min-width: 1.5rem;
		padding: 0 0.35rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 500;
		cursor: help;
		transition:
			background-color 0.2s,
			color 0.2s;
	}
	.count:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.hint {
		position: absolute;
		top: 100%;
		left: 50%;
		z-index: 50;
		transform: translateX(-50%) translateY(0.5rem);

		display: flex;
		flex-flow: row wrap;
		align-items: center;
		justify-content: center;
		gap: 0.5rem;

		width: max-content;
		max-width: 20rem;
		padding: 0.75rem;
		margin-top: 0.25rem;

		background-color: var(--back);
		color: var(--text);
		border: 1px solid var(--muted);
		border-radius: 0.75rem;
		box-shadow: var(--shadow-lg);

		opacity: 0;
		visibility: hidden;
		pointer-events: none;
		transition: all 0.2s cubic-bezier(0.16, 1, 0.3, 1);
		transition-delay: 0.25s;
	}
	.count:hover .hint,
	.hint:hover {
		opacity: 1;
		visibility: visible;
		pointer-events: auto;
		transform: translateX(-50%) translateY(0);
		transition-delay: 0s;
	}
	.hint::before {
		content: '';
		position: absolute;
		top: -1rem;
		left: 0;
		width: 100%;
		height: 1rem;
	}
	.hint > :global(.co-author-display) {
		--profile-pic-width: 2.5rem !important;
	}
</style>
