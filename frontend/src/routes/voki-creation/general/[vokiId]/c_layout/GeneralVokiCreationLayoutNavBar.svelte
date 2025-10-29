<script lang="ts">
	import { page } from '$app/state';
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import VokiCreationLayoutNavBar from '../../../c_layout/VokiCreationLayoutNavBar.svelte';

	let vokiId: string | undefined = page.params.vokiId;
	function withBasePath(path: string) {
		return '/voki-creation/general/' + vokiId + '/' + path;
	}
</script>

{#if vokiId}
	<VokiCreationLayoutNavBar
		links={[
			{ icon: authorsIcon, name: 'Authors', href: withBasePath('authors') },
			{ icon: mainIcon, name: 'Main Info', href: withBasePath('main') },
			{ icon: questionIcon, name: 'Questions', href: withBasePath('questions') },
			{ icon: resultsIcon, name: 'Results', href: withBasePath('results') },
			{ icon: publishingIcon, name: 'Publishing', href: withBasePath('publishing') },
			{
				icon: publishingIcon,
				name: 'Interactions',
				href: withBasePath('interactions')
			}
		]}
	/>
{:else}
	<ErrView err={{ message: 'Incorrect link. Voki id is not specified' }} />
{/if}
{#snippet authorsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M13 11C13 8.79086 11.2091 7 9 7C6.79086 7 5 8.79086 5 11C5 13.2091 6.79086 15 9 15C11.2091 15 13 13.2091 13 11Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M11.0386 7.55773C11.0131 7.37547 11 7.18927 11 7C11 4.79086 12.7909 3 15 3C17.2091 3 19 4.79086 19 7C19 9.20914 17.2091 11 15 11C14.2554 11 13.5584 10.7966 12.9614 10.4423"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M15 21C15 17.6863 12.3137 15 9 15C5.68629 15 3 17.6863 3 21"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M21 17C21 13.6863 18.3137 11 15 11"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
{/snippet}
{#snippet mainIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path d="M3 4H7" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 10H7" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 15.5H21" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<path d="M3 21.5H21" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
		<rect
			x="11"
			y="2.5"
			width="10"
			height="8"
			rx="2"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}

{#snippet questionIcon()}
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		fill="none"
		stroke-linecap="round"
		stroke-linejoin="round"
	>
		<path d="M1 7h16" stroke="currentColor"></path>
		<path d="M1 13H12" stroke="currentColor"></path>
		<path d="M1 19h12" stroke="currentColor"></path>
		<path d="M18.5 19v0.01" stroke="currentColor"></path>
		<path
			stroke="currentColor"
			d="M18.5 16a2.003 2.003 0 0 0 0.914 -3.782 1.98 1.98 0 0 0 -2.414 0.483"
		/>
	</svg>
{/snippet}

{#snippet resultsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<circle
			cx="18"
			cy="5"
			r="3"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<circle
			cx="6"
			cy="19"
			r="3"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M12 5H8.5C6.567 5 5 6.567 5 8.5C5 10.433 6.567 12 8.5 12H15.5C17.433 12 19 13.567 19 15.5C19 17.433 17.433 19 15.5 19H12"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}
{#snippet publishingIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M19 13C19 17.4183 15.4183 21 11 21C6.58172 21 3 17.4183 3 13C3 8.58172 6.58172 5 11 5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M14 3H18C19.4142 3 20.1213 3 20.5607 3.43934C21 3.87868 21 4.58579 21 6V10M20 4L11 13"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
{/snippet}
