<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import CreateNewAlbumColorInput from '../c_create_album_dialog/CreateNewAlbumColorInput.svelte';
	import CreateNewAlbumIconInput from '../c_create_album_dialog/CreateNewAlbumIconInput.svelte';

	let name = $state('');
	let mainColor = $state('#fabcbf');
	let secondaryColor = $state('#1abcff');
	let icon = $state('albums-bookmark-1-icon');
	let useTwoColors = $state(false);
	async function save() {}
</script>

<div class="new-album-creation">
	<label class="input-label">Album name</label>
	<input type="text" class="name-input" bind:value={name} />
	<div class="color-and-icons">
		<div class="icon-input">
			<label class="input-label">Album icon</label>
			<CreateNewAlbumIconInput
				bind:icon
				{mainColor}
				secondaryColor={useTwoColors ? secondaryColor : mainColor}
			/>
		</div>
		<div class="color-inputs-container">
			<div class="color-inputs-body">
				<div class="color-input">
					<label class="input-label">Main color</label>
					<CreateNewAlbumColorInput bind:color={mainColor} />
				</div>
				{#if useTwoColors}
					<div class="color-input">
						<label class="input-label">Second color</label>
						<CreateNewAlbumColorInput bind:color={secondaryColor} />
					</div>
				{/if}
			</div>
			<label class="use-two-colors-label"
				>Use two colors <DefaultCheckBox bind:checked={useTwoColors} />
			</label>
		</div>
	</div>
	<PrimaryButton onclick={() => save()}>Save</PrimaryButton>
</div>

<style>
	.new-album-creation {
		display: flex;
		flex-direction: column;
		width: 100%;
		height: 100%;
	}

	.input-label {
		width: 100%;
		margin-bottom: 0.125rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
		letter-spacing: 0.1px;
	}

	.name-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.5rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.5rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 500;
		outline: none;
	}

	.name-input:hover {
		border-color: var(--secondary-foreground);
	}

	.name-input:focus {
		border-color: var(--primary);
	}

	.color-and-icons {
		display: grid;
		grid-template-columns: 1fr 2fr;
		gap: 1rem;
		padding: 0 0.5rem;
		justify-items: center;
		margin-top: 0.75rem;
	}

	.color-inputs-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
	}

	.color-inputs-body {
		display: flex;
		flex-direction: row;
		width: 100%;
	}

	.color-input {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
	}

	.use-two-colors-label {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		color: var(--text);
		font-size: 1rem;
		font-weight: 500;
		letter-spacing: 0.1px;
	}
</style>
