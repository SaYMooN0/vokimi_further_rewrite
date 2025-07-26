<script lang="ts">
	const { img, removeImage }: { img: string; removeImage: (image: string) => void } = $props<{
		img: string;
		removeImage: (image: string) => void;
	}>();
</script>

<div class="img-view">
	<img src={img} alt="" />
	<button class="remove-img-btn" onclick={() => removeImage(img)}>
		<svg> <use href="#common-cross-icon" /></svg>
	</button>
</div>

<style>
    .img-view{
        position: relative;
        width: 12rem;
        height: 6rem;
    }

    .remove-img-btn{
        position: absolute;
        top: 0;
        right: 0;
        padding: 0.25rem;
        border-radius: 0.5rem;
        background-color: var(--primary);
        cursor: pointer;
    }
</style>