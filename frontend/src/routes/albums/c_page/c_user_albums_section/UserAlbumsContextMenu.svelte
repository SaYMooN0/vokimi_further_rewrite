<script lang="ts">
	import BaseContextMenu from '$lib/components/BaseContextMenu.svelte';
	import type { VokiAlbumPreviewData } from '../../types';

	interface Props {
		openEditAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openDeleteAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openCopyFromAnotherAlbumDialog: (album: VokiAlbumPreviewData) => void;
	}

	let { openEditAlbumDialog, openDeleteAlbumDialog, openCopyFromAnotherAlbumDialog }: Props =
		$props();
	let container: BaseContextMenu = $state<BaseContextMenu>()!;

	export function open(event: MouseEvent, album: VokiAlbumPreviewData) {
		currentAlbum = album;
		container.open(event.x, event.y, 8, -5);
	}
	let currentAlbum: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
</script>

<BaseContextMenu
	bind:this={container}
	onAfterClose={() => (currentAlbum = undefined)}
	class="album-context-menu"
>
	{#if currentAlbum}
		<div class="action" onclick={() => openEditAlbumDialog(currentAlbum!)}>
			<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" color="currentColor" fill="none">
				<path
					d="M16.4249 4.60509L17.4149 3.6151C18.2351 2.79497 19.5648 2.79497 20.3849 3.6151C21.205 4.43524 21.205 5.76493 20.3849 6.58507L19.3949 7.57506M16.4249 4.60509L9.76558 11.2644C9.25807 11.772 8.89804 12.4078 8.72397 13.1041L8 16L10.8959 15.276C11.5922 15.102 12.228 14.7419 12.7356 14.2344L19.3949 7.57506M16.4249 4.60509L19.3949 7.57506"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
				<path
					d="M18.9999 13.5C18.9999 16.7875 18.9999 18.4312 18.092 19.5376C17.9258 19.7401 17.7401 19.9258 17.5375 20.092C16.4312 21 14.7874 21 11.4999 21H11C7.22876 21 5.34316 21 4.17159 19.8284C3.00003 18.6569 3 16.7712 3 13V12.5C3 9.21252 3 7.56879 3.90794 6.46244C4.07417 6.2599 4.2599 6.07417 4.46244 5.90794C5.56879 5 7.21252 5 10.5 5"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>Edit
		</div>
		<div class="action" onclick={() => openCopyFromAnotherAlbumDialog(currentAlbum!)}>
			<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" color="currentColor" fill="none">
				<path
					d="M2 12V6.94975C2 6.06722 2 5.62595 2.06935 5.25839C2.37464 3.64031 3.64031 2.37464 5.25839 2.06935C5.62595 2 6.06722 2 6.94975 2C7.33642 2 7.52976 2 7.71557 2.01738C8.51665 2.09229 9.27652 2.40704 9.89594 2.92051C10.0396 3.03961 10.1763 3.17633 10.4497 3.44975L11 4C11.8158 4.81578 12.2237 5.22367 12.7121 5.49543C12.9804 5.64471 13.2651 5.7626 13.5604 5.84678C14.0979 6 14.6747 6 15.8284 6H16.2021C18.8345 6 20.1506 6 21.0062 6.76946C21.0849 6.84024 21.1598 6.91514 21.2305 6.99383C22 7.84935 22 9.16554 22 11.7979V14C22 17.7712 22 19.6569 20.8284 20.8284C19.6569 22 17.7712 22 14 22H10C6.22876 22 4.34315 22 3.17157 20.8284C2.51839 20.1752 2.22937 19.3001 2.10149 18"
					stroke="currentColor"
					stroke-linecap="round"
				/>
				<path
					d="M2 15C8.44365 15 6.55635 15 13 15M13 15L8.875 12M13 15L8.875 18"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>Copy from another album
		</div>
		<a class="action" href="/albums/{currentAlbum.id}">
			<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" color="currentColor" fill="none">
				<path
					d="M11.0991 3.00012C7.45013 3.00669 5.53932 3.09629 4.31817 4.31764C3.00034 5.63568 3.00034 7.75704 3.00034 11.9997C3.00034 16.2424 3.00034 18.3638 4.31817 19.6818C5.63599 20.9999 7.75701 20.9999 11.9991 20.9999C16.241 20.9999 18.3621 20.9999 19.6799 19.6818C20.901 18.4605 20.9906 16.5493 20.9972 12.8998"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				></path>
				<path
					d="M20.556 3.49612L11.0487 13.0586M20.556 3.49612C20.062 3.00151 16.7343 3.04761 16.0308 3.05762M20.556 3.49612C21.05 3.99074 21.0039 7.32273 20.9939 8.02714"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				></path>
			</svg>Open
		</a>
		<div class="action delete" onclick={() => openDeleteAlbumDialog(currentAlbum!)}>
			<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
				<path
					d="M9.1709 4C9.58273 2.83481 10.694 2 12.0002 2C13.3064 2 14.4177 2.83481 14.8295 4"
					stroke="currentColor"
					stroke-linecap="round"
				/>
				<path d="M20.5001 6H3.5" stroke="currentColor" stroke-linecap="round" />
				<path
					d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
					stroke="currentColor"
					stroke-linecap="round"
				/>
				<path d="M9.5 11L10 16" stroke="currentColor" stroke-linecap="round" />
				<path d="M14.5 11L14 16" stroke="currentColor" stroke-linecap="round" />
			</svg>Delete
		</div>
	{:else}
		<svg class="no-album-icon">
			<use href="#common-crossed-circle-icon" />
		</svg>

		<label class="no-album-selected-label">No album selected</label>
	{/if}
</BaseContextMenu>

<style>
	:global(.album-context-menu:has(.action)),
	:global(.album-context-menu:has(.no-album-selected-label)) {
		width: max-content;
		color: var(--muted-foreground);
		background-color: var(--back);
		display: grid;
	}
	:global(.album-context-menu:has(.action)) {
		grid-template-rows: 1fr 1fr;
		border-radius: 0.25rem;
		box-shadow: var(--shadow-xs);

		gap: 0.125rem;
	}
	.action {
		padding: 0.25rem 1.25rem 0.25rem 0.5rem;
		cursor: default;
		border-radius: inherit;
		font-size: 1rem;
		font-weight: 410;
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 0.375rem;
		align-items: center;
		color: inherit;
	}
	.action svg {
		height: 1.125rem;
		width: 1.125rem;
		color: inherit;
		stroke-width: 1.675;
		display: inline;
	}
	.action:hover {
		background-color: var(--accent);
		color: var(--primary);
	}
	.action.delete:hover {
		background-color: var(--err-back);
		color: var(--err-foreground);
	}
	:global(.album-context-menu:has(.no-album-selected-label)) {
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.5rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs), var(--shadow);
		padding: 0.25rem 0.75rem;
	}
	.no-album-icon {
		height: 1.25rem;
		width: 1.25rem;
	}
</style>
