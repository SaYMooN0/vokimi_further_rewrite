<script lang="ts">
	let { onCancel, onSave }: { onCancel: () => void; onSave: () => void } = $props<{
		onCancel: () => void;
		onSave: () => void;
	}>();
</script>

<div class="btns-container">
	<button onclick={onCancel} class="cancel-btn">Cancel</button>
	<button onclick={onSave} class="save-btn">Save</button>
</div>

<style>
	.btns-container {
		display: flex;
		flex-direction: row;
		justify-content: right;
		gap: 0.5rem;
		width: 100%;
		margin-top: 0.5rem;
		grid-template-columns: 1fr 1fr;
	}

	.btns-container button {
		width: 7rem;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.2px;
		transition: transform 0.12s ease-in;
		cursor: pointer;
	}

	.cancel-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 480;
	}

	.cancel-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.save-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.save-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
