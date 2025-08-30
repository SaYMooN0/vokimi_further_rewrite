<script lang="ts">
	let { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let isHovered = $state(false);
	function animateIcon() {
		isHovered = true;
		setTimeout(() => {
			isHovered = false;
		}, 600);
	}
</script>

<button class="add-to-album-btn" onmouseenter={() => animateIcon()} onfocus={() => animateIcon()}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M12 4V20M20 12H4"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="0"
			class:hov={isHovered}
		></path>
	</svg>
	<label class="btn-text">Add to album</label>
</button>

<style>
	.add-to-album-btn {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}

	.add-to-album-btn:hover {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	path {
		transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
		transform-origin: center;
		transform: none;
	}

	path[data-custom='0'].hov {
		animation: a0 0.6s forwards;
	}

	@keyframes a0 {
		0% {
			opacity: 0;
			transform: translateY(6px) scale(0.5);
		}

		80% {
			opacity: 1;
			transform: translateY(0) scale(1);
		}
	}
</style>
