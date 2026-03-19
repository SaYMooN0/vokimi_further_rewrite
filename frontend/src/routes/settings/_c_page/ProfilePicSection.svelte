<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { UserProfilePicData } from '$lib/ts/users';

	interface Props {
		profilePicData: UserProfilePicData;
	}
	let { profilePicData }: Props = $props();
</script>

<div class="profile-pic-section">
	<button>Edit</button>
</div>

<style>
	.profile-pic-section {
		display: flex;
		flex-direction: column;
		align-items: center;
	}
</style>
