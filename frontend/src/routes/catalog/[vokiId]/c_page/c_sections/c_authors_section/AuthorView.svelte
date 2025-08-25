<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { UserPreviewData } from '$lib/ts/stores/users-store-svelte';

	let { user }: { user: UserPreviewData } = $props<{ user: UserPreviewData }>();
</script>

<a class="author-view" href="/authors/{user.id}">
	<img src={StorageBucketMain.fileSrc(user.profilePic)} alt="user profile pic" />
	<label>{user.name}</label>
</a>

<style>
	.author-view {
		height: auto;
		display: flex;
		flex-direction: row;
		align-items: center;
		width: fit-content;
		border-radius: 1.5rem;
		padding: 0.25rem;
		box-shadow: var(--shadow-xs);
		gap: 0.25rem;
		cursor: pointer;
	}

	.author-view > img {
		height: 2.5rem;
		aspect-ratio: 1/1;
		object-fit: fill;
		border-radius: 50%;
	}
	.author-view > label {
		cursor: inherit;
		font-size: 1.25rem;
		font-weight: 450;
		color: var(--text);
		padding-right: 0.375rem;
	}
	.author-view:hover {
		box-shadow: var(--shadow-md), var(--shadow-xs);
	}
</style>
