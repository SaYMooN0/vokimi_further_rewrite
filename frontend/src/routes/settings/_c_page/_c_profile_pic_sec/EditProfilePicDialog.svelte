<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import UserProfilePicDisplay from '$lib/components/profile_pic/UserProfilePicDisplay.svelte';
	import type { UserProfilePicData } from '$lib/ts/users';
	import ProfilePicShapeSelector from './ProfilePicShapeSelector.svelte';

	let currentVal = $state<UserProfilePicData>();
	export function open() {}
</script>

<DialogWithCloseButton dialogId="edit-profile-pic-dialog">
	<div>
		{#if currentVal}
			<UserProfilePicDisplay
				key={currentVal.key}
				shape={currentVal.shape}
				sizeInRem={16}
				borderWidthInRem={0.375}
				borderColor="var(--back)"
			/>
			<ProfilePicShapeSelector
				currentShape={currentVal.shape}
				onSelect={(newShape) => {
					if (currentVal) {
						currentVal.shape = newShape;
					}
				}}
			/>
		{:else}
			<ErrView err={{ message: 'Something went wrong. Please close this dialog and try again' }} />
		{/if}
	</div>
</DialogWithCloseButton>
