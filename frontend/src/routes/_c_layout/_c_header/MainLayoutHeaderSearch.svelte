<script lang="ts">
	import { goto } from '$app/navigation';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	import { onMount } from 'svelte';

	let inputEl: HTMLInputElement;
	let inputValue = $state('');
	let isFocused = $state(false);

	let filteredUsers = $state<any[]>([]);
	let filteredTags = $state<any[]>([]);
	let filteredVokis = $state<any[]>([]);
	let showSuggestions = $derived(isFocused && inputValue.trim().length > 0);

	function handleKeydown(e: KeyboardEvent) {
		if ((e.altKey || e.metaKey) && e.code === 'KeyK') {
			e.preventDefault();
			inputEl.focus();
		}
		if (e.key === 'Enter' && isFocused) {
			submitSearch();
		}
		if (e.key === 'Escape' && isFocused) {
			inputEl.blur();
		}
	}

	function clearInput() {
		inputValue = '';
		inputEl.focus();
	}

	function handleInput() {
		const val = inputValue.trim();
		if (!val) {
			filteredUsers = [];
			filteredTags = [];
			filteredVokis = [];
			return;
		}

		const lowerVal = val.toLowerCase();

		if (val.startsWith('@')) {
			const query = lowerVal.slice(1);
			filteredUsers = [];
			filteredTags = [];
			filteredVokis = [];
		} else if (val.startsWith('#')) {
			const query = lowerVal.slice(1);
			filteredUsers = [];
			filteredTags = [];
			filteredVokis = [];
		} else {
			filteredUsers = [];
			filteredTags = [];
			filteredVokis = [];
		}
	}

	function submitSearch() {
		if (!inputValue.trim()) return;

		let type = 'vokis';
		if (inputValue.startsWith('@')) type = 'users';
		if (inputValue.startsWith('#')) type = 'tags';

		const q = inputValue.trim();
		goto(`/search-results?q=${encodeURIComponent(q)}&tab=${type}`);
		isFocused = false;
	}
	let isInputEmpty = $derived(inputValue.trim().length === 0);
	function navigateToUser(userId: string) {
		goto(`/authors/${userId}`);
		isFocused = false;
	}

	function navigateToTag(t: string) {
		goto(`/tags/${t}`);
		isFocused = false;
	}

	function navigateToVoki(vokiId: string) {
		goto(`/catalog/${vokiId}`);
		isFocused = false;
	}

	onMount(() => {
		window.addEventListener('keydown', handleKeydown);
		return () => {
			window.removeEventListener('keydown', handleKeydown);
		};
	});
</script>

