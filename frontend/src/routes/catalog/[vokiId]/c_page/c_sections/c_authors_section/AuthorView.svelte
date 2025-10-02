<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore, type UserPreviewData } from '$lib/ts/stores/users-store.svelte';

	let { userId }: { userId: string } = $props<{ userId: string }>();
	let user = UsersStore.Get(userId);
</script>

{#if user.state === 'loading'}
	<label>loading user...</label>
{:else if user.state === 'errs'}
	<label>error in loading <a href="/authors/{userId}">user</a></label>
{:else if user.state === 'ok'}
	<a class="author-view" href="/authors/{userId}">
		<img src={StorageBucketMain.fileSrc(user.data.profilePic)} alt="user profile pic" />
		<label>{user.data.name}</label>
	</a>
{/if}

<style>
	.author-view {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.25rem;
		width: fit-content;
		height: auto;
		padding: 0.25rem;
		border-radius: 1.5rem;
		box-shadow: var(--shadow-xs);
		cursor: pointer;
	}

	.author-view > img {
		height: 2.5rem;
		aspect-ratio: 1/1;
		object-fit: fill;
		border-radius: 50%;
	}

	.author-view > label {
		padding-right: 0.375rem;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 450;
		cursor: inherit;
	}

	.author-view:hover {
		box-shadow: var(--shadow-md), var(--shadow-xs);
	}
</style>
