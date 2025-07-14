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
		<path
			d="M3 17H19"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M3 21H12"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
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
			d="M14.1706 20.8905C18.3536 20.6125 21.6856 17.2332 21.9598 12.9909C22.0134 12.1607 22.0134 11.3009 21.9598 10.4707C21.6856 6.22838 18.3536 2.84913 14.1706 2.57107C12.7435 2.47621 11.2536 2.47641 9.8294 2.57107C5.64639 2.84913 2.31441 6.22838 2.04024 10.4707C1.98659 11.3009 1.98659 12.1607 2.04024 12.9909C2.1401 14.536 2.82343 15.9666 3.62791 17.1746C4.09501 18.0203 3.78674 19.0758 3.30021 19.9978C2.94941 20.6626 2.77401 20.995 2.91484 21.2351C3.05568 21.4752 3.37026 21.4829 3.99943 21.4982C5.24367 21.5285 6.08268 21.1757 6.74868 20.6846C7.1264 20.4061 7.31527 20.2668 7.44544 20.2508C7.5756 20.2348 7.83177 20.3403 8.34401 20.5513C8.8044 20.7409 9.33896 20.8579 9.8294 20.8905C11.2536 20.9852 12.7435 20.9854 14.1706 20.8905Z"
			stroke="currentColor"
			stroke-linejoin="round"
		/>
		<path
			d="M10.5 9.53846C10.5 8.68879 11.1716 8 12 8C12.8284 8 13.5 8.68879 13.5 9.53846C13.5 9.84473 13.4127 10.1301 13.2623 10.3698C12.8141 11.0844 12 11.7657 12 12.6154V13"
			stroke="currentColor"
			stroke-linecap="round"
		/>
		<path d="M12 15H12.009" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" />
	</svg>
{/snippet}

{#snippet resultsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M6.5 17.5L6.5 14.5M11.5 17.5L11.5 8.5M16.5 17.5V13.5"
			stroke="currentColor"
			stroke-linecap="round"
		/>
		<path
			d="M21.5 5.5C21.5 7.15685 20.1569 8.5 18.5 8.5C16.8431 8.5 15.5 7.15685 15.5 5.5C15.5 3.84315 16.8431 2.5 18.5 2.5C20.1569 2.5 21.5 3.84315 21.5 5.5Z"
			stroke="currentColor"
		/>
		<path
			d="M21.4955 11C21.4955 11 21.5 11.3395 21.5 12C21.5 16.4784 21.5 18.7175 20.1088 20.1088C18.7175 21.5 16.4783 21.5 12 21.5C7.52166 21.5 5.28249 21.5 3.89124 20.1088C2.5 18.7175 2.5 16.4784 2.5 12C2.5 7.52169 2.5 5.28252 3.89124 3.89127C5.28249 2.50003 7.52166 2.50003 12 2.50003L13 2.5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}
{#snippet publishingIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M18 7C18.7745 7.16058 19.3588 7.42859 19.8284 7.87589C21 8.99181 21 10.7879 21 14.38C21 17.9721 21 19.7681 19.8284 20.8841C18.6569 22 16.7712 22 13 22H11C7.22876 22 5.34315 22 4.17157 20.8841C3 19.7681 3 17.9721 3 14.38C3 10.7879 3 8.99181 4.17157 7.87589C4.64118 7.42859 5.2255 7.16058 6 7"
			stroke="currentColor"
			stroke-linecap="round"
		/>
		<path
			d="M12.0253 2.00052L12 14M12.0253 2.00052C11.8627 1.99379 11.6991 2.05191 11.5533 2.17492C10.6469 2.94006 9 4.92886 9 4.92886M12.0253 2.00052C12.1711 2.00657 12.3162 2.06476 12.4468 2.17508C13.3531 2.94037 15 4.92886 15 4.92886"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
	</svg>
{/snippet}
