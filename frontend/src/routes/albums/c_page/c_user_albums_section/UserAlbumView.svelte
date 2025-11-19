<script lang="ts">
	import type { VokiAlbumPreviewData } from '../../types';
	let { album, openContextMenu }: Props = $props();
	interface Props {
		album: VokiAlbumPreviewData;
		openContextMenu: (event: MouseEvent, album: VokiAlbumPreviewData) => void;
	}

	const handleMenuClick = (event: MouseEvent) => {
		event.preventDefault();
		event.stopPropagation();
		openContextMenu(event, album);
	};
</script>

<a class="album-card" href={`/albums/${album.id}`}>
	<svg
		class="album-icon"
		style="--icon-color-1: {album.mainColor}; --icon-color-2: {album.secondaryColor};"
	>
		<use href={`#${album.icon}`} />
	</svg>

	<div class="album-main">
		<p class="album-name">{album.name}</p>
		<p class="album-meta">
			{album.vokisCount === 1 ? '1 Voki' : `${album.vokisCount} Vokis`}
		</p>
	</div>

	<button
		type="button"
		class="album-menu-button"
		aria-label="Open album menu"
		onclick={(event) => handleMenuClick(event)}
	>
		<span class="menu-dot" />
		<span class="menu-dot" />
		<span class="menu-dot" />
	</button>
</a>

<style>
	.album-card {
		display: grid;
		grid-template-columns: auto 1fr 2rem;
		gap: 0.75rem;
		padding: 0.75rem 1rem;
		border-radius: 0.75rem;
		overflow: hidden;
		color: var(--text);
		box-shadow: var(--shadow-xs);
		align-items: center;
	}
	.album-card:hover {
		background-color: var(--secondary);
	}

	.album-icon {
		width: 2.25rem;
		height: 2.25rem;
		stroke-width: 2;
	}

	.album-main {
		min-width: 0;
		display: flex;
		flex-direction: column;
		gap: 0.125rem;
	}

	.album-name {
		font-size: 1.25rem;
		font-weight: 550;
		color: var(--text);
		white-space: nowrap;
		text-overflow: ellipsis;
		overflow: hidden;
	}

	.album-meta {
		font-size: 0.875rem;
		color: var(--muted-foreground);
	}

	.album-menu-button {
		padding: 0.25rem;
		border: none;
		width: 1.875rem;
		height: 1.875rem;
		background-color: transparent;
		display: inline-flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		cursor: pointer;
		border-radius: 0.375rem;
	}

	.album-menu-button:hover {
		background-color: var(--muted);
	}

	.menu-dot {
		margin: auto auto;
		width: 0.125rem;
		height: 0.125rem;
		border-radius: 9999rem;
		background-color: var(--muted-foreground);
	}
</style>
