<script lang="ts">
	const { onclick, text }: { onclick: () => void; text: string } = $props<{
		onclick: () => void;
		text: string;
	}>();
</script>

<button class="confirm-btn unselectable" onclick={() => onclick()} type="submit">{text}</button>

<style>
	.confirm-btn {
		width: 92%;
		height: 2.25rem;
		margin: 1rem auto 0 auto;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.5rem;
		font-weight: 450;
		background-color: var(--primary);
		color: var(--primary-foreground);
		transition: all 0.12s ease-in;
		cursor: pointer;
		box-shadow: var(--shadow);
		letter-spacing: 0.5px;
	}

	.confirm-btn:hover {
		background-color: var(--primary-hov);
	}

	.confirm-btn:active {
		width: 90%;
	}
</style>
