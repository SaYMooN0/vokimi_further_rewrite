<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { relativeTime } from 'svelte-relative-time';
	import RatingStarsView from './RatingStarsView.svelte';
	import type { Snippet } from 'svelte';

	interface Props {
		userId: string;
		content: { name: 'default'; ratingValue: number } | { name: 'custom'; children: Snippet };
		dateTime?: Date;
		highlight?: { enabled: false } | { enabled: true; text: string };
	}
	let { userId, content, dateTime = undefined, highlight = { enabled: false } }: Props = $props();

	let user: any = UsersStore.Get(userId);
</script>

<div class="rating-item" class:highlighted={highlight.enabled}>
	{#if highlight.enabled}
		<label class="highlight-label"> {highlight.text}</label>
	{/if}
	<div class="profile-pic-container">
		{#if user.state === 'loading'}
			<div class="profile-pic skeleton" aria-hidden="true" />
		{:else if user.state === 'errs'}
			<div class="profile-pic err">
				<svg><use href="#common-crossed-circle-icon" /></svg>
			</div>
		{:else if user.state === 'ok'}
			<img
				class="profile-pic"
				src={StorageBucketMain.fileSrc(user.data.profilePic)}
				alt="user profile pic"
			/>
		{/if}
	</div>

	<div class="content">
		{#if user.state === 'loading'}
			<div class="username loading skeleton" aria-hidden="true"></div>
		{:else if user.state === 'errs'}
			<label class="username error">Could not load user name</label>
		{:else if user.state === 'ok'}
			<label class="username">{user.data.name}</label>
		{/if}
		{#if content.name === 'default'}
			<RatingStarsView value={content.ratingValue} />
		{:else if content.name === 'custom'}
			{@render content.children()}
		{:else}
			<h1>Something went wrong. Content name: {(content as any).name}</h1>
		{/if}
	</div>
	<div class="date">
		{#if dateTime}
			<span use:relativeTime={{ date: dateTime }} />
		{/if}
	</div>
</div>

<style>
	.rating-item {
		position: relative;
		display: grid;
		align-items: center;
		padding: 0.25rem 0.5rem;
		border: 0.125rem solid var(--back);
		border-radius: 1rem;
		animation: var(--default-fade-in);
		grid-template-columns: auto 1fr auto;
		row-gap: 0.25rem;
	}

	.rating-item.highlighted {
		padding-top: 0.675rem;
		border-color: var(--primary);
	}

	.highlight-label {
		position: absolute;
		top: -0.125rem;
		left: 1.25rem;
		padding: 0 0.125rem;
		background-color: var(--back);
		color: var(--primary);
		font-size: 0.875rem;
		font-weight: 600;
		transform: translateY(-50%);
	}

	.profile-pic-container {
		width: 3rem;
		height: 3rem;
		padding-top: 0.25rem;
	}

	.profile-pic {
		display: grid;
		height: 100%;
		border-radius: 100vw;
		background: var(--muted);
		aspect-ratio: 1/1;
		object-fit: cover;
		place-items: center;
	}

	img.profile-pic {
		box-shadow: var(--shadow-xs) inset;
	}

	.profile-pic.err {
		background: var(--muted);
	}

	.profile-pic.err svg {
		width: 1rem;
		height: 1rem;
		color: var(--muted-foreground);
	}

	.content {
		align-self: center;
		min-width: 0;
	}

	.username {
		display: block;
		margin-left: -0.125rem;
		color: var(--text);
		font-weight: 600;
		line-height: 1.1;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.username.loading {
		width: 9rem;
		height: 1.125rem;
		margin-left: 0.125rem;
		border-radius: 0.375rem;
		color: transparent;
	}

	.username.error {
		color: var(--muted-foreground);
	}

	.date {
		color: var(--secondary-foreground);
		font-size: 1rem;
		white-space: nowrap;
	}

	.skeleton {
		position: relative;
		width: 100%;
		background: var(--muted);
		overflow: hidden;
	}

	.skeleton::before {
		position: absolute;
		width: 150%;
		background: linear-gradient(
			-65deg,
			var(--muted) 0%,
			var(--muted) 15%,
			color-mix(in oklab, var(--secondary) 35%, var(--muted)) 30%,
			color-mix(in oklab, var(--back) 70%, var(--muted)) 50%,
			color-mix(in oklab, var(--secondary) 35%, var(--muted)) 70%,
			var(--muted) 85%,
			var(--muted) 100%
		);
		animation: skeleton-sweep 2s infinite;
		content: '';
		inset: 0;
	}

	@keyframes skeleton-sweep {
		0% {
			transform: translateX(-120%);
		}

		100% {
			transform: translateX(120%);
		}
	}
</style>
