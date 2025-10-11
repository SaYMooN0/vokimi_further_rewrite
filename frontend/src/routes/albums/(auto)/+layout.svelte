<script lang="ts">
	import { page } from '$app/state';
	import type { Snippet } from 'svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';

	let { children }: { children: Snippet } = $props();
</script>

{#if page.data.response.isSuccess}
	{@render children()}
{:else}
	<div class="msg-container">
		<h1>Could not load your {page.data.albumName} auto album</h1>
		<a href="/albums" class="go-back-link">Go back to albums</a>
		<DefaultErrBlock errList={page.data.response.errs} />
	</div>
{/if}

<style>
	.msg-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		margin: 6rem auto 4rem;
	}
	.msg-container > h1 {
		color: var(--muted-foreground);
		font-size: 2.25rem;
		font-weight: 600;
		letter-spacing: 0.5px;
		text-align: center;
		text-wrap: pretty;
	}
	.go-back-link {
		margin: 1.5rem 0 3rem;
		padding: 0.25rem 1.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		border-radius: 5rem;
		letter-spacing: 0.125px;
		transition: all 0.06s ease-in;
	}
	.go-back-link:hover,
	.go-back-link:focus,
	.go-back-link:active {
		background-color: var(--primary-hov);
		padding: 0.25rem 1.675rem;
		letter-spacing: 0.4px;
	}
	.msg-container > :global(.err-block) {
		max-width: 60vw;
	}
</style>
