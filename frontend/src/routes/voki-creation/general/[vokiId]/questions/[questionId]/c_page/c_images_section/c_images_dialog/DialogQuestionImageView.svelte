<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	const { img, removeImg }: { img: string; removeImg: (image: string) => void } = $props<{
		img: string;
		removeImg: (image: string) => void;
	}>();
</script>

<div class="img-view">
	<img alt="" src={StorageBucketMain.fileSrc(img)} />
	<button class="remove-img-btn" onclick={() => removeImg(img)}>
		<svg> <use href="#common-cross-icon" /></svg>
	</button>
</div>

<style>
	.img-view {
		position: relative;
        display: flex;
        justify-content: center;
        align-items: center;
		width: fit-content;
		height: fit-content;
		border: 0.25rem solid var(--muted);
		border-radius: 1rem;
	}

	.img-view > img {
		z-index: 1;
		max-width: 14rem;
		max-height: 18rem;
		border-radius: 0.75rem;
		object-fit: contain;
	}

	.remove-img-btn {
		position: absolute;
		top: 0;
		right: 0;
		z-index: 2;
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.25rem;
		border: none;
		border-radius: 50%;
		background-color: var(--secondary-foreground);
		transition: opacity  0.08s ease;
		transform: translate(40%, -40%);
		cursor: pointer;
		outline: none;
	}

	.img-view:has(img:hover) .remove-img-btn {
		opacity: 0.3;
	}

	.remove-img-btn:hover {
		background-color: var(--primary-hov);
	}

	.remove-img-btn > svg {
		width: 1rem;
		height: 1rem;
		color: var(--primary-foreground);
		stroke-width: 2.3;
	}
</style>
