<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { relativeTime } from 'svelte-relative-time';
	import type { VokiRatingData } from '../../../../types';
	import RatingStarsView from '../c_ratings_shared/RatingStarsView.svelte';

	let { rating }: { rating: VokiRatingData } = $props<{ rating: VokiRatingData }>();
	let user: any = UsersStore.Get(rating.userId);
</script>

<div class="rating-item">
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

		<div class="stars">
			<RatingStarsView value={rating.value} />
		</div>
	</div>
	<span class="date" use:relativeTime={{ date: rating.dateTime }} />
</div>

<style>
	.rating-item {
		display: grid;
		grid-template-columns: auto 1fr auto;
		row-gap: 0.25rem;
		align-items: center;
		padding: 0 0.25rem;
		animation: var(--default-fade-in);
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
