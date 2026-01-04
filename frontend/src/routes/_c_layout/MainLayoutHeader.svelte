<script lang="ts">
	import logo from '$lib/logos/main-vokimi-logo.svg';
</script>

<div class="main-layout-header">
	<div class="left-part">
		<img src={logo} alt="logo" />
	</div>

	<div class="center-part">Center</div>
	<div class="right-part">Right</div>
</div>

<style>
	.main-layout-header {
		display: grid;
		width: 100vw;
        left: 0;
        top: 0;
		position: fixed;
		grid-template-columns: var(--side-panel-width) 1fr var(--side-panel-width);
		height: var(--layout-header-height);
	}
</style>