<div class="header-search" class:focused={isFocused}>
	<div
		class="input-wrapper"
		onclick={() => {
			inputEl.focus();
		}}
	>
		{#if isInputEmpty}
			<svg class="search-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
				<use href="#common-search-icon" />
			</svg>
		{:else}
			<button class="icon-btn" onclick={clearInput} aria-label="Clear search">
				<svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
					<path d="M18 6L6 18M6 6l12 12" />
				</svg>
			</button>
		{/if}
		<input
			bind:this={inputEl}
			bind:value={inputValue}
			oninput={handleInput}
			onfocus={() => (isFocused = true)}
			onblur={() => setTimeout(() => (isFocused = false), 200)}
			type="text"
			placeholder="Search Vokis, @users, #tags"
		/>
		{#if !isInputEmpty}
			<button class="icon-btn arrow-btn" onclick={submitSearch} aria-label="Search">
				<svg
					xmlns="http://www.w3.org/2000/svg"
					viewBox="0 0 24 24"
					fill="none"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				>
					<path d="M14 12L4 12" />
					<path
						d="M18.5859 13.6026L17.6194 14.3639C16.0536 15.5974 15.2707 16.2141 14.6354 15.9328C14 15.6515 14 14.6881 14 12.7613L14 11.2387C14 9.31191 14 8.34853 14.6354 8.06721C15.2707 7.7859 16.0536 8.40264 17.6194 9.63612L18.5858 10.3974C19.5286 11.1401 20 11.5115 20 12C20 12.4885 19.5286 12.8599 18.5859 13.6026Z"
					/>
				</svg>
			</button>
		{/if}
	</div>

	{#if showSuggestions}
		<div class="suggestions-dropdown">
			{#each filteredUsers as user}
				<button class="suggestion-item user" onclick={() => navigateToUser(user)}>
					<BasicUserDisplay userId={user.id} interactionLevel="JustDisplay" />
				</button>
			{/each}

			{#each filteredTags as tag}
				<button class="suggestion-item tag" onclick={() => navigateToTag(tag)}>
					<span class="tag-hash">#</span>
					<span class="tag-text">{tag.name}</span>
					<span class="tag-count">({tag.usageCount} uses)</span>
				</button>
			{/each}

			{#each filteredVokis as voki}
				<button class="suggestion-item voki" onclick={() => navigateToVoki(voki)}>
					<span class="voki-icon">
						<svg
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							width="16"
							height="16"
						>
							<rect x="3" y="3" width="18" height="18" rx="2" ry="2" />
						</svg>
					</span>
					<span class="voki-text">{voki.title}</span>
				</button>
			{/each}

			{#if filteredUsers.length === 0 && filteredTags.length === 0 && filteredVokis.length === 0}
				<div class="no-results">No suggestions</div>
			{/if}

			<button class="see-all-btn" onclick={submitSearch}>
				See all results for "{inputValue}"
				<svg
					width="16"
					height="16"
					viewBox="0 0 24 24"
					fill="none"
					stroke="currentColor"
					stroke-width="2"
				>
					<path d="M5 12h14M12 5l7 7-7 7" />
				</svg>
			</button>
		</div>
	{/if}
</div>

<style>
	.header-search {
		position: relative;
		width: 100%;
		height: 100%;
		max-width: 36rem;
		margin: 0 auto;
		display: flex;
		align-items: center;
	}

	.input-wrapper {
		display: flex;
		align-items: center;
		width: 100%;
		height: 100%;
		background-color: var(--secondary);
		border-radius: 9999px;
		padding: 0 1rem;
		transition: all 0.2s ease;
	}

	.header-search.focused .input-wrapper {
		border-color: var(--primary);
		box-shadow: 0 0 0 2px var(--shadow-color, rgba(0, 0, 0, 0.05));
		background-color: var(--background);
	}

	input {
		flex: 1;
		background: transparent;
		border: none;
		outline: none;
		font-size: 0.9375rem;
		color: var(--foreground);
		padding: 0 0.5rem;
		min-width: 0;
	}

	input::placeholder {
		color: var(--muted-foreground);
	}
	.search-icon {
		width: 1rem;
		height: 1rem;
	}
	.icon-btn,
	.icon-placeholder {
		display: flex;
		align-items: center;
		justify-content: center;
		width: 1.75rem;
		height: 1.75rem;
		padding: 0;
		background: transparent;
		border: none;
		color: var(--muted-foreground);
	}

	.icon-btn {
		cursor: pointer;
		border-radius: 50%;
		transition:
			background-color 0.15s,
			color 0.15s;
	}

	.icon-btn:hover {
		background-color: var(--muted);
		color: var(--foreground);
	}

	.icon-placeholder {
		pointer-events: none;
	}

	.arrow-btn {
		color: var(--primary);
	}

	.arrow-btn:hover {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.icon {
		width: 1.125rem;
		height: 1.125rem;
	}

	.suggestions-dropdown {
		position: absolute;
		top: calc(100% + 0.5rem);
		left: 0;
		width: 100%;
		background-color: var(--popover);
		border: 1px solid var(--border);
		border-radius: 0.75rem;
		box-shadow: var(--shadow-lg);
		padding: 0.5rem;
		z-index: 50;
		max-height: 20rem;
		overflow-y: auto;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.suggestion-item {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		width: 100%;
		padding: 0.5rem;
		border: none;
		background: transparent;
		border-radius: 0.5rem;
		text-align: left;
		font-size: 0.9375rem;
		cursor: pointer;
		color: var(--foreground);
		transition: background-color 0.15s;
	}

	.suggestion-item:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.tag-hash {
		color: var(--primary);
		font-weight: bold;
	}

	.tag-count {
		margin-left: auto;
		font-size: 0.75rem;
		color: var(--muted-foreground);
	}

	.voki-icon {
		color: var(--muted-foreground);
		display: flex;
		flex-shrink: 0;
	}

	.voki-text {
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.no-results {
		padding: 0.75rem;
		text-align: center;
		color: var(--muted-foreground);
		font-size: 0.875rem;
	}

	.see-all-btn {
		width: 100%;
		padding: 0.75rem 0.5rem;
		border-top: 1px solid var(--border);
		margin-top: 0.25rem;
		background: transparent;
		border: none;
		border-top: 1px solid var(--border);
		color: var(--primary);
		font-weight: 500;
		cursor: pointer;
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 0.5rem;
		font-size: 0.875rem;
		border-radius: 0 0 0.5rem 0.5rem;
	}

	.see-all-btn:hover {
		background-color: var(--accent);
	}
</style>
