<script lang="ts">
	const {
		text,
		onclick,
		className = ''
	}: { text: string; onclick: () => void; className?: string } = $props<{
		text: string;
		onclick: () => void;
		className?: string;
	}>();
</script>

<button class="btn {className}" {onclick}>{text}</button>

<style>
	.btn {
		min-width: 9.5rem;
		height: 2rem;
		margin: 0.5rem 0 0 auto;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.15px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
		justify-self: right;
	}

	.btn:hover {
		background-color: var(--primary-hov);
	}
</style>
