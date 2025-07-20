<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	const { userId }: { userId: string } = $props<{ userId: string }>();
</script>

<div class="invite-display">
	<img class="profile-picture" src={StorageBucketMain.fileSrc(`/profile-pictures/${userId}`)} />
	<a href={`/users/${userId}`} class="username">{userId}</a>
	<button class="cancel-btn">Cancel</button>
</div>

<style></style>
