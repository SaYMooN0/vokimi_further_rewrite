<script lang="ts">
	import { type Snippet } from 'svelte';
	import SideBar from './c_layout/SideBar.svelte';
	import AppToaster from './c_layout/AppToaster.svelte';
	import LayoutSprites from './c_layout/LayoutSprites.svelte';
	import LayoutDialogs from './c_layout/LayoutDialogs.svelte';

	let isFullWidthMode = $state(false);
	const { children }: { children: Snippet } = $props<{ children: Snippet }>();
</script>

<LayoutDialogs />
<LayoutSprites />
<AppToaster />

<div class="page" class:full-width={isFullWidthMode}>
	<div class="width-limit">
		<SideBar />
		<div id="page-content">
			{@render children()}
		</div>
	</div>
</div>

<style>
	.page {
		display: flex;
		flex-direction: column;
		width: 100%;
		box-sizing: border-box;

		--side-bar-links-top-padding: 4rem;
		--width-limit: 90vw;
	}

	.width-limit {
		display: grid;
		gap: 1rem;
		width: var(--width-limit);
		height: 100%;
		margin: 0 auto;
		grid-template-columns: auto 1fr;
	}

	@media (1920px <= width) {
		.page {
			--width-limit: 112rem;
		}
	}

	@media (1536px <= width <= 1919px) {
		.page {
			--width-limit: 94vw;
		}
	}

	@media (1366px <= width <= 1535px) {
		.page {
			--width-limit: 80rem;
		}
	}

	@media (width <= 1365px) {
		.page {
			--width-limit: 90vw;
		}
	}

	.page.full-width {
		--width-limit: 100% !important;
	}
</style>
