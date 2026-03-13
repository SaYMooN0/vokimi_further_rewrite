<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSubheader from './_c_shared/ProfileSidebarSubheader.svelte';

	interface Props {
		aboutMe: Extract<PossiblyHidden<string>, { showOnProfile: true }>;
	}

	let { aboutMe }: Props = $props();
</script>

<div class="sidebar-about-me-section">
	<ProfileSidebarSubheader title="About me" />
	<p>{aboutMe.value}</p>
</div>

<style>
	.sidebar-about-me-section {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
</style>
