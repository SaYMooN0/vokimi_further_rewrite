<script lang="ts">
	import VokiCreationLayoutNavBar from '../../../c_shared/VokiCreationLayoutNavBar.svelte';
	import { page } from '$app/state';

	let vokiId: string = page.params.vokiId;
	function withBasePath(path: string) {
		return '/voki-creation/general/' + vokiId + '/' + path;
	}
</script>

<VokiCreationLayoutNavBar
	links={[
		{ icon: mainIcon, name: 'Main', href: withBasePath('main') },
		{ icon: questionIcon, name: 'Questions', href: withBasePath('questions') },
		{ icon: resultsIcon, name: 'Results', href: withBasePath('results') }
	]}
/>

{#snippet mainIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M12 22H10.5C6.72873 22 4.8431 22 3.67153 20.8284C2.49995 19.6568 2.49997 17.7712 2.5 13.9999L2.50004 9.99993C2.50007 6.22872 2.50008 4.34312 3.67166 3.17156C4.84323 2 6.72883 2 10.5 2H11.4999C15.2712 2 17.1568 2 18.3284 3.17157C19.4999 4.34315 19.4999 6.22876 19.4999 10V11"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M7 7H15M7 12H11.5"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		/>
		<path
			d="M17.5 20.7727C19.2673 20.7727 20.7 19.3075 20.7 17.5C20.7 15.6925 19.2673 14.2273 17.5 14.2273M17.5 20.7727C15.7327 20.7727 14.3 19.3075 14.3 17.5C14.3 15.6925 15.7327 14.2273 17.5 14.2273M17.5 20.7727V22M17.5 14.2273V13M14.5913 15.7149L13.5004 15.0455M21.5 19.9545L20.4092 19.2851M20.4087 15.7149L21.4996 15.0455M13.5 19.9545L14.5908 19.2851"
			stroke="currentColor"
			stroke-linecap="round"
		/>
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
