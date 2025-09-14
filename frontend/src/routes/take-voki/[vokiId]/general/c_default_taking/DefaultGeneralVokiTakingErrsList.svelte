<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ErrUtils, type Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	interface Props {
		errs: (Err & { questionOrder?: number })[];
		jumpToSpecificQuestion: (questionOrder: number) => Err[];
	}
	let { errs, jumpToSpecificQuestion }: Props = $props();
	function onErrClick(err: Err & { questionOrder?: number }) {
		if (err.questionOrder) {
			const resErrs = jumpToSpecificQuestion(err.questionOrder);
			if (resErrs.length > 0) toast.error(resErrs[0].message);
		}
	}
</script>

<div class="errs">
	{#each errs as err}
		<div class="err" class:can-jump={err.questionOrder} onclick={() => onErrClick(err)}>
			<label class="message">
				{err.message}
			</label>
			<label class="details">
				{#if ErrUtils.hasNonEmptyDetails(err)}
					{err.details}
				{/if}
			</label>
		</div>
	{/each}
</div>

<style>
	.err.can-jump:hover {
		text-decoration: underline;
	}
</style>
