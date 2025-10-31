<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { DateUtils } from '$lib/ts/utils/date-utils';

	interface Props {
		viewerId: string;
		primaryAuthorId: string;
		creationDate: Date;
	}
	let { viewerId, primaryAuthorId, creationDate }: Props = $props();
	let primaryAuthor = UsersStore.Get(primaryAuthorId);
	let viewerIsPrimaryAuthor = $derived(viewerId === primaryAuthorId);
	// primaryAuthor = { state: 'loading', errs: [] };
</script>

<div
	class="primary-author-container"
	class:viewer-is-primary-author={primaryAuthor.state === 'ok' && viewerIsPrimaryAuthor}
	class:loading={primaryAuthor.state === 'loading'}
	class:err={primaryAuthor.state === 'errs'}
>
	{#if primaryAuthor.state === 'ok'}
		<label class="prim-author-label">
			{#if viewerIsPrimaryAuthor}
				You are the primary author
			{:else}
				Voki primary author
			{/if}
		</label>
	{/if}
	{#if primaryAuthor.state === 'ok'}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(primaryAuthor.data.profilePic)}
			alt={`Profile picture of ${primaryAuthor.data.displayName}`}
			loading="lazy"
			decoding="async"
		/>
		<div class="main-content">
			<label class="display-name">{primaryAuthor.data.displayName}</label>
			<a href="/authors/{primaryAuthorId}" class="unique-name">@{primaryAuthor.data.uniqueName}</a>
			<label class="created-voki-date">Created voki on {DateUtils.toLocaleDateOnly(creationDate)}</label>
		</div>
	{:else if primaryAuthor.state === 'loading'}
		<div class="profile-pic"></div>
		<div class="main-content"></div>
	{:else}
		<div class="profile-pic"></div>
		<div class="main-content">
			{#if primaryAuthor.state === 'errs' && primaryAuthor.errs.length > 0}
				{#each primaryAuthor.errs as err}
					<p class="err-view">{err}</p>
				{/each}
			{:else}
				<p class="err-view">Something went wrong. Could not load user data</p>
			{/if}
		</div>
	{/if}
</div>

<style>
	.primary-author-container {
		position: relative;
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		width: 100%;
		min-height: calc(4rem + 5vh);
		padding: 0.75rem 1rem 0.75rem;
		border-radius: 1rem;
		background: var(--back);
		border: 0.125rem solid transparent;
		box-shadow: var(--shadow-xs), var(--shadow-md);
		animation: var(--default-fade-in);
		height: calc(5rem + 5vh);
		border: 0.125rem solid var(--back);
		gap: 0.5rem;
	}

	.primary-author-container.viewer-is-primary-author {
		border-color: var(--primary);
		box-shadow: none;
	}

	.prim-author-label {
		position: absolute;
		top: 0%;
		transform: translateY(calc(-50% - 0.125rem));
		left: 2rem;
		padding: 0rem 0.25rem;
		font-size: 1rem;
		font-weight: 450;
		background: var(--back);
		border-radius: 10rem;
		color: var(--text);
	}
	.viewer-is-primary-author > .prim-author-label {
		color: var(--primary);
	}
	.profile-pic {
		aspect-ratio: 1;
		height: 100%;
		border-radius: 100vw;
		overflow: hidden;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
	}

	.main-content {
		display: grid;
		align-content: center;
	}

	.display-name {
		font-size: calc(1.25rem + 0.5vh);
		font-weight: 600;
		color: var(--text);
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

	.unique-name {
		font-size: 1.125rem;
		color: var(--muted-foreground);
		overflow: hidden;
		text-overflow: ellipsis;
		font-weight: 425;
	}
	.unique-name:hover {
		color: var(--primary);
	}

	.created-voki-date {
		margin-top: calc(0.125rem + 0.5vh);
		font-size: 1rem;
		color: var(--secondary-foreground);
	}

	.loading {
		pointer-events: none;
		opacity: 0.95;
		overflow: hidden;
	}

	.loading > * {
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
	}
	.loading > .main-content {
		margin-left: 0.5rem;
		height: 80%;
		width: 90%;
		border-radius: 1.5rem;
	}

	@keyframes shimmer {
		0% {
			transform: translateX(-70%);
		}
		100% {
			transform: translateX(70%);
		}
	}

	.loading::after {
		position: absolute;
		background: linear-gradient(
			-40deg,
			transparent 0%,
			transparent 25%,
			color-mix(in srgb, var(--secondary-foreground) 10%, var(--secondary) 15%) 50%,
			transparent 75%,
			transparent 100%
		);
		transform: translateX(-90%);
		animation: shimmer 1.3s infinite;
		content: '';
		inset: 0;
	}

	.primary-author-container.err {
		background:
			linear-gradient(var(--err-back), var(--err-back)) padding-box,
			linear-gradient(135deg, var(--err-foreground), var(--accent)) border-box;
		border: 0.125rem solid transparent;
		box-shadow: var(--err-shadow);
	}

	.err-view {
		background: var(--err-back);
		color: var(--err-foreground);
		padding: 0.5rem 0.75rem;
		border-radius: var(--radius);
		border: 0.09375rem solid var(--muted);
		box-shadow: var(--shadow-xs);
		font-size: 0.9375rem;
	}
</style>
