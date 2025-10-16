<script lang="ts">
	import type { Language } from '$lib/ts/language';
	import { ProfileSetupProcessState } from './profile-setup-process-state.svelte';
	import type { ResponseVoidResult } from '$lib/ts/backend-communication/result-types';
	import ProfileSetupLanguagesStep from './c_process_steps/ProfileSetupLanguagesStep.svelte';
	import ProfileSetupTagsStep from './c_process_steps/ProfileSetupTagsStep.svelte';
	import ProfileSetupDisplayNameStep from './c_process_steps/ProfileSetupDisplayNameStep.svelte';
	import ProfileSetupProfilePictureStep from './c_process_steps/ProfileSetupProfilePictureStep.svelte';
	import SetupProcessStepHeader from './c_setup_process/SetupProcessStepHeader.svelte';
	import SetupProcessFlowButtons from './c_setup_process/SetupProcessFlowButtons.svelte';
	import ProfileSetupConfirmationStep from './c_process_steps/ProfileSetupConfirmationStep.svelte';
	interface Props {
		initialLangs: Language[];
		initialTags: string[];
		initialProfilePic: string;
		initialDisplayName: string;
		saveSetup: () => Promise<ResponseVoidResult>;
	}
	let { initialLangs, initialTags, initialProfilePic, initialDisplayName, saveSetup }: Props =
		$props();
	let setupProcessState = new ProfileSetupProcessState(
		initialLangs,
		initialTags,
		initialProfilePic,
		initialDisplayName
	);
	type Step = 'languages' | 'tags' | 'display-name' | 'profile-pic' | 'confirmation';
	let currentStep = $state<Step>('languages');
	type ProcessButton = {
		appearance: () => 'primary' | 'secondary' | 'hidden';
		onClick: () => void;
	};
	let nextButton: ProcessButton = $derived({
		appearance: () => {
			if (currentStep === 'confirmation') {
				return 'hidden';
			}
			if (currentStep === 'languages') {
				if (setupProcessState.anyLanguagesChosen()) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'tags') {
				if (setupProcessState.chosenFavoriteTags.size > 0) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'display-name') {
				if (setupProcessState.displayNameInputValue.length > 0) {
					return 'primary';
				} else {
					return 'secondary';
				}
			}
			if (currentStep === 'profile-pic') {
				return 'primary';
			}
			return 'secondary';
		},
		onClick: () => {
			if (currentStep === 'languages') {
				currentStep = 'tags';
			} else if (currentStep === 'tags') {
				currentStep = 'display-name';
			} else if (currentStep === 'display-name') {
				currentStep = 'profile-pic';
			} else if (currentStep === 'profile-pic') {
				currentStep = 'confirmation';
			}
		}
	});
	let prevButton: ProcessButton = $derived({
		appearance: () => {
			if (currentStep === 'languages') {
				return 'hidden';
			}
			return 'secondary';
		},
		onClick: () => {
			if (currentStep === 'confirmation') {
				currentStep = 'profile-pic';
			} else if (currentStep === 'profile-pic') {
				currentStep = 'display-name';
			} else if (currentStep === 'display-name') {
				currentStep = 'tags';
			} else if (currentStep === 'tags') {
				currentStep = 'languages';
			}
		}
	});
</script>

<div class="setup-process-container" aria-label="Profile setup">
	<div class="step-content">
		{#if currentStep === 'languages'}
			<SetupProcessStepHeader text="Choose languages you are comfortable with" />
			<ProfileSetupLanguagesStep
				isLanguageChosen={(l) => setupProcessState.isLanguageChosen(l)}
				toggleLanguage={(l) => setupProcessState.toggleLanguage(l)}
			/>
		{:else if currentStep === 'tags'}
			<SetupProcessStepHeader text="Choose tags you are interested in" />
			<ProfileSetupTagsStep
				chosenTags={setupProcessState.chosenFavoriteTags}
				suggestions={setupProcessState.suggestedTags()}
				chooseTag={(tag) => setupProcessState.chooseTag(tag)}
				removeTag={(tag) => setupProcessState.removeChosenTag(tag)}
			/>
		{:else if currentStep === 'display-name'}
			<SetupProcessStepHeader text="Input name that you want to be known by" />
			<ProfileSetupDisplayNameStep bind:displayName={setupProcessState.displayNameInputValue} />
		{:else if currentStep === 'profile-pic'}
			<SetupProcessStepHeader text="Choose your profile picture" />
			<ProfileSetupProfilePictureStep bind:profilePic={setupProcessState.profilePicInputValue} />
		{:else if currentStep === 'confirmation'}
			<SetupProcessStepHeader text="Save chosen settings" />
			<ProfileSetupConfirmationStep />
		{:else}
			<h1>Something went wrong, reload the page</h1>
		{/if}
	</div>

	<div class="footer">
		<SetupProcessFlowButtons {nextButton} {prevButton} />
	</div>
</div>

<style>
	.setup-process-container {
		width: 100%;
		max-width: 66rem;
		display: grid;
		gap: 1rem;
	}

	.step-content {
		min-height: 20rem;
		display: grid;
		align-items: start;
		animation: fade-in 220ms ease-out both;
	}

	.footer {
		margin-top: 0.25rem;
		padding-top: 0.75rem;
	}
</style>
