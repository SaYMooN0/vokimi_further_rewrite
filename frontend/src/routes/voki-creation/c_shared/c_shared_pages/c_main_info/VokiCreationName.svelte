<script lang="ts">
	import { TextareaAutosize } from 'runed';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import MainInfoSectionButton from './c_sections_shared/MainInfoSectionButton.svelte';

	let { vokiName, vokiId }: { vokiName: string; vokiId: string } = $props<{
		vokiName: string;
		vokiId: string;
	}>();

	let textarea = $state<HTMLTextAreaElement>(null!);
	let newName = $state(vokiName);
	new TextareaAutosize({ element: () => textarea, input: () => newName });

	let isEditing = $state(false);

	async function saveChanges() {}
</script>

<div class="voki-name-section">
	{#if isEditing}
		<VokiCreationFieldName fieldName="Voki name:" className="edit-field-name" />

		<textarea
			class="name-input"
			bind:this={textarea}
			bind:value={newName}
			name={StringUtils.rndStr()}
		/>
		<div class="btns-container">
			<button onclick={() => (isEditing = false)} class="cancel-btn">Cancel</button>
			<button onclick={() => saveChanges()} class="save-btn">Save</button>
		</div>
	{:else}
		<p class="voki-name-p">
			<VokiCreationFieldName fieldName="Voki name:" />
			<label class="voki-name-value">{vokiName}</label>
		</p>
		<MainInfoSectionButton text="Edit name" onclick={() => (isEditing = true)} />
	{/if}
</div>

<style>
	.voki-name-section {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
	}

	.voki-name-section > :global(.edit-field-name) {
		width: 100%;
		margin-bottom: 0.375rem;
	}
	.name-input {
		background-color: var(--secondary);
		outline: 0.125rem solid var(--secondary);
		resize: none;
		border: none;
		padding: 0.25rem 0.375rem;
		border-radius: 0.375rem;
		font-size: 1.25rem;
		width: 100%;
		box-sizing: border-box;
		font-weight: 500;
	}
	.name-input:hover {
		outline-color: var(--secondary-foreground);
	}
	.name-input:focus {
		outline-color: var(--primary);
	}
	.btns-container {
		width: 100%;
		display: flex;
		flex-direction: row;
		grid-template-columns: 1fr 1fr;
		justify-content: right;
		gap: 0.5rem;
		margin-top: 0.5rem;
	}

	.btns-container button {
		width: 7rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.2px;
		cursor: pointer;
		transition: transform 0.12s ease-in;
		height: 2rem;
	}
	.cancel-btn {
		font-weight: 480;
		background-color: var(--muted);
		color: var(--muted-foreground);
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
	.voki-name-p {
		width: 100%;
	}
	.voki-name-value {
		text-decoration: none;
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
	}
</style>
