<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';

	let { userId }: { userId: string } = $props<{ userId: string }>();
	let user = UsersStore.Get(userId);
</script>

<a
	class="author-view"
	href="/authors/{userId}"
	class:loading={user.state === 'loading'}
	class:error={user.state === 'errs'}
	class:ok={user.state === 'ok'}
>
	{#if user.state === 'loading'}
		<div class="profile-pic"></div>
		<label></label>
	{:else if user.state === 'errs'}
		<svg class="profile-pic">
			<use href="#common-crossed-circle-icon" />
		</svg>
		<label class="">Error in loading</label>
	{:else if user.state === 'ok'}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(user.data.profilePic)}
			alt="user profile pic"
		/>
		<div class="names">
			<label class="display-name">{user.data.displayName}</label>
			<label class="unique-name">@{user.data.uniqueName}</label>
		</div>
	{/if}
</a>

<style>
	.author-view {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.25rem 0.5rem 0.25rem 0.25rem;
		border-radius: 1rem;
		background-color: var(--back);
		text-decoration: none;
		box-shadow: var(--shadow-xs);
		transition:
			box-shadow 0.15s ease-in-out,
			background-color 0.15s ease-in-out,
			transform 0.08s ease-in-out;
		cursor: default;
	}

	.author-view:hover {
		background-color: var(--secondary);
	}

	.author-view .profile-pic {
		display: block;
		width: 2.375rem;
		height: 2.375rem;
		border-radius: 50%;
		cursor: inherit;
	}

	.author-view.loading .profile-pic {
		position: relative;
		background: var(--muted);
		cursor: inherit;
		overflow: hidden;
	}

	.author-view.loading label {
		position: relative;
		width: 8rem;
		height: 1.25rem;
		margin-right: 0.375rem;
		border-radius: 0.375rem;
		background-color: var(--muted);
		overflow: hidden;
	}

	.author-view.loading > *::after {
		position: absolute;
		background: linear-gradient(100deg, transparent 20%, var(--secondary) 40%, transparent 60%);
		opacity: 0.9;
		animation: shimmer 1.2s ease-in-out infinite;
		content: '';
		inset: 0;
		background-size: 200% 100%;
	}

	.author-view.ok .profile-pic {
		object-fit: cover;
		box-shadow: var(--shadow);
	}

	.author-view.ok .names {
		display: flex;
		flex-direction: column;
		align-content: center;
	}

	.author-view.ok .display-name {
		color: var(--text);
		font-size: 1rem;
		font-weight: 450;
	}

	.author-view.ok .unique-name {
		color: var(--muted-foreground);
		font-size: 0.875rem;
		font-weight: 440;
	}

	.author-view.ok:hover .unique-name {
		color: var(--primary);
	}

	.author-view.error {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
	}

	.author-view.error label {
		font-size: 1.125rem;
		font-weight: 500;
	}

	.author-view.error svg {
		padding: 0.25rem;
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
