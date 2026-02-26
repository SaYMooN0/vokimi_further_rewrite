<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

	interface Props {
		src: string;
	}

	let { src }: Props = $props();

	const fullSrc = $derived(src.startsWith('blob:') ? src : StorageBucketMain.fileSrc(src));

	let audio: HTMLAudioElement | undefined = $state();

	let paused = $state(true);
	let currentTime = $state(0);
	let duration = $state(0);

	const isPlaying = $derived(!paused);

	const progressPercent = $derived(duration > 0 ? (currentTime / duration) * 100 : 0);

	function togglePlay() {
		if (!audio) return;
		paused = !paused;
	}

	function formatTime(seconds: number) {
		if (!Number.isFinite(seconds)) return '0:00';
		const mins = Math.floor(seconds / 60);
		const secs = Math.floor(seconds % 60);
		return `${mins}:${secs.toString().padStart(2, '0')}`;
	}
</script>

<div class="audio-player">
	<audio
		bind:this={audio}
		src={fullSrc}
		bind:paused
		bind:currentTime
		bind:duration
		preload="metadata"
	/>

	<button
		class="play-btn unselectable"
		onclick={togglePlay}
		aria-label={isPlaying ? 'Pause' : 'Play'}
	>
		{#if isPlaying}
			<svg
				xmlns="http://www.w3.org/2000/svg"
				viewBox="0 0 24 24"
				fill="currentColor"
				stroke="currentColor"
			>
				<path
					d="M4 7C4 5.58579 4 4.87868 4.43934 4.43934C4.87868 4 5.58579 4 7 4C8.41421 4 9.12132 4 9.56066 4.43934C10 4.87868 10 5.58579 10 7V17C10 18.4142 10 19.1213 9.56066 19.5607C9.12132 20 8.41421 20 7 20C5.58579 20 4.87868 20 4.43934 19.5607C4 19.1213 4 18.4142 4 17V7Z"
				/>
				<path
					d="M14 7C14 5.58579 14 4.87868 14.4393 4.43934C14.8787 4 15.5858 4 17 4C18.4142 4 19.1213 4 19.5607 4.43934C20 4.87868 20 5.58579 20 7V17C20 18.4142 20 19.1213 19.5607 19.5607C19.1213 20 18.4142 20 17 20C15.5858 20 14.8787 20 14.4393 19.5607C14 19.1213 14 18.4142 14 17V7Z"
				/>
			</svg>
		{:else}
			<svg
				xmlns="http://www.w3.org/2000/svg"
				viewBox="0 0 24 24"
				fill="currentColor"
				stroke="currentColor"
				stroke-linejoin="round"
			>
				<path
					d="M18.8906 12.846C18.5371 14.189 16.8667 15.138 13.5257 17.0361C10.296 18.8709 8.6812 19.7884 7.37983 19.4196C6.8418 19.2671 6.35159 18.9776 5.95624 18.5787C5 17.6139 5 15.7426 5 12C5 8.2574 5 6.3861 5.95624 5.42132C6.35159 5.02245 6.8418 4.73288 7.37983 4.58042C8.6812 4.21165 10.296 5.12907 13.5257 6.96393C16.8667 8.86197 18.5371 9.811 18.8906 11.154C19.0365 11.7084 19.0365 12.2916 18.8906 12.846Z"
				/>
			</svg>
		{/if}
	</button>

	<div class="progress-container">
		<span class="time">{formatTime(currentTime)}</span>
		<input
			type="range"
			min="0"
			max={duration || 1}
			step="any"
			value={currentTime}
			oninput={(e: Event) => {
				const target = e.target as HTMLInputElement;
				if (audio && duration > 0 && duration !== Infinity) {
					const time = Number(target.value);
					audio.currentTime = time;
				}
			}}
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
		width: 2.5rem;
		height: 2.5rem;
		border-radius: 50%;
		background-color: var(--primary);
		color: var(--primary-foreground);
		cursor: pointer;
		transition:
			transform 0.15s ease,
			background-color 0.15s ease;
		border: none;
	}

	.play-btn:hover {
		background-color: var(--primary-hov);
	}

	.play-btn svg {
		width: 1.25rem;
		height: 1.25rem;
		margin-left: 0.125rem;
		stroke-width: 0;
	}
	.play-btn:active svg {
		transform: scale(0.94);
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
		height: 0.5rem;

		border-radius: 0.25rem;
		outline: none;
		background: linear-gradient(var(--primary), var(--primary)) 0 / var(--progress, 0%) 100%
			no-repeat var(--muted);
		cursor: pointer;
		transition: height 0.1s ease;
	}

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
