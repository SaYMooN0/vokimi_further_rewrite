<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	interface Props {
		profilePic: string;
	}
	let { profilePic = $bindable() }: Props = $props();
</script>

<div class="profile-pic-step">
	<div class="preview-wrap">
		<div class="preview">
			<img src={StorageBucketMain.fileSrc(profilePic)} alt="Current profile pic" />
		</div>

		<label class="actions"> Choose custom </label>
	</div>
</div>

<style>
	.profile-pic-step {
		display: grid;
		gap: 1.25rem;
	}

	.preview-wrap {
		display: grid;
		justify-items: center;
		gap: 0.75rem;
		padding-block: 0.5rem 0.25rem;
	}

	.preview {
		inline-size: 9rem;
		block-size: 9rem;
		border-radius: 999rem;
		overflow: hidden;
		background: var(--back);
		display: grid;
		place-items: center;
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.preview img {
		inline-size: 100%;
		block-size: 100%;
		object-fit: cover;
	}
</style>
