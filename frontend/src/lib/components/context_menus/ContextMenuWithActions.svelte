<script lang="ts">
	import type { Snippet } from 'svelte';
	import BaseContextMenu from './BaseContextMenu.svelte';

	export type ActionsContextMenuJustContent = { type: 'content'; content: Snippet };
	export type ActionsContextMenuActionsContent = { type: 'actions'; actions: Action[] };

	type ActionType = 'default' | 'red';
	type Action = {
		label: string;
		icon?: Snippet;
		action: { isLink: true; href: string } | { isLink: false; onclick: () => void };
		type: ActionType;
	};

	interface Props {
		content: ActionsContextMenuJustContent | ActionsContextMenuActionsContent;
		onAfterClose?: () => void;
		class?: string;
		id?: string;
	}
	let { content, onAfterClose, class: className = '', id }: Props = $props();
	let menu = $state<BaseContextMenu>()!;
	export function open(x: number, y: number, ox = 0, oy = 0) {
		menu.open(x, y, ox, oy);
	}
</script>

<BaseContextMenu bind:this={menu} class="generic-context-menu {className}" {onAfterClose} {id}>
	{#if content.type === 'actions'}
		{#each content.actions as a}
			{#if a.action.isLink}
				<a class="action {a.type === 'red' ? 'red' : ''}" href={a.action.href}>
					{@render actionContent(a)}
				</a>
			{:else if !a.action.isLink}
				<div
					class="action {a.type === 'red' ? 'red' : ''}"
					onclick={() => (a.action as { onclick: () => void }).onclick()}
				>
					{@render actionContent(a)}
				</div>
			{/if}
		{/each}
	{:else}
		{@render content.content()}
	{/if}
</BaseContextMenu>
{#snippet actionContent(a: { label: string; icon?: Snippet })}
	<div class="icon-container">
		{@render a.icon?.()}
	</div>
	<span>{a.label}</span>
{/snippet}

<style>
	:global(.generic-context-menu) {
		display: grid;
		background-color: var(--back);
		color: var(--muted-foreground);
		border-radius: 0.25rem;
		box-shadow: var(--shadow-xs);
		width: max-content;
		padding: 0.125rem;
	}

	.action {
		display: grid;
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.375rem;
		padding: 0.25rem 1.25rem 0.25rem 0.5rem;
		cursor: default;
		border-radius: inherit;
		font-size: 1rem;
		font-weight: 410;
		color: inherit;
		text-decoration: none;
	}

	.icon-container {
		width: 1.125rem;
		height: 1.125rem;
	}
	.icon-container > :global(svg) {
		width: 100%;
		height: 100%;
		stroke-width: 1.675;
		color: inherit;
	}
	.action:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.action.red:hover {
		background-color: var(--err-back);
		color: var(--err-foreground);
	}
</style>
