<script lang="ts">
	import SideBarBottom from './_c_side_bar/SideBarBottom.svelte';
	import AlbumsLink from './_c_side_bar/_c_links/AlbumsLink.svelte';
	import AuthorsLink from './_c_side_bar/_c_links/AuthorsLink.svelte';
	import CatalogLink from './_c_side_bar/_c_links/CatalogLink.svelte';
	import MyVokisLink from './_c_side_bar/_c_links/MyVokisLink.svelte';
	import NotificationsLink from './_c_side_bar/_c_links/NotificationsLink.svelte';
	import ProfileLink from './_c_side_bar/_c_links/ProfileLink.svelte';
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
		position: sticky;
		top: 0;
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		max-width: var(--sidebar-width);
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
