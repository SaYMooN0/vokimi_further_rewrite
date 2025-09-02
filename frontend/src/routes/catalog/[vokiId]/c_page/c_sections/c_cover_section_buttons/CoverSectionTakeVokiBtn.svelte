<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki';

	let { vokiId, vokiType }: { vokiId: string; vokiType: VokiType } = $props<{
		vokiId: string;
		vokiType: VokiType;
	}>();
	let isHovered = $state(false);
	function animateIcon() {
		isHovered = true;
		setTimeout(() => {
			isHovered = false;
		}, 600);
	}
</script>

<a
	href="/take-voki/{vokiId}/{StringUtils.pascalToKebab(vokiType)}"
	class="take-voki-btn"
	onmouseenter={() => animateIcon()}
	onfocus={() => animateIcon()}
>
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M9.00005 6C9.00005 6 15 10.4189 15 12C15 13.5812 9 18 9 18"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			data-custom="0"
			class:hov={isHovered}
		/>
	</svg>

	<label class="btn-text">Take voki</label>
</a>

<style>
	.take-voki-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.take-voki-btn:hover {
		background-color: var(--primary-hov);
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
