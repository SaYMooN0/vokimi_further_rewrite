<script lang="ts">
	const { cover }: { cover: string } = $props<{ cover: string }>();
</script>

<h1>{cover}</h1>

<div class="img-container">
    <img src={ImgUtils.imgUrlWithVersion(imgPath)} alt="voki cover" />
    <label for="voki-cover-input" class="img-btn change-img-btn">
        Change Test Cover
    </label>
    <input
        type="file"
        id="voki-cover-input"
        accept=".jpg,.png,.webp"
        hidden
        on:change={handleImageInputChange}
    />
    <button
        class="img-btn remove-img-btn"
        on:click={changeImageToDefault}
    >
        Set Cover To Default
    </button>

</div>

<style>
    .img-container {
        width: 600px;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 4px;
    }
    .img-container img {
        border-radius: 14px;
        width: 100%;
        height: 100%;
        object-fit: contain;
    }
    .change-img-btn,
    .remove-img-btn {
        height: 36px;
        width: 82%;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 16px;
        border: none;
        color: white;
        font-size: 20px;
        cursor: pointer;
        transition: all 0.12s ease-in;
    }
    .change-img-btn {
        margin-top: 8px;
        background-color: var(--primary);
    }
    .remove-img-btn {
        background-color: var(--red-del);
    }
    .change-img-btn:hover,
    .remove-img-btn:hover {
        width: 86%;
    }
</style>
