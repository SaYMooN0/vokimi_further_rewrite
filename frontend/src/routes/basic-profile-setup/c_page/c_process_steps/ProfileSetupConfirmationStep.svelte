<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiUserProfiles, RJO } from '$lib/ts/backend-communication/backend-services';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';

	interface Props {
		chosenTags: Iterable<string>;
		goToTagsStep: () => void;
		profilePic: string;
		goToPicStep: () => void;
		displayName: string;
		goToNameStep: () => void;
		languages: Iterable<string>;
		goToLanguagesStep: () => void;
		changeStateToSaved: () => void;
	}
	let {
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
		isLoading = true;
		const response = await ApiUserProfiles.fetchVoidResponse(
			'/save-basic-setup',
			RJO.POST({
				tags: chosenTags,
				profilePic: profilePic,
				UserDisplayName: displayName,
				PreferredLanguages: languages
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

<div class="confirmation-container">
	<h2>Confirm selected settings</h2>

	<section class="section">
		<div>
			<h3>Display name</h3>
			<button class="edit-btn" onclick={goToNameStep}>Edit</button>
		</div>
		<p class="value">{displayName}</p>
	</section>

	<section class="section">
		<div>
			<h3>Profile picture</h3>
			<button class="edit-btn" onclick={goToPicStep}>Edit</button>
		</div>
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(profilePic)}
			alt="Profile pic preview"
		/>
	</section>

	<section class="section">
		<div>
			<h3>Languages</h3>
			<button class="edit-btn" onclick={goToLanguagesStep}>Edit</button>
		</div>
		<div class="tag-list">
			{#each languages as lang}
				<span class="tag">{lang}</span>
			{/each}
		</div>
	</section>

	<section class="section">
		<div>
			<h3>Favorite tags</h3>
			<button class="edit-btn" onclick={goToTagsStep}>Edit</button>
		</div>
		<div class="tag-list">
			{#each chosenTags as tag}
				<span class="tag">#{tag}</span>
			{/each}
		</div>
	</section>
	<DefaultErrBlock errList={savingErrs} />
	<button class="save-btn" onclick={() => saveSetup()}>Save</button>
</div>

<style>
	.confirmation-container {
		display: flex;
		flex-direction: column;
		gap: 2rem;
		max-width: 32rem;
		margin: 0 auto;
		padding: 2rem;
		background: var(--back);
		color: var(--text);
	}

	h2 {
		text-align: center;
		color: var(--primary);
		font-size: 1.5rem;
	}

	.section {
		padding: 1.5rem;
		border-radius: var(--radius);
		background: var(--secondary);
		box-shadow: var(--shadow);
	}

	.edit-btn {
		background: none;
		border: none;
		color: var(--primary);
		cursor: pointer;
		font-weight: 500;
		transition: color 0.15s ease-in-out;
	}

	.edit-btn:hover {
		color: var(--primary-hov);
		text-decoration: underline;
	}

	.profile-pic {
		width: 6rem;
		height: 6rem;
		object-fit: cover;
		border-radius: 50%;
		border: 0.2rem solid var(--primary);
	}

	.save-btn {
		margin-top: 1rem;
		padding: 0.75rem 1.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.1rem;
		border: none;
		border-radius: var(--radius);
		cursor: pointer;
		box-shadow: var(--shadow-md);
		transition: background-color 0.15s ease-in-out;
	}

	.save-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
