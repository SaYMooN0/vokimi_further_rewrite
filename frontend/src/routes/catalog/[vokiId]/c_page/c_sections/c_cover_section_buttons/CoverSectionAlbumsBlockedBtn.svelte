<script lang="ts">
	let isHovered = $state(false);
	function animateIcon() {
		isHovered = true;
		setTimeout(() => {
			isHovered = false;
		}, 600);
	}
</script>

<button class="album-blocked-btn" onmouseenter={() => animateIcon()} onfocus={() => animateIcon()}>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M12 16.5V14.5"
			stroke="currentColor"
			stroke-linecap="round"
			data-custom="0"
			class:hov={isHovered}
		/>
		<path
			d="M4.2678 18.8447C4.49268 20.515 5.87612 21.8235 7.55965 21.9009C8.97626 21.966 10.4153 22 12 22C13.5847 22 15.0237 21.966 16.4403 21.9009C18.1239 21.8235 19.5073 20.515 19.7322 18.8447C19.8789 17.7547 20 16.6376 20 15.5C20 14.3624 19.8789 13.2453 19.7322 12.1553C19.5073 10.485 18.1239 9.17649 16.4403 9.09909C15.0237 9.03397 13.5847 9 12 9C10.4153 9 8.97626 9.03397 7.55965 9.09909C5.87612 9.17649 4.49268 10.485 4.2678 12.1553C4.12104 13.2453 3.99999 14.3624 3.99999 15.5C3.99999 16.6376 4.12104 17.7547 4.2678 18.8447Z"
			stroke="currentColor"
			data-custom="1"
			class:hov={isHovered}
		/>
		<path
			d="M7.5 9V6.5C7.5 4.01472 9.51472 2 12 2C14.4853 2 16.5 4.01472 16.5 6.5V9"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="2"
			class:hov={isHovered}
		/>
	</svg>

	<label class="btn-text">Add to album</label>
</button>

<style>
	.album-blocked-btn {
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
