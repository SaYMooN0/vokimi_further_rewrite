<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { ProfilePicShape } from '../types';

	interface Props {
		key: string;
		shape: ProfilePicShape;
	}
	let { key, shape }: Props = $props();
</script>

<div class="profile-picture">
	<img src={StorageBucketMain.fileSrc(key)} alt={`Profile picture`} />
</div>

<style>
	.profile-picture {
		width: 12rem;
		height: 12rem;
		flex-shrink: 0;
		margin-top: -5rem;
		position: relative;
	}

	.profile-picture img {
		display: block;
		width: 100%;
		height: 100%;
		object-fit: cover;
		border-radius: 999rem;
		border: 0.25rem solid var(--back);
	}
</style>
