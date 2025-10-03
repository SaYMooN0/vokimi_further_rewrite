<script lang="ts">
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import type { Err } from '$lib/ts/err';
	import VokiPageTabSectionLabel from '../c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import RatingStarsView from './c_ratings_shared/RatingStarsView.svelte';
	import UserRatingEditingState from './c_user_rating/UserRatingEditingState.svelte';
	import UserRatingErrsList from './c_user_rating/UserRatingErrsList.svelte';

	interface Props {
		saveNewUserRating: (
			value: number
		) => Promise<ResponseResult<{ value: number; dateTime: Date }>>;
		userRating: { value: number; dateTime: Date } | undefined;
	}
	let { saveNewUserRating, userRating }: Props = $props();
	let isEditing = $state(false);
	let savingErrs = $state<Err[]>([]);
	async function saveRating() {
		if (newStarsValue < 1 || newStarsValue > 5) {
			savingErrs = [{ message: `Rating must be between 1 and 5. Chosen value: ${newStarsValue}` }];
			return;
		}
		const response = await saveNewUserRating(newStarsValue);
		if (response.isSuccess) {
			userRating = { value: response.data.value, dateTime: response.data.dateTime };
			isEditing = false;
		} else {
			savingErrs = response.errs;
		}
	}
	let newStarsValue = $state(userRating ? userRating.value : 0);
	$effect(() => {
		newStarsValue;
		savingErrs = [];
	});
</script>

{#if !isEditing && userRating}
	<div class="user-rating">
		<VokiPageTabSectionLabel fieldName="Your rating:" />
		<RatingStarsView value={userRating.value} />
		<button class="change-rating-btn" onclick={() => (isEditing = true)}>Change rating</button>
	</div>
{:else}
	<UserRatingEditingState
		bind:starsValue={newStarsValue}
		onSaveButtonClicked={() => saveRating()}
		onCancelButtonClick={() => {
			if (userRating) {
				isEditing = false;
			} else {
				newStarsValue = 0;
			}
		}}
	/>
{/if}
{#if savingErrs.length > 0}
	<UserRatingErrsList errs={savingErrs} clearErrs={() => (savingErrs = [])} />
{/if}

<style>
	.user-rating {
		display: flex;
		flex-direction: row;
		align-items: center;
		font-size: 1.25rem;
		font-weight: 450;
	}
	
	.change-rating-btn {
		margin-left: auto;
		color: var(--primary-foreground);
		background-color: var(--primary);
		border: none;
		padding: 0.25rem 0.75rem;
		border-radius: 0.25rem;
		font-size: 0.875rem;
		font-weight: 450;
		letter-spacing: 0.1px;
		cursor: pointer;
	}
	.change-rating-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
