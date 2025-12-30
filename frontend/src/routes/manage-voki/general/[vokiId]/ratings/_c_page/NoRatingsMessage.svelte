<!-- <script lang="ts">
	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();
	const rateVokiLink = `/catalog/${vokiId}?tab=ratings`;
	function onCopyLinkClick() {}
</script>

<div class="no-ratings-message">
	<h1 class="main-msg">This voki doesn't have any ratings yet</h1>
	<p class="invite-friends">You can invite your friends to take and rate this voki</p>
	<div class="actions-container">
		<a class="rate-voki-link" href={rateVokiLink}>Rate this voki</a>
		<svg class="copy-link-btn" onclick={() => onCopyLinkClick()}
			><use href="#context-menu-copy-from-album-icon" /></svg
		>
	</div>
</div> -->
<script lang="ts">
	import { browser } from '$app/environment';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();

	const rateVokiPath = `/catalog/${vokiId}?tab=ratings`;
	const catalogPath = `/catalog/${vokiId}`;

	let copied = $state(false);
	let copyErr = $state<string | null>(null);

	let resetCopiedTimeout: ReturnType<typeof setTimeout> | null = null;

	function getShareUrl(): string {
		// на сервере location нет — вернём относительный путь
		if (!browser) return rateVokiPath;
		return `${location.origin}${rateVokiPath}`;
	}

	async function copyText(text: string) {
		// 1) Clipboard API
		if (browser && navigator?.clipboard?.writeText) {
			await navigator.clipboard.writeText(text);
			return;
		}

		// 2) fallback
		const ta = document.createElement('textarea');
		ta.value = text;
		ta.setAttribute('readonly', '');
		ta.style.position = 'fixed';
		ta.style.left = '-9999rem';
		document.body.appendChild(ta);
		ta.select();
		document.execCommand('copy');
		document.body.removeChild(ta);
	}

	async function onCopyLinkClick() {
		copyErr = null;

		try {
			await copyText(getShareUrl());
			copied = true;

			if (resetCopiedTimeout) clearTimeout(resetCopiedTimeout);
			resetCopiedTimeout = setTimeout(() => {
				copied = false;
			}, 1400);
		} catch {
			copyErr = 'Could not copy the link';
			copied = false;
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
				class="share-input"
				readonly
				value={getShareUrl()}
				onclick={onShareInputClick}
				aria-label="Share link"
			/>

			<button class="icon-btn" type="button" onclick={onCopyLinkClick} aria-label="Copy link">
				<svg class="icon"><use href="#context-menu-copy-link-icon" /></svg>
			</button>
		</div>

		{#if copied}
			<div class="hint ok">
				<svg class="hint-icon" viewBox="0 0 24 24" aria-hidden="true">
					<path
						d="M9.2 16.2 5.5 12.6a1 1 0 1 1 1.4-1.4l2.3 2.3 7-7a1 1 0 0 1 1.4 1.4l-8.4 8.3z"
						fill="currentColor"
					/>
				</svg>
				Copied
			</div>
		{/if}

		{#if copyErr}
			<div class="hint err">
				<svg class="hint-icon" viewBox="0 0 24 24" aria-hidden="true">
					<path
						d="M12 2a10 10 0 1 0 0 20 10 10 0 0 0 0-20zm1 13h-2v-2h2v2zm0-4h-2V7h2v4z"
						fill="currentColor"
					/>
				</svg>
				{copyErr}
			</div>
		{/if}
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
		padding: 0.4rem 0.75rem;
		border-radius: 999rem;
		font-weight: 600;
		font-size: 0.95rem;
		width: fit-content;
		box-shadow: var(--shadow-xs);
	}

	.title {
		margin-top: 0.9rem;
		color: var(--text);
		font-size: 1.85rem;
		line-height: 1.15;
		font-weight: 700;
	}

	.subtitle {
		margin-top: 0.6rem;
		color: var(--muted-foreground);
		font-size: 1.1rem;
		line-height: 1.45;
	}

	.share {
		margin-top: 1.6rem;
		background: color-mix(in srgb, var(--secondary) 75%, transparent);
		border: 0.0625rem solid color-mix(in srgb, var(--muted) 70%, transparent);
		border-radius: 1rem;
		padding: 1rem;
		box-shadow: var(--shadow-xs);
	}

	.share-label {
		color: var(--secondary-foreground);
		font-weight: 600;
		font-size: 1rem;
		margin-bottom: 0.75rem;
	}

	.share-row {
		display: flex;
		gap: 0.75rem;
		align-items: center;
	}

	.share-input {
		width: 100%;
		border: 0.0625rem solid color-mix(in srgb, var(--muted) 75%, transparent);
		background: var(--back);
		border-radius: 0.85rem;
		padding: 0.8rem 0.9rem;
		font-size: 1rem;
		color: var(--text);
		box-shadow: var(--shadow-xs);
	}

	.share-input:focus {
		outline: none;
		border-color: color-mix(in srgb, var(--primary) 45%, var(--muted));
		box-shadow: var(--shadow-md);
	}

	.icon-btn {
		flex: 0 0 auto;
		display: inline-flex;
		align-items: center;
		justify-content: center;
		width: 3rem;
		height: 3rem;
		border-radius: 0.9rem;
		border: 0.0625rem solid color-mix(in srgb, var(--muted) 75%, transparent);
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
		cursor: pointer;
	}

	.icon-btn:hover {
		box-shadow: var(--shadow-md);
	}

	.icon-btn:active {
		transform: translateY(0.0625rem);
	}

	.icon {
		width: 1.4rem;
		height: 1.4rem;
	}

	.hint {
		margin-top: 0.75rem;
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		font-weight: 600;
		font-size: 0.95rem;
		padding: 0.45rem 0.7rem;
		border-radius: 0.85rem;
		animation: hint-in 0.12s ease-in-out;
	}

	.hint-icon {
		width: 1.1rem;
		height: 1.1rem;
	}

	.hint.ok {
		background: var(--accent);
		color: var(--accent-foreground);
		border: 0.0625rem solid color-mix(in srgb, var(--accent-foreground) 18%, transparent);
	}

	.hint.err {
		background: var(--err-back);
		color: var(--err-foreground);
		border: 0.0625rem solid color-mix(in srgb, var(--err-foreground) 22%, transparent);
		box-shadow: var(--err-shadow);
	}

	@keyframes hint-in {
		from {
			opacity: 0.6;
			transform: translateY(0.125rem);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
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
		border-radius: 0.95rem;
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
