<script lang="ts">
	import { page } from '$app/state';
	import VokiCreationLayoutNavBar from '../../../c_layout/VokiCreationLayoutNavBar.svelte';

	let vokiId: string = page.params.vokiId;
	function withBasePath(path: string) {
		return '/voki-creation/general/' + vokiId + '/' + path;
	}
</script>

<VokiCreationLayoutNavBar
	links={[
		{ icon: authorsIcon, name: 'Authors', href: withBasePath('authors') },
		{ icon: mainIcon, name: 'Main Info', href: withBasePath('main') },
		{ icon: questionIcon, name: 'Questions', href: withBasePath('questions') },
		{ icon: resultsIcon, name: 'Results', href: withBasePath('results') },
		{ icon: publishingIcon, name: 'Publishing', href: withBasePath('publishing') }
	]}
/>
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
		<path d="M3 17H19" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"></path>
		<path d="M3 21H12" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"></path>
		<path
			d="M3 8C3 5.64298 3 4.46447 3.73223 3.73223C4.46447 3 5.64298 3 8 3H16C18.357 3 19.5355 3 20.2678 3.73223C21 4.46447 21 5.64298 21 8C21 10.357 21 11.5355 20.2678 12.2678C19.5355 13 18.357 13 16 13H8C5.64298 13 4.46447 13 3.73223 12.2678C3 11.5355 3 10.357 3 8Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
{/snippet}

{#snippet questionIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M2.5 12C2.5 7.52166 2.5 5.28249 3.89124 3.89124C5.28249 2.5 7.52166 2.5 12 2.5C16.4783 2.5 18.7175 2.5 20.1088 3.89124C21.5 5.28249 21.5 7.52166 21.5 12C21.5 16.4783 21.5 18.7175 20.1088 20.1088C18.7175 21.5 16.4783 21.5 12 21.5C7.52166 21.5 5.28249 21.5 3.89124 20.1088C2.5 18.7175 2.5 16.4783 2.5 12Z"
			stroke="currentColor"
		></path>
		<path
			d="M9.5 9.5C9.5 8.11929 10.6193 7 12 7C13.3807 7 14.5 8.11929 14.5 9.5C14.5 10.3569 14.0689 11.1131 13.4117 11.5636C12.7283 12.0319 12 12.6716 12 13.5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M12.0001 17H12.009"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
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
