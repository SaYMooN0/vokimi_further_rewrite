<script lang="ts">
	import ContextMenuWithActions, {
		type ActionsContextMenuActionsContent,
		type ActionsContextMenuJustContent
	} from '$lib/components/context_menus/ContextMenuWithActions.svelte';
	import type { VokiAlbumPreviewData } from '../../types';

	interface Props {
		openEditAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openDeleteAlbumDialog: (album: VokiAlbumPreviewData) => void;
		openCopyFromAnotherAlbumDialog: (album: VokiAlbumPreviewData) => void;
	}

	let { openEditAlbumDialog, openDeleteAlbumDialog, openCopyFromAnotherAlbumDialog }: Props =
		$props();
	let contextMenu = $state<ContextMenuWithActions>()!;

	export function open(event: MouseEvent, album: VokiAlbumPreviewData) {
		currentAlbum = album;
		contextMenu.open(event.x, event.y, 8, -5);
	}
	let currentAlbum: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
	let menuContent: ActionsContextMenuJustContent | ActionsContextMenuActionsContent = $derived(
		currentAlbum
			? {
					type: 'actions',
					items: [
						{
							label: 'Edit',
							iconHref: '#context-menu-edit-icon',
							action: {
								isLink: false,
								onclick: () => openEditAlbumDialog(currentAlbum!)
							},
							type: 'default'
						},
						{
							label: 'Copy from another album',
							iconHref: '#context-menu-copy-icon',
							action: {
								isLink: false,
								onclick: () => openCopyFromAnotherAlbumDialog(currentAlbum!)
							},
							type: 'default'
						},
						{
							label: 'Open',
							iconHref: '#context-menu-open-icon',
							action: {
								isLink: true,
								href: `/albums/${currentAlbum!.id}`
							},
							type: 'default'
						},
						{
							label: 'Delete',
							iconHref: '#context-menu-delete-icon',
							action: {
								isLink: false,
								onclick: () => openDeleteAlbumDialog(currentAlbum!)
							},
							type: 'red'
						}
					]
				}
			: {
					type: 'content',
					content: noAlbumContent
				}
	);
</script>

<ContextMenuWithActions
	bind:this={contextMenu}
	content={menuContent}
	id="user-album-context-menu"
/>

{#snippet noAlbumContent()}
	<svg class="no-album-icon">
		<use href="#common-crossed-circle-icon" />
	</svg>

	<label class="no-album-selected-label">No album selected</label>
{/snippet}

<style>
	:global(#user-album-context-menu:has(.no-album-selected-label)) {
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 0.75rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs), var(--shadow);
		grid-template-columns: auto 1fr;
	}

	.no-album-icon {
		width: 1.25rem;
		height: 1.25rem;
	}
</style>
