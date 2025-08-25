<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki';
	import AddToAlbumButtonIcon from './c_cover_section_icons/AddToAlbumButtonIcon.svelte';
	import AlbumsBlockedButtonIcon from './c_cover_section_icons/AlbumsBlockedButtonIcon.svelte';
	import ManageVokiButtonIcon from './c_cover_section_icons/ManageVokiButtonIcon.svelte';
	import TakeVokiButtonIcon from './c_cover_section_icons/TakeVokiButtonIcon.svelte';
	interface Props {
		vokiId: string;
		cover: string;
		usersWithAccessToManage: string[];
		vokiType: VokiType;
	}
	let { vokiId, cover, usersWithAccessToManage, vokiType }: Props = $props();
</script>

<div class="voki-cover-section">
	<img class="voki-cover" src={StorageBucketMain.fileSrc(cover)} alt="voki cover" />
	<div class="buttons-container">
		<a href="/take-voki/{StringUtils.pascalToKebab(vokiType)}/{vokiId}" class="take-voki-btn">
			<TakeVokiButtonIcon />
			<label class="btn-text">Take voki</label>
		</a>
		<AuthView>
			{#snippet authenticated(authData)}
				<button class="add-to-album-btn">
					<AddToAlbumButtonIcon />
					<label class="btn-text">Add to album</label>
				</button>
				{#if usersWithAccessToManage.includes(authData.userId)}
					<a href="/manage-voki/{vokiId}" class="manage-voki-btn">
						<ManageVokiButtonIcon />
						<label class="btn-text">Manage voki</label>
					</a>
				{/if}
			{/snippet}
			{#snippet unauthenticated()}
				<button class="add-to-album-btn" disabled>
					<AlbumsBlockedButtonIcon />
					<label class="btn-text">Add to album</label>
				</button>
			{/snippet}
		</AuthView>
	</div>
</div>

<style>
	.voki-cover-section {
		width: 100%;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
	}
	.voki-cover-section > img {
		width: 25rem;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}
	.buttons-container {
		width: 100%;
		display: flex;
		gap: 0.5rem;
		flex-direction: column;
	}
	.buttons-container > * {
		height: 2rem;
		width: 100%;
		border-radius: 0.25rem;
		border: none;
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
	}
	.buttons-container > * > :global(svg) {
		height: 2rem;
		width: 2rem;
		margin-right: 0.5rem;
	}
	.btn-text {
		font-size: 1.375rem;
		width: 8.5rem;
	}
	.take-voki-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
	.add-to-album-btn {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}
	.manage-voki-btn {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}
</style>
