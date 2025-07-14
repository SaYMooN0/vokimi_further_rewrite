<script lang="ts">
	const { text, onclick }: { text: string; onclick: () => void } = $props<{
		text: string;
		onclick: () => void;
	}>();
</script>

<button class="btn" {onclick}>{text}</button>

<style>
	.btn {
		justify-self: right;
		margin: 0 1rem 0 auto;
		background-color: var(--primary);
		color: var(--primary-foreground);
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		font-weight: 450;
		letter-spacing: 0.15px;
		cursor: pointer;
		transition: transform 0.12s ease-in;
		width: 9.5rem;
		height: 2rem;
	}
	.btn:hover {
		background-color: var(--primary-hov);
	}
</style>
