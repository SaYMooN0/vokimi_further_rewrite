<script lang="ts">
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import type { AnswerDataColorOnly } from '../../../../../../types';
	import AnswerEditingColorInput from './c_shared/AnswerEditingColorInput.svelte';

	let { answer = $bindable() }: { answer: AnswerDataColorOnly } = $props<{
		answer: AnswerDataColorOnly;
	}>();

	function normalizeForShades(v: string): string {
		let t = (v ?? '').trim();
		if (!t.startsWith('#')) t = '#' + t;
		return ColorUtils.isHex6(t) ? t.toUpperCase() : '#000000';
	}
	let base = $derived(normalizeForShades(answer.color));
	let shades = $derived([
		ColorUtils.adjustLightness(base, +21),
		ColorUtils.adjustLightness(base, +14),
		ColorUtils.adjustLightness(base, +7),
		base.toUpperCase(),
		ColorUtils.adjustLightness(base, -7),
		ColorUtils.adjustLightness(base, -14),
		ColorUtils.adjustLightness(base, -21)
	]);
</script>

<div class="answer-content">
	<AnswerEditingColorInput bind:color={answer.color} />
	<div class="shades">
		{#each shades as shade}
			<div class="shade" style="--shade:{shade}" onclick={() => (answer.color = shade)}></div>
		{/each}
	</div>
	<div class="divider"></div>

	<div class="presets">
		Choose from presets
		<div class="presets-list">
			{#each ColorUtils.colorPresets as p}
				<div class="preset" style="--preset:{p}" onclick={() => (answer.color = p)}></div>
			{/each}
		</div>
	</div>
</div>

<style>
	.answer-content {
		display: flex;
		width: 100%;
		height: 100%;
		overflow: hidden;
		align-items: center;
		justify-content: space-between;
		padding: 0.5rem 2rem 0;
	}

	.divider {
		width: 0.125rem;
		height: calc(100% - 1rem);
		background-color: var(--muted);
		border-radius: 0.25rem;
	}

	.shades {
		display: flex;
		flex-direction: row;
		height: fit-content;
		justify-content: center;
		border-radius: 0.875rem;
		border: 0.125rem solid var(--secondary-foreground);
		background-color: var(--secondary-foreground);
	}

	.shade {
		width: 2.5rem;
		height: 7rem;
		background: var(--shade);
		cursor: pointer;
		transition: transform 0.2s ease-out;
		z-index: 1;
	}
	.shade:first-child {
		border-radius: 0.75rem 0 0 0.75rem;
		transform-origin: left;
	}

	.shade:last-child {
		border-radius: 0 0.75rem 0.75rem 0;
		transform-origin: right;
	}
	.shade:hover {
		transform: scaleX(1.2);
		z-index: 2;
	}
	.presets {
		display: flex;
		flex-direction: column;
		justify-content: center;
		font-size: 1rem;
		color: var(--secondary-foreground);
		margin-bottom: 0.25rem;
		width: fit-content;
		gap: 0.5rem;
		text-align: center;
	}

	.presets-list {
		display: grid;
		gap: 0.5rem;
		grid-template-columns: 1fr 1fr 1fr 1fr;
		grid-template-rows: 1fr 1fr;
	}

	.preset {
		height: 2.5rem;
		aspect-ratio: 1/1;
		border-radius: 0.5rem;
		background: var(--preset);
		cursor: pointer;
		box-shadow: var(--shadow-md), var(--shadow-xs);
		transition: all 0.12s ease-out;
	}
	.preset:hover {
		transform: scale(1.1);
	}
</style>
