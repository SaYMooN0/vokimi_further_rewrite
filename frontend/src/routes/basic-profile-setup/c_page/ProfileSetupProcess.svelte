<script lang="ts">
	import type { Language } from '$lib/ts/language';
	import { ProfileSetupProcessState } from './profile-setup-process-state.svelte';
	interface Props {
		initialLangs: Language[];
		initialTags: string[];
		initialProfilePic: string;
	}
	let { initialLangs, initialTags, initialProfilePic }: Props = $props();
	let setupProcessState = new ProfileSetupProcessState(
		initialLangs,
		initialTags,
		initialProfilePic
	);
	type Step = 'languages' | 'tags' | 'display-name' | 'profile-pic';
</script>

<h1>Steps</h1>