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
			{
				icon: interactionSettingsIcon,
				name: 'Interactions',
				href: withBasePath('interactions')
			},
			{ icon: publishingIcon, name: 'Publishing', href: withBasePath('publishing') }
		]}
	/>
{:else}
	<ErrView err={{ message: 'Incorrect link. Voki id is not specified' }} />
{/if}
{#snippet authorsIcon()}
	<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M1.87399 17.625c0 0.6962 0.27656 1.3639 0.76885 1.8562 0.49228 0.4922 1.15996 0.7688 1.85615 0.7688 0.6962 0 1.36388 -0.2766 1.85616 -0.7688 0.49228 -0.4923 0.76884 -1.16 0.76884 -1.8562 0 -0.3447 -0.06789 -0.6861 -0.19981 -1.0045 -0.13192 -0.3185 -0.32528 -0.6079 -0.56903 -0.8517 -0.24376 -0.2437 -0.53313 -0.4371 -0.85161 -0.569C5.18506 15.0679 4.84371 15 4.49899 15s-0.68606 0.0679 -1.00454 0.1998c-0.31848 0.1319 -0.60786 0.3253 -0.85161 0.569 -0.24376 0.2438 -0.43711 0.5332 -0.56903 0.8517 -0.13192 0.3184 -0.19982 0.6598 -0.19982 1.0045Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M8.24899 23.25c-0.36249 -0.6797 -0.90287 -1.248 -1.56339 -1.6443C6.02508 21.2093 5.26928 21 4.49899 21c-0.77028 0 -1.52609 0.2093 -2.1866 0.6057 -0.66052 0.3963 -1.20091 0.9646 -1.563397 1.6443"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M16.874 17.625c0 0.3447 0.0679 0.6861 0.1998 1.0045 0.1319 0.3185 0.3253 0.6079 0.569 0.8517 0.2438 0.2437 0.5332 0.4371 0.8516 0.569 0.3185 0.1319 0.6599 0.1998 1.0046 0.1998 0.3447 0 0.6861 -0.0679 1.0045 -0.1998 0.3185 -0.1319 0.6079 -0.3253 0.8516 -0.569 0.2438 -0.2438 0.4372 -0.5332 0.5691 -0.8517 0.1319 -0.3184 0.1998 -0.6598 0.1998 -1.0045 0 -0.3447 -0.0679 -0.6861 -0.1998 -1.0045 -0.1319 -0.3185 -0.3253 -0.6079 -0.5691 -0.8517 -0.2437 -0.2437 -0.5331 -0.4371 -0.8516 -0.569 -0.3184 -0.1319 -0.6598 -0.1998 -1.0045 -0.1998 -0.3447 0 -0.6861 0.0679 -1.0046 0.1998 -0.3184 0.1319 -0.6078 0.3253 -0.8516 0.569 -0.2437 0.2438 -0.4371 0.5332 -0.569 0.8517 -0.1319 0.3184 -0.1998 0.6598 -0.1998 1.0045Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M23.249 23.25c-0.3625 -0.6797 -0.9029 -1.248 -1.5634 -1.6443C21.0251 21.2093 20.2693 21 19.499 21s-1.5261 0.2093 -2.1866 0.6057c-0.6605 0.3963 -1.2009 0.9646 -1.5634 1.6443"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M9.37399 3.375c0 0.34472 0.0679 0.68606 0.19982 1.00454 0.13192 0.31848 0.32527 0.60786 0.56899 0.85162 0.2438 0.24375 0.5332 0.43711 0.8516 0.56902C11.3129 5.9321 11.6543 6 11.999 6c0.3447 0 0.6861 -0.0679 1.0045 -0.19982 0.3185 -0.13191 0.6079 -0.32527 0.8516 -0.56902 0.2438 -0.24376 0.4372 -0.53314 0.5691 -0.85162 0.1319 -0.31848 0.1998 -0.65982 0.1998 -1.00454s-0.0679 -0.68606 -0.1998 -1.00454c-0.1319 -0.31848 -0.3253 -0.60786 -0.5691 -0.85162 -0.2437 -0.24375 -0.5331 -0.43711 -0.8516 -0.569024C12.6851 0.817898 12.3437 0.75 11.999 0.75c-0.3447 0 -0.6861 0.067898 -1.0046 0.199816 -0.3184 0.131914 -0.6078 0.325274 -0.8516 0.569024 -0.24372 0.24376 -0.43707 0.53314 -0.56899 0.85162 -0.13192 0.31848 -0.19982 0.65982 -0.19982 1.00454Z"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M15.248 8.25002c-0.4008 -0.47055 -0.8989 -0.84847 -1.4601 -1.10762 -0.5611 -0.25916 -1.1718 -0.39338 -1.7899 -0.39338 -0.6181 0 -1.2288 0.13422 -1.79 0.39338 -0.56108 0.25915 -1.05925 0.63707 -1.46001 1.10762"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M9.04999 19.707c1.91521 0.7301 4.03321 0.7237 5.94401 -0.018"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M6.34799 6c-0.82097 0.7689 -1.47533 1.69816 -1.92256 2.73023C3.9782 9.7623 3.74763 10.8752 3.74799 12c0 0.253 0.015 0.5 0.038 0.75"
		></path>
		<path
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
			d="M20.211 12.75c0.022 -0.248 0.038 -0.5 0.038 -0.75 0.0005 -1.1248 -0.23 -2.23779 -0.6772 -3.26988C19.1245 7.69802 18.4701 6.76879 17.649 6"
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
{#snippet interactionSettingsIcon()}
	<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
		<circle cx="12" cy="12" r="3" stroke="currentColor" />
		<path
			d="M13.7654 2.15224C13.3978 2 12.9319 2 12 2C11.0681 2 10.6022 2 10.2346 2.15224C9.74457 2.35523 9.35522 2.74458 9.15223 3.23463C9.05957 3.45834 9.0233 3.7185 9.00911 4.09799C8.98826 4.65568 8.70226 5.17189 8.21894 5.45093C7.73564 5.72996 7.14559 5.71954 6.65219 5.45876C6.31645 5.2813 6.07301 5.18262 5.83294 5.15102C5.30704 5.08178 4.77518 5.22429 4.35436 5.5472C4.03874 5.78938 3.80577 6.1929 3.33983 6.99993C2.87389 7.80697 2.64092 8.21048 2.58899 8.60491C2.51976 9.1308 2.66227 9.66266 2.98518 10.0835C3.13256 10.2756 3.3397 10.437 3.66119 10.639C4.1338 10.936 4.43789 11.4419 4.43786 12C4.43783 12.5581 4.13375 13.0639 3.66118 13.3608C3.33965 13.5629 3.13248 13.7244 2.98508 13.9165C2.66217 14.3373 2.51966 14.8691 2.5889 15.395C2.64082 15.7894 2.87379 16.193 3.33973 17C3.80568 17.807 4.03865 18.2106 4.35426 18.4527C4.77508 18.7756 5.30694 18.9181 5.83284 18.8489C6.07289 18.8173 6.31632 18.7186 6.65204 18.5412C7.14547 18.2804 7.73556 18.27 8.2189 18.549C8.70224 18.8281 8.98826 19.3443 9.00911 19.9021C9.02331 20.2815 9.05957 20.5417 9.15223 20.7654C9.35522 21.2554 9.74457 21.6448 10.2346 21.8478C10.6022 22 11.0681 22 12 22C12.9319 22 13.3978 22 13.7654 21.8478C14.2554 21.6448 14.6448 21.2554 14.8477 20.7654C14.9404 20.5417 14.9767 20.2815 14.9909 19.902C15.0117 19.3443 15.2977 18.8281 15.781 18.549C16.2643 18.2699 16.8544 18.2804 17.3479 18.5412C17.6836 18.7186 17.927 18.8172 18.167 18.8488C18.6929 18.9181 19.2248 18.7756 19.6456 18.4527C19.9612 18.2105 20.1942 17.807 20.6601 16.9999C21.1261 16.1929 21.3591 15.7894 21.411 15.395C21.4802 14.8691 21.3377 14.3372 21.0148 13.9164C20.8674 13.7243 20.6602 13.5628 20.3387 13.3608C19.8662 13.0639 19.5621 12.558 19.5621 11.9999C19.5621 11.4418 19.8662 10.9361 20.3387 10.6392C20.6603 10.4371 20.8675 10.2757 21.0149 10.0835C21.3378 9.66273 21.4803 9.13087 21.4111 8.60497C21.3592 8.21055 21.1262 7.80703 20.6602 7C20.1943 6.19297 19.9613 5.78945 19.6457 5.54727C19.2249 5.22436 18.693 5.08185 18.1671 5.15109C17.9271 5.18269 17.6837 5.28136 17.3479 5.4588C16.8545 5.71959 16.2644 5.73002 15.7811 5.45096C15.2977 5.17191 15.0117 4.65566 14.9909 4.09794C14.9767 3.71848 14.9404 3.45833 14.8477 3.23463C14.6448 2.74458 14.2554 2.35523 13.7654 2.15224Z"
			stroke="currentColor"
		/>
	</svg>
{/snippet}
