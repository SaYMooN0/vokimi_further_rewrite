<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { ProfilePicShape } from '../types';
	import RoundedSquareProfilePicDisplay from './_c_profile_pic/RoundedSquareProfilePicDisplay.svelte';
	import SquircleProfilePicDisplay from './_c_profile_pic/SquircleProfilePicDisplay.svelte';
	import WavesProfilePicDisplay from './_c_profile_pic/WavesProfilePicDisplay.svelte';

	interface Props {
		key: string;
		shape: ProfilePicShape;
	}

	let { key, shape }: Props = $props();
	const src = $derived(StorageBucketMain.fileSrc(key));
</script>

<div class="profile-picture">
	{#if shape === 'Waves'}
		<WavesProfilePicDisplay {src} />
	{:else if shape === 'Squircle'}
		<SquircleProfilePicDisplay {src} />
	{:else if shape === 'Circle' || shape === 'RoundedSquare20' || shape === 'RoundedSquare30'}
		<RoundedSquareProfilePicDisplay
			src={StorageBucketMain.fileSrc(key)}
			radiusPercent={shape === 'Circle'
				? 50
				: shape === 'RoundedSquare20'
					? 20
					: shape === 'RoundedSquare30'
						? 30
						: 0}
		/>
	{/if}
</div>

<style>
	.profile-picture {
		--profile-picture-size: 12rem;
		width: var(--profile-picture-size);
		height: var(--profile-picture-size);
		flex-shrink: 0;
		margin-top: -5rem;
		position: relative;
		--profile-pic-border: 0.375rem;
		--profile-pic-color: var(--back);
	}
</style>
