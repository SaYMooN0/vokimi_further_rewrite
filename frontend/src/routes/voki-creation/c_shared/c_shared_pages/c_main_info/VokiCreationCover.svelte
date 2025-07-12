<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-services';
	import { ApiVokimiStorage } from '$lib/ts/storage-service';
	import { toast } from 'svelte-sonner';

	let { cover, vokiId }: { cover: string; vokiId: string } = $props<{
		cover: string;
		vokiId: string;
	}>();

	async function handleImageInputChange(event: Event) {
		const input = event.target as HTMLInputElement;
		if (input.files && input.files.length > 0) {
			// await updateCover(input.files[0]);
		}
	}
	async function changeImageToDefault() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ newVokiCover: string }>(
			`/vokis/${vokiId}/set-cover-to-default`,
			{ method: 'PATCH' }
		);
		if (response.isSuccess) {
			cover = response.data.newVokiCover;
		} else {
			toast.error("Couldn't set voki cover to default");
		}
	}
</script>

<div class="img-container">
	<img src={ApiVokimiStorage.fileSrcWithVersion(cover)} alt="voki cover" />
	<label for="voki-cover-input" class="img-btn change-btn">Change cover</label>
	<input
		type="file"
		id="voki-cover-input"
		accept=".jpg,.png,.webp"
		hidden
		onchange={handleImageInputChange}
	/>
	<button class="img-btn set-default-btn" onclick={changeImageToDefault}>Set to default </button>
</div>

<style>
	.img-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 30rem;
	}

	.img-container img {
		width: 100%;
		height: 100%;
		border-radius: 1rem;
		object-fit: contain;
	}

	.img-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		box-shadow: var(--shadow);
		cursor: pointer;
		font-weight: 420;
		letter-spacing: 0.2px;
	}

	.change-btn {
		margin-top: 1rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.set-default-btn {
		margin-top: 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.set-default-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
