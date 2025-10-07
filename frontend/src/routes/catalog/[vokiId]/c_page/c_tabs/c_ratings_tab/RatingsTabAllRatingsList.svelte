<script lang="ts">
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import { getAuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { getSignInDialogOpenFunction } from '../../../../../c_layout/ts_layout_contexts/sign-in-dialog-context';
	import type { VokiRatingData } from '../../../types';
	import RatingsListItem from './c_all_ratings/RatingsListItem.svelte';
	import RatingsTabUserRating from './c_all_ratings/RatingsTabUserRating.svelte';
	import UserRatingCannotRateMessage from './c_all_ratings/UserRatingCannotRateMessage.svelte';

	interface Props {
		allRatings: VokiRatingData[];
		hasUserTakenVoki: boolean;
		saveNewRating: (value: number) => Promise<ResponseResult<VokiRatingData>>;
	}
	let { allRatings, hasUserTakenVoki, saveNewRating }: Props = $props();

	type UserRatingStateType =
		| { name: 'not-taken' }
		| { name: 'unauthenticated' }
		| ({ name: 'rated' } & VokiRatingData)
		| { name: 'not-rated'; userId: string };

	async function processRatings(): Promise<{
		userRatingState: UserRatingStateType;
		otherUserRatings: VokiRatingData[];
	}> {
		const storeData = await getAuthStore();
		if (storeData.isAuthenticated && hasUserTakenVoki) {
			const userRatingValue = allRatings.find((r) => r.userId === storeData.userId);
			if (userRatingValue) {
				return {
					userRatingState: {
						name: 'rated',
						...userRatingValue
					},
					otherUserRatings: allRatings.filter((r) => r.userId !== storeData.userId)
				};
			}
			return {
				userRatingState: {
					name: 'not-rated',
					userId: storeData.userId!
				},
				otherUserRatings: allRatings
			};
		} else if (storeData.isAuthenticated && !hasUserTakenVoki) {
			{
				return {
					userRatingState: { name: 'not-taken' },
					otherUserRatings: allRatings
				};
			}
		} else {
			return {
				userRatingState: { name: 'unauthenticated' },
				otherUserRatings: allRatings
			};
		}
	}
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

{#await processRatings()}
	<label>Loading...</label>
{:then ratings}
	{#if ratings.userRatingState.name === 'unauthenticated'}
		<UserRatingCannotRateMessage mainText="To rate Vokis you need to be logged in">
			<div class="auth-needed-to-rate-btns">
				<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
				<button class="signup-btn" onclick={() => openSignInDialog('signup')}
					>I don't have an account yet</button
				>
			</div>
		</UserRatingCannotRateMessage>
	{:else if ratings.userRatingState.name === 'not-taken'}
		<UserRatingCannotRateMessage mainText="To rate this Voki you need to take it first" />
	{:else}
		<RatingsTabUserRating ratingState={ratings.userRatingState} {saveNewRating} />
	{/if}
	{#if ratings.userRatingState.name === 'rated' && ratings.otherUserRatings.length === 0}
		<div class="no-other-ratings">No one else has rated this Voki</div>
	{:else if ratings.otherUserRatings.length != 0}
		{#each ratings.otherUserRatings as rating}
			<RatingsListItem
				userId={rating.userId}
				content={{ name: 'default', ratingValue: rating.value }}
				dateTime={rating.dateTime}
			/>
		{/each}
	{/if}
{/await}

<style>
	.auth-needed-to-rate-btns {
		display: flex;
		justify-items: center;
		gap: 2rem;
	}

	.auth-needed-to-rate-btns > button {
		padding: 0.25rem 1rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 420;
		cursor: pointer;
	}

	.auth-needed-to-rate-btns > button:hover {
		background-color: var(--primary-hov);
	}

	.no-other-ratings {
		margin: 0 auto;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}
</style>
