<script lang="ts">
	import { onClickOutside } from 'runed';
	import { onMount, onDestroy, type Snippet } from 'svelte';

	let containerRef = $state<HTMLElement>()!;
	let menuState = $state({
		isOpen: false,
		x: 0,
		y: 0
	});
	let scrollPos = $state({ x: 0, y: 0 });
	interface Props {
		children: Snippet;
		class?: string;
		onAfterClose?: () => void;
	}
	let { children, class: className = '', onAfterClose }: Props = $props();
	function preventScroll(e: Event) {
		e.preventDefault();
	}

	export function open(x: number, y: number) {
		menuState = { isOpen: true, x, y };
		lockScroll();
	}
	function lockScroll() {
		scrollPos = { x: window.scrollX, y: window.scrollY };
		window.addEventListener('wheel', preventScroll, { passive: false });
		window.addEventListener('touchmove', preventScroll, { passive: false });
	}

	export function close() {
		menuState.isOpen = false;
		unlockScroll();
		if (onAfterClose) {
			onAfterClose();
		}
	}
	function unlockScroll() {
		window.removeEventListener('wheel', preventScroll);
		window.removeEventListener('touchmove', preventScroll);
	}

	onMount(() => {
		const observer = new MutationObserver(() => {
			if (window.scrollX !== scrollPos.x || window.scrollY !== scrollPos.y) {
				close();
			}
		});
		observer.observe(document.body, { attributes: true, subtree: true });

		window.addEventListener('scroll', close, { passive: true });

		onDestroy(() => {
			observer.disconnect();
			window.removeEventListener('scroll', close);
		});
	});

	onClickOutside(() => containerRef, close);
</script>

{#if menuState.isOpen}
	<div
		bind:this={containerRef}
		class="context-menu {className}"
		style="top:{menuState.y}px; left:{menuState.x}px;"
	>
		{@render children()}
	</div>
{/if}

<style>
	.context-menu {
		position: absolute;
		padding: 0.5rem;
		border-radius: var(--radius);
		background: var(--secondary);
		box-shadow: var(--shadow-md);
		animation: fade-in 80ms ease-out;
		z-index: 9999;
	}

	@keyframes fade-in {
		from {
			opacity: 0;
			transform: scale(0.96);
		}
		to {
			opacity: 1;
			transform: scale(1);
		}
	}
</style>
