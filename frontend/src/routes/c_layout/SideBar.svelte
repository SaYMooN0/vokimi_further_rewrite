<script lang="ts">
	import SideBarBottom from './c_side_bar/SideBarBottom.svelte';
	import AlbumsLink from './c_side_bar/c_links/AlbumsLink.svelte';
	import CatalogLink from './c_side_bar/c_links/CatalogLink.svelte';
	import CreatorsLink from './c_side_bar/c_links/CreatorsLink.svelte';
	import MyVokisLink from './c_side_bar/c_links/MyVokisLink.svelte';
	import NotificationsLink from './c_side_bar/c_links/NotificationsLink.svelte';
	let isCollapsed = $state(false);
</script>

<div class="sidebar">
	<div class="links">
		<CatalogLink {isCollapsed} />
		<CreatorsLink {isCollapsed} />
		<MyVokisLink {isCollapsed} />
		<AlbumsLink {isCollapsed} />
		<NotificationsLink {isCollapsed} />
	</div>
	<SideBarBottom />
</div>

<style>
	.sidebar {
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		height: 100vh;
		padding: 4rem 0 2rem;
	}

	.links {
		--sidebar-width: 12rem;

		display: flex;
		flex-direction: column;
		justify-content: center;
		gap: 0.5rem;
		width: var(--sidebar-width);
		background-color: var(--back);
	}
</style>
