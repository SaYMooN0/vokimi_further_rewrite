<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';

	let { searchedTags = $bindable() }: { searchedTags: string[] } = $props<{
		searchedTags: string[];
	}>();
	let tagSearchInput: string = $state('');
	let inputEl: HTMLInputElement;

	function onResetClick(event: MouseEvent) {
		setTimeout(() => {
			inputEl.focus();
		});
		tagSearchInput = '';
		event.stopPropagation();
	}
</script>

<div class="search-bar">
	<svg class="search-icon" viewBox="0 0 24 24" fill="none">
		<path
			d="M17.5 17.5L22 22"
			stroke="currentColor"
			stroke-width="2.5"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M20 11C20 6.02944 15.9706 2 11 2C6.02944 2 2 6.02944 2 11C2 15.9706 6.02944 20 11 20C15.9706 20 20 15.9706 20 11Z"
			stroke="currentColor"
			stroke-width="2.5"
			stroke-linejoin="round"
		/>
	</svg>
	<input
		placeholder="Search for tag..."
		bind:value={tagSearchInput}
		bind:this={inputEl}
		name={'tag-search-' + StringUtils.rndStr(12)}
	/>
	<svg
		class="reset-button"
		fill="none"
		viewBox="0 0 24 24"
		stroke="currentColor"
		stroke-width="2.5"
		onclick={onResetClick}
	>
		<path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"></path>
	</svg>
</div>

<style>
	.search-bar {
		position: relative;
		display: flex;
		align-items: center;
		width: 20rem;
		border: 0.125rem solid var(--back-second);
		border-radius: 1.5rem;
		background-color: var(--back-second);
		transition: border-radius 0.15s ease-out;
		box-sizing: border-box;
	}

	.search-bar:hover {
		border-color: var(--accent-main);
	}

	.search-bar:focus-within {
		border-color: var(--accent-hov);
	}

	.search-bar > input {
		z-index: 2;
		width: 100%;
		height: 100%;
		padding: 0.5rem 1.75rem;
		border: none;
		background-color: transparent;
		font-size: 1rem;
		outline: none;
		box-sizing: border-box;
	}

	.search-bar > svg {
		position: absolute;
		color: var(--gray);
		transition: 0.2s ease;
	}

	.search-icon {
		left: 0.625rem;
		z-index: 1;
		width: 1rem;
		height: 1rem;
	}

	.search-bar:hover .search-icon {
		color: var(--accent-main);
	}

	.search-bar:focus-within .search-icon {
		color: var(--accent-hov);
	}

	.reset-button {
		right: 0.5rem;
		z-index: 3;
		width: 1.125rem;
		height: 1.125rem;
		border-radius: 0.25rem;
		color: var(--gray);
		opacity: 0;
		cursor: pointer;
		box-sizing: border-box;
		visibility: hidden;
	}

	.reset-button:hover {
		color: var(--text-main);
	}

	.search-bar > input:not(:placeholder-shown) ~ .reset-button {
		opacity: 1;
		visibility: visible;
	}
</style>
