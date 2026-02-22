<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	interface Props {
		src: string;
	}
	let { src }: Props = $props();

	let fullSrc = $derived(src.startsWith('blob:') ? src : StorageBucketMain.fileSrc(src));

	let audio: HTMLAudioElement;
	let isPlaying = $state(false);
	let currentTime = $state(0);
	let duration = $state(0);

	function togglePlay() {
		if (!audio) return;
		if (isPlaying) {
			audio.pause();
		} else {
			audio.play();
		}
	}

	function formatTime(seconds: number) {
		if (!seconds || isNaN(seconds)) return '0:00';
		const mins = Math.floor(seconds / 60);
		const secs = Math.floor(seconds % 60);
		return `${mins}:${secs.toString().padStart(2, '0')}`;
	}

	function onTimeUpdate() {
		if (audio) {
			currentTime = audio.currentTime;
		}
	}

	function onLoadedMetadata() {
		if (audio) {
			duration = audio.duration;
			if (duration === Infinity) {
				audio.currentTime = Number.MAX_SAFE_INTEGER;
				audio.ontimeupdate = () => {
					audio.ontimeupdate = onTimeUpdate;
					duration = audio.duration;
					audio.currentTime = 0;
				};
			}
		}
	}

	function onSeek(e: Event) {
		const target = e.target as HTMLInputElement;
		if (audio) {
			const time = Number(target.value);
			audio.currentTime = time;
			currentTime = time;
		}
	}
</script>

<div class="audio-player">
	<audio
		bind:this={audio}
		src={fullSrc}
		onplay={() => (isPlaying = true)}
		onpause={() => (isPlaying = false)}
		ontimeupdate={onTimeUpdate}
		onloadedmetadata={onLoadedMetadata}
		onended={() => (isPlaying = false)}
		preload="metadata"
	></audio>

	<button
		class="play-btn unselectable"
		onclick={togglePlay}
		aria-label={isPlaying ? 'Pause' : 'Play'}
	>
		{#if isPlaying}
			<svg viewBox="0 0 24 24" fill="currentColor"><path d="M6 19h4V5H6v14zm8-14v14h4V5h-4z" /></svg
			>
		{:else}
			<svg viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z" /></svg>
		{/if}
	</button>

	<div class="progress-container">
		<span class="time">{formatTime(currentTime)}</span>
		<input
			type="range"
			min="0"
			max={duration || 1}
			step="0.01"
			value={currentTime}
			oninput={onSeek}
			class="progress-bar"
			style="--progress: {(currentTime / (duration || 1)) * 100}%"
		/>
		<span class="time">{formatTime(duration)}</span>
	</div>
</div>

<style>
	.audio-player {
		display: flex;
		align-items: center;
		gap: 1rem;
		width: 100%;
		padding: 0.75rem 1rem;
		border-radius: 0.75rem;
		background-color: var(--secondary);
		transition: all 0.2s ease;
	}

	.play-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 3rem;
		height: 3rem;
		min-width: 3rem;
		border-radius: 50%;
		background-color: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
		transition:
			transform 0.15s ease,
			background-color 0.15s ease;
	}

	.play-btn:hover {
		background-color: var(--primary-hov);
		transform: scale(1.05);
	}

	.play-btn:active {
		transform: scale(0.95);
	}

	.play-btn svg {
		width: 1.5rem;
		height: 1.5rem;
		margin-left: 0.125rem; /* Visual center for play icon */
	}

	.play-btn svg:has(path[d='M6 19h4V5H6v14zm8-14v14h4V5h-4z']) {
		margin-left: 0; /* Clear margin for pause icon */
	}

	.progress-container {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		flex: 1;
		width: 100%;
	}

	.time {
		font-size: 0.875rem;
		font-weight: 500;
		color: var(--secondary-foreground);
		min-width: 2.5rem;
		text-align: center;
		font-variant-numeric: tabular-nums;
	}

	.progress-bar {
		flex: 1;
		-webkit-appearance: none;
		appearance: none;
		height: 0.375rem;
		border-radius: 0.25rem;
		outline: none;
		background: linear-gradient(var(--primary), var(--primary)) 0 / var(--progress, 0%) 100%
			no-repeat var(--muted);
		cursor: pointer;
		transition: height 0.1s ease;
	}

	.progress-bar:hover {
		height: 0.5rem;
	}

	/* Thumb */
	.progress-bar::-webkit-slider-thumb {
		-webkit-appearance: none;
		appearance: none;
		width: 1rem;
		height: 1rem;
		border-radius: 50%;
		background: var(--primary);
		cursor: pointer;
		transition: transform 0.1s ease;
	}

	.progress-bar::-webkit-slider-thumb:hover {
		transform: scale(1.2);
	}

	.progress-bar::-moz-range-thumb {
		width: 1rem;
		height: 1rem;
		border: none;
		border-radius: 50%;
		background: var(--primary);
		cursor: pointer;
		transition: transform 0.1s ease;
	}

	.progress-bar::-moz-range-thumb:hover {
		transform: scale(1.2);
	}
</style>
