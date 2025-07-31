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
		width: fit-content;
		height: fit-content;
		border: 0.25rem solid var(--muted);
		border-radius: 1rem;
        display: flex;
        justify-content: center;
        align-items: center;
	}
	.img-view > img {
		max-width: 14rem;
		max-height: 18rem;
		object-fit: contain;
		z-index: 1;
		border-radius: 0.75rem;
	}

	.remove-img-btn {
		z-index: 2;
		position: absolute;
		right: 0;
		top: 0;
		transform: translate(40%, -40%);
		padding: 0.25rem;
		border-radius: 50%;
		background-color: var(--secondary-foreground);
		cursor: pointer;
		outline: none;
		border: none;
		display: flex;
		justify-content: center;
		align-items: center;
		transition: opacity  0.08s ease;
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
