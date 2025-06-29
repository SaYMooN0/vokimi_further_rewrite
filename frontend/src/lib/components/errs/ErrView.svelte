<script lang="ts">
	import { ErrUtils, type Err } from '$lib/ts/err';

	const { err }: { err: Err } = $props<{ err: Err }>();
	let showAdditional = $state(false);

	let iconElement = $state<SVGSVGElement>()!;

	function toggleDetails() {
		showAdditional = !showAdditional;
		if (iconElement) {
			iconElement.classList.remove('rotate-down', 'rotate-up');
			iconElement.classList.add(showAdditional ? 'rotate-down' : 'rotate-up');
		}
	}
</script>

<div class="err-container">
	<div class="err-message">
		{err.message}
		{#if ErrUtils.hasSomethingExceptMessage(err)}
			<svg
				bind:this={iconElement}
				xmlns="http://www.w3.org/2000/svg"
				viewBox="0 0 24 24"
				fill="none"
				onclick={toggleDetails}
			>
				<path
					d="M17.9998 15C17.9998 15 13.5809 9.00001 11.9998 9C10.4187 8.99999 5.99985 15 5.99985 15"
					stroke="currentColor"
					stroke-width="2"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>
		{/if}
	</div>
	{#if ErrUtils.hasNonEmptyDetails(err)}
		<label class="err-additional" class:hidden={!showAdditional}>
			Details: {err.details}
		</label>
	{/if}
	{#if ErrUtils.hasSpecifiedCode(err)}
		<label class="err-additional" class:hidden={!showAdditional}>
			Code: {err.code}
		</label>
	{/if}
</div>

<style>
	.err-container {
		display: flex;
		flex-direction: column;
		box-sizing: border-box;
		padding: 0.125rem 0.5rem;
		border-radius: 0.25rem;
		box-shadow: var(--err-shadow);
		background-color: var(--err-back);
		color: var(--err-foreground);
	}

	.err-message {
		display: grid;
		grid-template-columns: 1fr 1.5rem;
		align-items: center;
		min-height: 1.5rem;
		margin: 0;
		font-size: 1rem;
		font-weight: 440;
		word-break: normal;
		overflow-wrap: anywhere;
		color: inherit;
	}

	.err-message > svg {
		transition: transform 0.17s ease-in;
		transform: scale(1.2);
		cursor: pointer;
	}

	:global(.err-message > .rotate-down) {
		animation: rotate-down 0.4s ease-in-out forwards;
	}

	:global(.err-message > .rotate-up) {
		animation: rotate-up 0.4s ease-in-out forwards;
	}

	.err-additional {
		height: auto;
		box-sizing: border-box;
		margin: 0.125rem 0 0.125rem 0.25rem;
		color: inherit;
		font-size: 1rem;
		opacity: 1;
		transition: all 0.2s ease;
		interpolate-size: allow-keywords;
		font-weight: 420;
	}

	.hidden {
		height: 0;
		margin: 0;
		font-size: 0;
		opacity: 0;
	}

	@keyframes rotate-down {
		from {
			transform: rotate(0deg) scale(1.2);
		}

		to {
			transform: rotate(-180deg) scale(1.2);
		}
	}

	@keyframes rotate-up {
		from {
			transform: rotate(-180deg) scale(1.2);
		}

		to {
			transform: rotate(0deg) scale(1.2);
		}
	}
</style>
