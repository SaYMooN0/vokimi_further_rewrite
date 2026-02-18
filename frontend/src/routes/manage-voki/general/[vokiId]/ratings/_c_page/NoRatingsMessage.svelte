<script lang="ts">
	import { browser } from '$app/environment';
	import { toast } from 'svelte-sonner';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();

	const rateVokiPath = $derived(`/catalog/${vokiId}?tab=ratings`);
	const catalogPath = $derived(`/catalog/${vokiId}`);

	let resetCopiedTimeout: ReturnType<typeof setTimeout> | null = null;

	function getShareUrl(): string {
		if (!browser) {
			return rateVokiPath;
		}
		return `${location.origin}${rateVokiPath}`;
	}

	async function onCopyLinkClick() {
		try {
			if (browser && navigator?.clipboard?.writeText) {
				await navigator.clipboard.writeText(getShareUrl());
				toast.success('Link copied to clipboard');
			} else {
				toast.error('Failed to copy link to clipboard');
			}
		} catch {
			toast.error('Failed to copy link to clipboard');
		}
	}

	function onShareInputClick(e: MouseEvent) {
		const el = e.currentTarget as HTMLInputElement;
		el.focus();
		el.select();
	}
</script>

<div class="no-ratings-message">
	<div class="badge">No ratings yet</div>

	<h1 class="title">This voki doesn’t have any ratings yet</h1>
	<p class="subtitle">Invite friends to take it and leave the first ratings.</p>

	<div class="share">
		<div class="share-label">Share the ratings link</div>

		<div class="share-row">
			<input
				class="share-input share-row-el"
				readonly
				value={getShareUrl()}
				onclick={onShareInputClick}
				aria-label="Share link"
			/>

			<button
				class="icon-btn share-row-el"
				type="button"
				onclick={onCopyLinkClick}
				aria-label="Copy link"
			>
				<svg class="icon"><use href="#context-menu-copy-link-icon" /></svg>
			</button>
		</div>
	</div>

	<div class="actions">
		<a class="btn primary" href={rateVokiPath}>Open ratings tab</a>
		<a class="btn secondary" href={catalogPath}>Open in catalog</a>
	</div>
</div>

<style>
	.no-ratings-message {
		display: flex;
		flex-direction: column;
		min-height: 100%;
		padding: 2rem 4rem;
	}

	.badge {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.375rem 1rem;
		border-radius: 999rem;
		background: var(--accent);
		color: var(--accent-foreground);
		font-size: 1rem;
		font-weight: 550;
	}

	.title {
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.875rem;
		font-weight: 700;
		line-height: 1;
	}

	.subtitle {
		margin-top: 0.75rem;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 425;
	}

	.share {
		padding: 1rem;
		margin-top: 2rem;
		border-radius: 1rem;
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
	}

	.share-label {
		margin-bottom: 0.75rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
	}

	.share-row {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		height: 2.75rem;
	}

	.share-row-el {
		height: 100%;
		border-radius: 0.75rem;
		box-shadow: var(--shadow-xs);
	}

	.share-row-el:hover {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.share-input {
		width: 100%;
		padding: 0 1rem;
		border: none;
		background: var(--back);
		color: var(--text);
		font-size: 1rem;
		cursor: default;
		outline: none;
	}

	.icon-btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		height: 100%;
		padding: 0.5rem;
		border: none;
		background: var(--secondary);
		color: var(--muted-foreground);
		cursor: default;
		flex: 0 0 auto;
		aspect-ratio: 1/1;
	}

	.icon-btn:active {
		transform: scale(0.97);
	}

	.icon {
		height: 100%;
		aspect-ratio: 1/1;
		stroke-width: 2;
	}

	.actions {
		display: flex;
		flex-wrap: wrap;
		gap: 0.75rem;
		margin-top: 1.6rem;
	}

	.btn {
		display: inline-flex;
		justify-content: center;
		align-items: center;
		padding: 0.85rem 1.1rem;
		border: 0.0625rem solid transparent;
		border-radius: 1rem;
		font-size: 1rem;
		font-weight: 700;
		box-shadow: var(--shadow-md);
	}

	.btn.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover {
		background: var(--primary-hov);
	}

	.btn.secondary {
		border-color: color-mix(in srgb, var(--muted) 70%, transparent);
		background: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
	}

	.btn.secondary:hover {
		box-shadow: var(--shadow-md);
	}
</style>
