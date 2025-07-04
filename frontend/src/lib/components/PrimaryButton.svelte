<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		onclick: () => void;
		children: Snippet;
		class?: string;
		[key: string]: any;
	}
	let { onclick, children, class: userClass = '', ...props }: Props = $props();
</script>

<button class={`primary-btn ${userClass}`} {onclick} {...props}>
	{@render children()}
</button>

<style>
	.primary-btn {
		width: fit-content;
		box-sizing: border-box;
		padding: 0.25rem 1.25rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.5rem;
		font-weight: 420;
		letter-spacing: 0.25px;
		transition: background-color 0.08s ease-in-out;
		cursor: pointer;
		transition: all 0.12s ease;
		box-shadow: var(--shadow-md);

	}

	.primary-btn:hover {
		background-color: var(--primary-hov);
	}

	.primary-btn:active {
		transform: scale(0.96);
	}
</style>
