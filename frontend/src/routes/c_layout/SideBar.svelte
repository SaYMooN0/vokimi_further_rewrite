<script lang="ts">
	import SideBarBottom from './c_side_bar/SideBarBottom.svelte';
	import AlbumsLink from './c_side_bar/c_links/AlbumsLink.svelte';
	import AuthorsLink from './c_side_bar/c_links/AuthorsLink.svelte';
	import CatalogLink from './c_side_bar/c_links/CatalogLink.svelte';
	import MyVokisLink from './c_side_bar/c_links/MyVokisLink.svelte';
	import NotificationsLink from './c_side_bar/c_links/NotificationsLink.svelte';
	import ProfileLink from './c_side_bar/c_links/ProfileLink.svelte';
	let isCollapsed = $state(false);
</script>

<div class="sidebar">
	<div class="links">
		<ProfileLink {isCollapsed} />
		<CatalogLink {isCollapsed} />
		<AuthorsLink {isCollapsed} />
		<MyVokisLink {isCollapsed} />
		<AlbumsLink {isCollapsed} />
		<NotificationsLink {isCollapsed} />
	</div>
	<SideBarBottom />
</div>

<style>
	.sidebar {
		--sidebar-width: 13rem;
		max-width: var(--sidebar-width);
		position: sticky;
		top: 0;
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		height: 100vh;
		padding: var(--sidebar-links-top-padding) 0 2rem 0;
		padding-left: 0.25rem;
	}

	.links {
		display: flex;
		flex-direction: column;
		justify-content: center;
		gap: 0.5rem;
		width: var(--sidebar-width);
		background-color: var(--back);
	}
</style>
