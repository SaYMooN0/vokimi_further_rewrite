<script lang="ts">
	import { onMount, onDestroy } from 'svelte';
	import { onClickOutside } from 'runed';

	let contextMenuState = $state({
		isOpen: false,
		x: 0,
		y: 0
	});

	let container: HTMLElement = $state<HTMLElement>()!;
	let scrollPos = { x: 0, y: 0 };

	function preventScroll(e: Event) {
		e.preventDefault();
	}

	function lockScroll() {
		scrollPos = { x: window.scrollX, y: window.scrollY };
		window.addEventListener('wheel', preventScroll, { passive: false });
		window.addEventListener('touchmove', preventScroll, { passive: false });
	}

	function unlockScroll() {
		window.removeEventListener('wheel', preventScroll);
		window.removeEventListener('touchmove', preventScroll);
	}

	function closeMenu() {
		contextMenuState = { isOpen: false, x: 0, y: 0 };
		unlockScroll();
	}

	onMount(() => {
		const observer = new MutationObserver(() => {
			if (window.scrollX !== scrollPos.x || window.scrollY !== scrollPos.y) {
				closeMenu();
			}
		});
		observer.observe(document.body, { attributes: true, subtree: true });
		window.addEventListener('scroll', closeMenu, { passive: true });
		onDestroy(() => {
			observer.disconnect();
			window.removeEventListener('scroll', closeMenu);
		});
	});

	onClickOutside(
		() => container,
		() => closeMenu()
	);

	export function open(x: number, y: number, vokiId: string) {
		lockScroll();
		contextMenuState = { isOpen: true, x, y };
	}
</script>

{#if contextMenuState.isOpen}
	<div
		bind:this={container}
		class="context-menu"
		style="top:{contextMenuState.y}px; left:{contextMenuState.x}px;"
	>
		<div>Content</div>
	</div>
{/if}

<style>
	.context-menu {
		position: absolute;
		padding: 0.5rem;
		border-radius: var(--radius);
		background: var(--secondary);
		box-shadow: var(--shadow-md);
	}
</style>
