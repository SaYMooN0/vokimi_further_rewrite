<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import type { UserProfilePreview } from '$lib/ts/users';
	import { getErrsViewDialogOpenFunction } from '../../routes/c_layout/ts_layout_contexts/errs-view-dialog-context';
	import { goto } from '$app/navigation';

	type ComponentInteraction = 'WholeComponentLink' | 'UniqueNameGotoOnClick' | 'JustDisplay';
	interface Props {
		userId: string;
		class?: string;
		interactionLevel: ComponentInteraction;
	}
	let { userId, class: className = undefined, interactionLevel }: Props = $props();
	let user = UsersStore.Get(userId);
	const openErrsViewDialog = getErrsViewDialogOpenFunction();

	function handleUniqueNameClick(e: MouseEvent) {
		if (interactionLevel !== 'UniqueNameGotoOnClick') {
			return;
		}

		//middle click
		if (e.button === 1) {
			window.open(`/authors/${userId}`, '_blank');
			return;
		}

		if (e.ctrlKey || e.metaKey) {
			window.open(`/authors/${userId}`, '_blank');
			return;
		}

		goto(`/authors/${userId}`);
	}

</script>

{#if user.state === 'ok'}
	{#if interactionLevel === 'WholeComponentLink'}
		<a class="user-display ok interactive-all {className}" href="/authors/{userId}">
			{@render okStateContent(user.data, true)}
		</a>
	{:else}
		<div class="user-display ok {className}">
			{@render okStateContent(user.data, interactionLevel === 'UniqueNameGotoOnClick')}
		</div>
	{/if}
{:else if user.state === 'errs'}
	<div
		class="user-display error interactive-all {className}"
		onclick={() => openErrsViewDialog(user.errs)}
	>
		<svg class="profile-pic">
			<use href="#common-crossed-circle-icon" />
		</svg>
		<label class="error-label"
			>Error in loading<svg><use href="#common-information-icon" /></svg></label
		>
	</div>
{:else if user.state === 'loading'}
	<div class="user-display loading">
		<div class="profile-pic skeleton-anim"></div>
		<div class="names-container-loading">
			<label class="skeleton-anim" />
			<label class="skeleton-anim" />
		</div>
	</div>
{/if}

{#snippet okStateContent(data: UserProfilePreview, isUniqueNameLink: boolean)}
	<img
		class="profile-pic"
		src={StorageBucketMain.fileSrc(data.profilePic)}
		alt="user profile pic"
	/>
	<div class="names">
		<label class="display-name">{data.displayName}</label>
		<label class="unique-name" onclick={handleUniqueNameClick} class:interactive={isUniqueNameLink}
			>@{data.uniqueName}</label
		>
	</div>
{/snippet}

<style>
	.user-display {
		--profile-pic-width: 2.75rem;

		display: inline-grid;
		align-items: center;
		gap: 0.25rem;
		width: fit-content;
		border-radius: 100vw;
		background-color: var(--back);
		line-height: default;
		text-decoration: none;
		grid-template-columns: var(--profile-pic-width) 1fr;
	}

	.user-display:not(.interactive-all) {
		cursor: default;
	}

	.user-display.interactive-all {
		cursor: pointer;
	}

	.user-display * {
		cursor: inherit;
	}

	.user-display .profile-pic {
		display: block;
		width: 100%;
		aspect-ratio: 1/1;
		border-radius: 50%;
		object-fit: cover;
	}

	.user-display.ok .names {
		display: flex;
		flex-direction: column;
		align-content: center;
		padding-top: 0.125rem;
		line-height: 1;
		text-indent: 0;
	}

	.user-display.ok .display-name {
		color: var(--text);
		font-size: 1rem;
		font-weight: 450;
	}

	.user-display.ok .unique-name {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 440;
	}

	.user-display.ok .unique-name.interactive:hover {
		color: var(--primary);
		cursor: pointer !important;
	}

	.user-display.ok.interactive-all:hover .unique-name {
		color: var(--primary);
	}

	.names-container-loading {
		display: grid;
		gap: 0.5rem;
		width: 8rem;
	}

	.names-container-loading > label {
		border-radius: 0.375rem;
	}

	.names-container-loading > label:nth-child(1) {
		width: 100%;
		height: 1rem;
	}

	.names-container-loading > label:nth-child(2) {
		width: 75%;
		height: 0.875rem;
	}

	.skeleton-anim {
		position: relative;
		background: var(--secondary);
		box-shadow: var(--shadow-xs);
		cursor: default;
		overflow: hidden;
	}

	.skeleton-anim::after {
		position: absolute;
		height: 100%;
		background: linear-gradient(
			120deg,
			transparent 0%,
			transparent 20%,
			color-mix(in srgb, var(--secondary-foreground) 10%, var(--secondary) 10%) 50%,
			transparent 80%,
			transparent 100%
		);
		opacity: 0.9;
		animation: shimmer 1.5s ease infinite;
		content: '';
		inset: 0;
		background-size: 200% 100%;
	}

	.user-display.error {
		color: var(--secondary-foreground);
	}

	.error .profile-pic {
		padding: 0.5rem;
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
		stroke-width: 1.25;
	}

	.error .error-label {
		width: max-content;
		padding: 0.125rem 0.5rem;
		border-radius: 0.375rem;
		font-size: 1rem;
		font-weight: 450;
	}

	.error-label > svg {
		width: 1.25rem;
		height: 1.25rem;
		padding: 0;
		padding-bottom: 0.125rem;
		margin-left: 0.125rem;
		color: inherit;
		stroke-width: 2;
		vertical-align: middle;
	}

	.user-display.error:hover .error-label {
		background-color: var(--secondary);
		color: var(--muted-foreground);
		text-decoration: underline;
	}

	@keyframes shimmer {
		from {
			background-position: 200% 0;
		}

		to {
			background-position: -200% 0;
		}
	}
</style>
