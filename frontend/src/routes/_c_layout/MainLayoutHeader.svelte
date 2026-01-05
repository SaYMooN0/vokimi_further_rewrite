<script lang="ts">
	import logo from '$lib/logos/main-vokimi-logo.svg';
	import LayoutHeaderRightPart from './_c_header/LayoutHeaderRightPart.svelte';
	import MainLayoutHeaderSearch from './_c_header/MainLayoutHeaderSearch.svelte';
</script>

<div class="main-layout-header">
	<div class="side left unselectable">
		<img src={logo} alt="logo" />
	</div>

	<div class="middle-part">
		<MainLayoutHeaderSearch />
	</div>
	<div class="side right">
		<LayoutHeaderRightPart />
	</div>
</div>

<style>
	.main-layout-header {
		display: grid;
		left: 0;
		right: 0;
		top: 0;
		position: fixed;
		grid-template-columns: var(--side-panel-width) 1fr var(--side-panel-width);
		height: var(--layout-header-height);
		max-height: var(--layout-header-height);
		padding: var(--layout-header-vertical-padding) 0;
		outline: 1px solid blueviolet;
	}
	.side.left {
		padding: 0 var(--sides-padding);
		display: flex;
		align-items: center;
		justify-content: center;
	}
	.side.left > img {
		height: 2.25rem;
		width: fit-content;
	}
	.middle-part {
		width: 100%;
		height: 100%;
	}
	.side.right {
		max-height: var(--layout-header-height);
		padding: 0 var(--sides-padding);
	}
</style>
