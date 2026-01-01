<script lang="ts">
	import { browser } from '$app/environment';
	import { toast } from 'svelte-sonner';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();

	const rateVokiPath = `/catalog/${vokiId}?tab=ratings`;
	const catalogPath = `/catalog/${vokiId}`;

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
		min-height: 100%;
		display: flex;
		flex-direction: column;
		padding: 2rem 4rem;
	}

	.badge {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		background: var(--accent);
		color: var(--accent-foreground);
		padding: 0.375rem 1rem;
		border-radius: 999rem;
		font-weight: 550;
		font-size: 1rem;
		width: fit-content;
	}

	.title {
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.875rem;
		line-height: 1;
		font-weight: 700;
	}

	.subtitle {
		margin-top: 0.75rem;
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 425;
	}

	.share {
		margin-top: 2rem;
		background: var(--secondary);
		border-radius: 1rem;
		padding: 1rem;
		box-shadow: var(--shadow-xs);
	}

	.share-label {
		color: var(--secondary-foreground);
		font-weight: 500;
		font-size: 1rem;
		margin-bottom: 0.75rem;
	}

	.share-row {
		display: flex;
		gap: 0.75rem;
		align-items: center;
		height: 2.75rem;
	}
	.share-row-el {
		height: 100%;
		box-shadow: var(--shadow-xs);
		border-radius: 0.75rem;
	}
	.share-row-el:hover {
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.share-input {
		width: 100%;
		background: var(--back);
		border: none;
		padding: 0 1rem;
		font-size: 1rem;
		color: var(--text);
		outline: none;
		cursor: default;
	}

	.icon-btn {
		flex: 0 0 auto;
		display: inline-flex;
		align-items: center;
		justify-content: center;
		height: 100%;
		aspect-ratio: 1/1;
		background: var(--secondary);
		padding: 0.5rem;
		border: none;
		cursor: default;
		color: var(--muted-foreground);
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
		margin-top: 1.6rem;
		display: flex;
		gap: 0.75rem;
		flex-wrap: wrap;
	}

	.btn {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		padding: 0.85rem 1.1rem;
		border-radius: 1rem;
		font-weight: 700;
		font-size: 1rem;
		box-shadow: var(--shadow-md);
		border: 0.0625rem solid transparent;
	}

	.btn.primary {
		background: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover {
		background: var(--primary-hov);
	}

	.btn.secondary {
		background: var(--secondary);
		color: var(--secondary-foreground);
		border-color: color-mix(in srgb, var(--muted) 70%, transparent);
		box-shadow: var(--shadow-xs);
	}

	.btn.secondary:hover {
		box-shadow: var(--shadow-md);
	}
</style>
