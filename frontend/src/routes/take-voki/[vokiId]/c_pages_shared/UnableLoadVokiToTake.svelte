<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';

	let { vokiId, vokiTypeName, errs }: { vokiId: string; vokiTypeName: string; errs: Err[] } =
		$props<{
			vokiId: string;
			vokiTypeName: string;
			errs: Err[];
		}>();
</script>

<div class="unable-to-load">
	<h1>
		Unable to load requested {vokiTypeName} voki
	</h1>
	<a class="link" href="/catalog/{vokiId}">Go to this voki page</a>
	<DefaultErrBlock errList={errs} />
	<p>Voki id: {vokiId}</p>
</div>

<style>
	.unable-to-load {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
	}
	.link {
		background-color: var(--primary);
		text-decoration: none;
	}
</style>
