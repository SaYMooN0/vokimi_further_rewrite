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
	<div
		class="album-icon-wrapper"
		style="--icon-color-1: {album.mainColor}; --icon-color-2: {album.secondaryColor};"
	>
		<svg class="album-icon">
			<use href={`#${album.icon}`} />
		</svg>
	</div>

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
		display: block;
		padding: 1rem;
		border-radius: var(--radius);
		overflow: hidden;
		text-decoration: none;
		color: var(--text);
		box-shadow: var(--shadow-xs);
	}

	.album-icon-wrapper {
		flex: 0 0 auto;
		width: 2.75rem;
		height: 2.75rem;
		border-radius: 9999rem;
		display: flex;
		align-items: center;
		justify-content: center;
		background-color: var(--back);
		box-shadow: var(--shadow-xs);
	}

	.album-icon {
		width: 1.75rem;
		height: 1.75rem;
		stroke-width: 2;
	}

	.album-main {
		flex: 1 1 auto;
		min-width: 0;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.album-name {
		font-size: 1rem;
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
		flex: 0 0 auto;
		margin-left: 0.5rem;
		padding: 0.25rem;
		border: none;
		border-radius: 9999rem;
		background-color: transparent;
		display: inline-flex;
		align-items: center;
		justify-content: center;
		gap: 0.1875rem;
		cursor: pointer;
	}

	.album-menu-button:hover {
		background-color: var(--secondary);
	}

	.menu-dot {
		width: 0.125rem;
		height: 0.125rem;
		border-radius: 9999rem;
		background-color: var(--muted-foreground);
	}
</style>
