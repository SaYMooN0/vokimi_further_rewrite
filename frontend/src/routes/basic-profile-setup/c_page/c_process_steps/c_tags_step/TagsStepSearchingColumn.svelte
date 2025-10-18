<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiTags } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { useDebounce } from 'runed';
	interface Props {
		isTagChosen: (tag: string) => boolean;
		chooseTag: (tag: string) => void;
		maxTagLength: number;
	}
	let { isTagChosen, chooseTag, maxTagLength }: Props = $props();

	let searchedTags: string[] = $state([]);
	let errs: Err[] = $state([]);
	let searchInput: string = $state('');
	let inputEl: HTMLInputElement;

	function onResetClick(event: MouseEvent) {
		setTimeout(() => {
			inputEl.focus();
		});
		searchInput = '';
		event.stopPropagation();
		errs = [];
	}

	const performSearch = useDebounce(async () => {
		const value = searchInput.trim();

		if (value === '') {
			searchedTags = [];
			errs = [];
			return;
		}
		const response = await ApiTags.fetchJsonResponse<{ tags: string[] }>(
			`/search?count=8&searchValue=${value}`,
			{ method: 'GET' }
		);

		if (response.isSuccess) {
			searchedTags = response.data.tags;
			errs = [];
		} else {
			errs = response.errs;
		}
	}, 260);
</script>

<div class="tags-searching">
	<div class="search-bar">
		<svg class="search-icon" viewBox="0 0 24 24" fill="none">
			<path
				d="M17.5 17.5L22 22"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
			<path
				d="M20 11C20 6.02944 15.9706 2 11 2C6.02944 2 2 6.02944 2 11C2 15.9706 6.02944 20 11 20C15.9706 20 20 15.9706 20 11Z"
				stroke="currentColor"
				stroke-linejoin="round"
			/>
		</svg>
		<input
			placeholder="Search for tag..."
			bind:value={searchInput}
			bind:this={inputEl}
			oninput={() => performSearch()}
			name={'tag-search-' + StringUtils.rndStr(12)}
			maxlength={maxTagLength}
		/>
		<svg class="reset-button" fill="none" viewBox="0 0 24 24" onclick={onResetClick}>
			<use href="#common-cross-icon" />
		</svg>
	</div>
	<div class="searched-tags-list">
		{#each searchedTags as tag}
			<div class="tag">
				#{tag}
				{#if isTagChosen(tag)}
					<svg class="added-icon"><use href="#common-check-icon" /></svg>
				{:else}
					<svg class="add-icon" onclick={() => chooseTag(tag)}><use href="#common-plus-icon" /></svg
					>
				{/if}
			</div>
		{/each}
		<label class="continue-typing-label">
			If you don't find the tag you need continue entering the name of the tag
		</label>
	</div>
	<DefaultErrBlock errList={errs} />
</div>

<style>
	.tags-searching {
		width: 100%;
		display: grid;
		grid-template-rows: auto 1fr auto;
		gap: 0.5rem;
	}
	.search-bar {
		display: grid;
		align-items: center;
		justify-self: center;
		width: calc(100% - 2rem);
		height: 2.25rem;
		box-sizing: border-box;
		padding: 0 0.5rem 0 0.75rem;
		border: 0.125rem solid var(--secondary);
		border-radius: 2rem;
		background-color: var(--secondary);
		transition: border-color 0.05s ease-out;
		grid-template-columns: auto 1fr auto;
		box-shadow: var(--shadow-xs), var(--shadow-md);
	}

	.search-bar:hover {
		box-shadow: var(--shadow-md);
		border-color: var(--secondary-foreground);
	}
	.search-bar:focus-within {
		box-shadow: none;

		border-color: var(--primary);
	}

	.search-bar > input {
		z-index: 2;
		width: 100%;
		height: 100%;
		box-sizing: border-box;
		padding-left: 0.125rem;
		border: none;
		background-color: transparent;
		font-size: 1rem;
		outline: none;
	}

	input::placeholder {
		color: var(--secondary-foreground);
	}

	.search-bar > svg {
		height: 1.125rem;
		stroke-width: 2.5;
		color: var(--secondary-foreground);
		transition: 0.2s ease;
	}

	.search-bar:focus-within .search-icon {
		color: var(--primary);
	}

	.reset-button {
		box-sizing: border-box;
		padding: 0.0625rem;
		border-radius: 0.25rem;
		color: var(--secondary-foreground);
		opacity: 0;
		cursor: pointer;
		visibility: hidden;
	}

	.reset-button:hover {
		background-color: var(--muted);
	}

	.search-bar > input:not(:placeholder-shown) ~ .reset-button {
		opacity: 1;
		visibility: visible;
	}
	.searched-tags-list {
		max-height: 20rem;
		overflow-y: auto;
		scrollbar-gutter: stable;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}
	.tag {
		width: 100%;
		background-color: var(--secondary);
		display: grid;
		grid-template-columns: 1fr auto;
		box-shadow: var(--shadow-xs) inset;
		align-items: center;
		padding: 0.25rem 0.5rem;
		border-radius: 0.375rem;
		font-size: 1.125rem;
		color: var(--secondary-foreground);
	}
	.tag svg {
		height: 1.375rem;
		width: 1.375rem;
		stroke-width: 2.5;
		color: var(--secondary-foreground);
		border-radius: 0.25rem;
	}
	.tag .add-icon:hover,
	.tag .add-icon:focus,
	.tag .add-icon:active {
		color: var(--primary);
		background-color: var(--muted);
	}
	.tag .added-icon {
		color: var(--primary);
		stroke-width: 2.875;
	}
	.continue-typing-label {
		font-size: 0.875rem;
		color: var(--secondary-foreground);
		text-align: center;
		text-wrap: balance;
	}
</style>
