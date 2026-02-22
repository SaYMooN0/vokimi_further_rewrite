<script lang="ts">
	import type { Snippet } from 'svelte';
	import { onMount } from 'svelte';
	import { VokiIgnoreSpoilerCookie } from '$lib/ts/cookies/voki-ignore-spoiler-cookie';

	interface Props {
		vokiId: string;
		hasUserTaken: boolean;
		text: string;
		children: Snippet;
	}
	let { vokiId, hasUserTaken, text, children }: Props = $props();

	let spoilerIgnored = $state(false);
	let showVokiNotTaken = $derived(!(hasUserTaken || spoilerIgnored));

	onMount(() => {
		if (VokiIgnoreSpoilerCookie.isIgnored(vokiId)) {
			spoilerIgnored = true;
		}
	});

	function handleIgnoreSpoiler() {
		spoilerIgnored = true;
		VokiIgnoreSpoilerCookie.markAsIgnoredFor12Hours(vokiId);
	}

	const catalogHref = $derived(`/catalog/voki/${vokiId}`);
</script>

{#if showVokiNotTaken}
	<div class="wrap" role="region" aria-label="Spoiler warning">
		<div class="content">
			<div class="icon" aria-hidden="true">
				<svg viewBox="0 0 24 24">
					<use href="#common-warning-icon" />
				</svg>
			</div>

			<h2 class="title">Spoiler warning</h2>
			<p class="text">{text}</p>

			<div class="actions">
				<a class="primary" href={catalogHref}>See Voki in catalog</a>
				<button class="secondary" type="button" onclick={handleIgnoreSpoiler}>See anyway</button>
			</div>
		</div>
	</div>
{:else}
	{@render children()}
{/if}

<style>
	.wrap {
		width: 100%;
		min-height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		padding: 2rem 1.25rem;
	}

	.content {
		width: 100%;
		max-width: 34rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		text-align: center;
		gap: 0.75rem;
	}

	.icon {
		display: grid;
		place-items: center;
		width: 3.5rem;
		height: 3.5rem;
		color: var(--warn-foreground);
	}

	.icon > svg {
		width: 100%;
		height: 100%;
	}

	.title {
		color: var(--text);
		font-size: 1.5rem;
		line-height: 1.25;
		font-weight: 700;
		letter-spacing: -0.01em;
	}

	.text {
		color: var(--secondary-foreground);
		font-size: 1rem;
		line-height: 1.45;
		max-width: 30rem;
	}

	.actions {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		margin-top: 0.5rem;
	}

	.primary,
	.secondary {
		width: 100%;
		display: inline-flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		padding: 0.85rem 1rem;
		border-radius: 0.75rem;
		font-size: 1rem;
		font-weight: 600;
		line-height: 1;
		cursor: pointer;
	}

	.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.primary:hover {
		background: var(--primary-hov);
	}

	.secondary {
		background: var(--secondary);
		color: var(--text);
		border: 0.0625rem solid var(--muted);
	}

	.secondary:hover {
		background: var(--accent);
		color: var(--accent-foreground);
		border-color: var(--accent);
	}

	.primary:focus-visible,
	.secondary:focus-visible {
		outline: 0.125rem solid var(--accent);
		outline-offset: 0.125rem;
	}
</style>
