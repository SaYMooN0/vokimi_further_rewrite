<script lang="ts">
	import MyVokisLink from './c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './c_layout/VokiInitializingDialog.svelte';
	import type { Snippet } from 'svelte';
	import { page } from '$app/state';

	const { children }: { children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;
</script>

<div class="links-container">
	<MyVokisLink
		text="Draft Vokis"
		href="/my-vokis/draft"
		isCurrent={page.data.currentTab === 'draft'}
	/>
	<MyVokisLink
		text="Published Vokis"
		href="/my-vokis/published"
		isCurrent={page.data.currentTab === 'published'}
	/>
</div>
<VokiInitializingDialog bind:this={vokiInitializingDialog} />
<div class="my-vokis-page-content">
	{@render children()}
</div>
<div class="new-voki-container">
	<button class="new-voki-btn" onclick={() => vokiInitializingDialog.open()}>New voki</button>
</div>

<style>
	* {
		--container-height: 3.5rem;
	}

	.links-container {
		position: sticky;
		top: 2rem;
		display: grid;
		gap: 1rem;
		width: fit-content;
		height: var(--container-height);
		box-sizing: border-box;
		padding: 0.75rem 5rem;
		margin: 0.5rem auto;
		border-radius: 2rem;
		background-color: var(--back);
		grid-template-columns: 1fr 1fr;
	}

	.my-vokis-page-content {
		margin-bottom: 1rem;
	}

	.new-voki-container {
		position: fixed;
		bottom: 1rem;
		left: 50%;
		transform: translateX(-50%);
	}
</style>
