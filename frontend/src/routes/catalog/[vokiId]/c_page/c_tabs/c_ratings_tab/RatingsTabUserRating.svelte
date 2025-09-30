<script lang="ts">
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import type { Err } from '$lib/ts/err';
	import UserRatingFirstTime from './c_user_rating/UserRatingFirstTime.svelte';

	interface Props {
		saveNewUserRating: (
			value: number
		) => Promise<ResponseResult<{ value: number; dateTime: Date }>>;
		userRating: { value: number; dateTime: Date } | undefined;
	}
	let { saveNewUserRating, userRating }: Props = $props();
	let isEditing = $state(false);
	let savingErrs = $state<Err[]>([]);
	async function saveRating(newRating: number) {
		const response = await saveNewUserRating(newRating);
		if (response.isSuccess) {
			userRating = { value: response.data.value, dateTime: response.data.dateTime };
		} else {
			savingErrs = response.errs;
		}
	}
</script>

{#if isEditing}{:else if userRating}{:else}
	<UserRatingFirstTime saveNewUserRating={(newVal) => saveRating(newVal)} />
{/if}
