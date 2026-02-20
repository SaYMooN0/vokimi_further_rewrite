<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';

	interface Props {
		state: 'initLoading' | 'existsCheckLoading' | 'existsCheckFailed' | 'success';
	}
	let { state }: Props = $props();

	type CircleType = 'loading' | 'success' | 'failed' | 'empty';

	const circle1Type = $derived<CircleType>(state === 'initLoading' ? 'loading' : 'success');
	const circle2Type = $derived.by<CircleType>(() => {
		if (state === 'initLoading') {
			return 'empty';
		}
		if (state === 'existsCheckLoading') {
			return 'loading';
		}
		if (state === 'existsCheckFailed') {
			return 'failed';
		}
		return 'success';
	});
</script>

<div class="stepper">
	<div class="connector" class:active={true}></div>

	<div class="step">
		{@render circle(circle1Type)}
		<span class="step-label">Initialize</span>
	</div>

	<div class="connector" class:active={state !== 'initLoading'}></div>

	<div class="step">
		{@render circle(circle2Type)}
		<span class="step-label">Verify</span>
	</div>

	<div class="connector" class:active={state === 'success' || state === 'existsCheckFailed'}></div>
</div>

{#snippet circle(type: CircleType)}
	<div
		class="circle"
		class:loading={type === 'loading'}
		class:success={type === 'success'}
		class:failed={type === 'failed'}
		class:empty={type === 'empty'}
	>
		{#if type === 'loading'}
			<LinesLoader sizeRem={1.75} strokePx={2} color="var(--primary)" speedSec={1.2} />
		{:else if type === 'success'}
			<svg class="icon"><use href="#common-check-icon" /></svg>
		{:else if type === 'failed'}
			<svg class="icon"><use href="#common-warning-icon" /></svg>
		{:else if type === 'empty'}
			<span class="step-number">2</span>
		{/if}
	</div>
{/snippet}

<style>
	.stepper {
		display: grid;
		grid-template-columns: 4rem 4.375rem 4rem 4.375rem 4rem;
		align-items: flex-start;
		justify-content: center;
		margin: 0 auto;
		--circle-size: 3.5rem;
	}

	.step {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		flex-shrink: 0;
	}

	.step-label {
		font-size: 0.75rem;
		font-weight: 550;
		color: var(--muted-foreground);
		letter-spacing: 0.06em;
		text-transform: uppercase;
		user-select: none;
	}

	.connector {
		width: 100%;
		height: 0.125rem;
		margin-top: calc(var(--circle-size) / 2);
		background-color: var(--muted);
		border-radius: 100vmax;
		transition: background-color 0.5s ease;
	}

	.connector.active {
		background-color: var(--primary);
	}

	.circle {
		width: var(--circle-size);
		height: var(--circle-size);
		border-radius: 50%;
		border: 0.125rem solid var(--muted);
		display: flex;
		align-items: center;
		justify-content: center;
		background-color: var(--back);
		color: var(--muted-foreground);
		transition:
			background-color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			border-color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			color 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			box-shadow 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			transform 0.45s cubic-bezier(0.22, 1, 0.36, 1),
			opacity 0.45s ease;
		position: relative;
		z-index: 2;
		flex-shrink: 0;
	}

	.circle.loading {
		border-color: var(--primary);
		box-shadow: 0 0 0 0.25rem var(--accent);
	}

	.circle.success {
		background-color: var(--primary);
		border-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: var(--shadow-md);
	}

	.circle.failed {
		background-color: var(--warn-back);
		color: var(--warn-foreground);
		border-color: var(--warn-foreground);
	}

	.circle.empty {
		opacity: 0.75;
		border-style: dashed;
	}
	.icon {
		width: 2rem;
		height: 2rem;
		flex-shrink: 0;
		stroke-width: 1.675;
	}

	.circle.success .icon {
		animation: popIn 0.45s cubic-bezier(0.175, 0.885, 0.32, 1.275) both;
	}

	.circle.failed .icon {
		animation: popIn 0.45s cubic-bezier(0.175, 0.885, 0.32, 1.275) both;
	}

	.step-number {
		font-size: 1rem;
		font-weight: 600;
		color: var(--muted-foreground);
	}

	@keyframes popIn {
		from {
			transform: scale(0) rotate(-30deg);
			opacity: 0;
		}
		to {
			transform: scale(1) rotate(0);
			opacity: 1;
		}
	}
</style>
