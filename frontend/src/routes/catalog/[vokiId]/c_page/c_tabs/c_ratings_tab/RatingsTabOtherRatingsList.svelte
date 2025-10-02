<script lang="ts">
	import { getAuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import type { VokiRatingData } from '../../../types';
	import VokiPageTabSectionLabel from '../c_tabs_shared/VokiPageTabSectionLabel.svelte';
	import RatingsListItem from './c_other_ratings/RatingsListItem.svelte';
	import OtherRatingsUserView from './c_other_ratings/RatingsListItem.svelte';

	interface Props {
		allRatings: VokiRatingData[];
	}
	let { allRatings }: Props = $props();

	async function filteredRatings() {
		const storeData = await getAuthStore();
		console.log('data', storeData);
		if (storeData.userId) {
			console.log('true');
			console.log('ratings', allRatings);
			console.log(storeData.userId);

			return allRatings.filter((r) => r.userId !== storeData.userId);
		} else {
			return allRatings;
		}
	}
</script>

{#await filteredRatings() then ratings}
	<div class="other-ratings-label-container">
		<VokiPageTabSectionLabel fieldName="Other user ratings:" />
	</div>
	{#if ratings.length === 0}
		<div>No one else has rated this Voki</div>
	{:else}
		<div class="ratings-list">
			{#each ratings as rating}
				<RatingsListItem {rating} />
			{/each}
		</div>
	{/if}
{/await}

<style>
	.other-ratings-label-container {
		display: flex;
		align-items: center;
		justify-content: space-between;
	}
</style>
