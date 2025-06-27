<script lang="ts">
	let {
		onclick,
		text,
		class: userClass = '',
		...props
	}: {
		onclick: () => void;
		text: string;
		class?: string;
		[key: string]: any;
	} = $props<{
		onclick: () => void;
		text: string;
		class?: string;
		[key: string]: any;
	}>();
</script>

<button class={`primary-btn ${userClass}`} {onclick} {...props}>
	{text}
</button>

<style>
	.primary-btn {
		width: fit-content;
		box-sizing: border-box;
		padding: 0.5rem 1rem;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 400;
		letter-spacing: 0.25px;
		transition: background-color 0.08s ease-in-out;
		cursor: pointer;

	}

	.primary-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
