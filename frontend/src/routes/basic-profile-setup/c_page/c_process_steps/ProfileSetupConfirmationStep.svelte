<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import TagItemChip from '$lib/components/TagItemChip.svelte';
	import { ApiUserProfiles, RJO } from '$lib/ts/backend-communication/backend-services';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import { LanguageUtils, type Language } from '$lib/ts/language';
	import ConfirmationStepItemsListWrapper from './c_confirmation_step/ConfirmationStepItemsListWrapper.svelte';
	import ConfirmationStepSmallEditButton from './c_confirmation_step/ConfirmationStepSmallEditButton.svelte';

	interface Props {
		uniqueName: string;
		chosenTags: Set<string>;
		goToTagsStep: () => void;
		profilePic: string;
		goToPicStep: () => void;
		displayName: string;
		goToNameStep: () => void;
		languages: Set<Language>;
		goToLanguagesStep: () => void;
		changeStateToSaved: () => void;
	}
	const {
		uniqueName,
		chosenTags,
		goToTagsStep,
		profilePic,
		goToPicStep,
		displayName,
		goToNameStep,
		languages,
		goToLanguagesStep,
		changeStateToSaved
	}: Props = $props();
	let savingErrs: Err[] = $state([]);
	let isLoading = $state(false);
	async function saveSetup() {
		if (isLoading) {
			return;
		}
		isLoading = true;
		const response = await ApiUserProfiles.fetchVoidResponse(
			'/save-basic-setup',
			RJO.POST({
				tags: Array.from(chosenTags),
				profilePic: profilePic,
				UserDisplayName: displayName,
				PreferredLanguages: Array.from(languages)
			})
		);
		if (response.isSuccess) {
			changeStateToSaved();
		} else {
			savingErrs = response.errs;
		}
		isLoading = false;
	}
</script>

<div class="confirmation-step-container">
	<div class="user-name-pic-container" aria-label="Profile picture">
		<div class="profile-pic-container">
			<img src={StorageBucketMain.fileSrc(profilePic)} alt="Profile picture preview" />
			<ConfirmationStepSmallEditButton onclick={goToPicStep} className="edit-profile-pic-btn" />
		</div>

		<div class="names-info">
			<h3 class="display-name">
				{displayName}
				<ConfirmationStepSmallEditButton onclick={goToNameStep} className="edit-name-pic-btn" />
			</h3>
			<label class="unique-name">@{uniqueName}</label>
		</div>
	</div>
	<ConfirmationStepItemsListWrapper listName="Languages" onEditClick={goToLanguagesStep}>
		{#if languages.size > 0}
			<div class="languages-list">
				{#each languages as lang}
					<div class="language">
						<svg><use href={LanguageUtils.icon(lang)} /></svg>
						{LanguageUtils.name(lang)}
					</div>
				{/each}
			</div>
		{:else}
			<FieldNotSetLabel text="No languages chosen as preferred" />
		{/if}
	</ConfirmationStepItemsListWrapper>
	<ConfirmationStepItemsListWrapper listName="Tags" onEditClick={goToTagsStep}>
		{#if chosenTags.size > 0}
			<div class="tags-list">
				{#each chosenTags as tag}
					<TagItemChip {tag} isLink={false} className="tag-item" />
				{/each}
			</div>
		{:else}
			<FieldNotSetLabel text="No languages chosen as preferred" />
		{/if}
	</ConfirmationStepItemsListWrapper>

	<DefaultErrBlock errList={savingErrs} />
	<button class="save-btn unselectable" class:loading={isLoading} onclick={() => saveSetup()}
		>{#if isLoading}
			Loading <LinesLoader sizeRem={1.3} strokePx={2} />
		{:else}
			Save
		{/if}
	</button>
</div>

<style>
	.confirmation-step-container {
		margin: 1rem auto 0;
		padding: 2rem;
		box-shadow: var(--shadow-xs), var(--shadow-md);

		border-radius: 1rem;
		display: grid;
		gap: 1.75rem;
		min-width: 40rem;
		max-width: min(58rem, 100%);
	}

	.user-name-pic-container {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.5rem;
	}

	.profile-pic-container {
		width: 8rem;
		height: 8rem;
		position: relative;
	}

	.profile-pic-container img {
		width: 100%;
		height: 100%;
		object-fit: cover;
		border-radius: 50%;
		background-color: var(--muted);
		border: 0.125rem solid var(--muted);
	}
	.profile-pic-container > :global(.edit-profile-pic-btn) {
		position: absolute;
		right: 0.25rem;
		bottom: 0.25rem;
		height: 1.75rem !important;
		width: 1.75rem !important;
		border-radius: 0.5rem !important;

		transition: opacity 0.2s ease-in;
	}

	.profile-pic-container:has(img:hover) > :global(.edit-profile-pic-btn) {
		opacity: 0.2;
	}

	.names-info {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.display-name {
		font-size: 1.5rem;
		font-weight: 600;
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		flex-wrap: nowrap;
	}
	.unique-name {
		font-size: 1rem;
		font-weight: 400;
		color: var(--muted-foreground);
	}
	.languages-list {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		width: 100%;
		gap: 0.5rem;
	}
	.language {
		width: auto;
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 0.375rem;
		align-items: center;
		justify-content: center;
		padding: 0.25rem 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		border-radius: 0.375rem;
		box-shadow: var(--shadow-xs);
		font-weight: 500;
		font-size: 1.125rem;
	}
	.language > svg {
		height: 1.25rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.25rem;
		stroke-width: 1.9;
	}
	.tags-list {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		width: 100%;
		column-gap: 0.75rem;
		row-gap: 0.5rem;
	}

	.save-btn {
		width: min(100%, 28rem);
		margin: 0 auto;
		margin-top: 1rem;
		padding: 0.5rem 1.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		border: none;
		border-radius: 0.5rem;
		font-weight: 500;
		letter-spacing: 1px;
		cursor: pointer;
		box-shadow: var(--shadow-md);
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 0.5rem;
	}

	.save-btn:not(.loading):hover {
		background-color: var(--primary-hov);
	}
	.save-btn.loading {
		opacity: 0.85;
		font-weight: 450;
		cursor: not-allowed !important;
	}

	.save-btn > :global(.container) {
		--loader-color: var(--primary-foreground);
	}
</style>
