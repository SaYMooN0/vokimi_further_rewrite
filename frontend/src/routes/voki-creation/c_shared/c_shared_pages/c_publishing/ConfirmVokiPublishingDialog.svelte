<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	export function open() {}
	let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton bind:this={dialog}>
	<div class="msg-container">
		<label class="warning-label">Warning</label>
		<p class="warning-text">
			Publishing your Voki will make it public — anyone can view it and take it. After publishing,
			most parts can’t be changed, so make sure it’s ready. Click the button below to run the check.
			We’ll scan your Voki for issues and show you if anything needs fixing.
		</p>
		<PrimaryButton onclick={() => loadPublishingIssues()} class="check-for-issues-btn"
			>Check for issues</PrimaryButton
		>
	</div>
</DialogWithCloseButton>
