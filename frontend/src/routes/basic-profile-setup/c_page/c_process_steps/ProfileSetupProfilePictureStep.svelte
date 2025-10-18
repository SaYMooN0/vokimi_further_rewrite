<script lang="ts">
	import PicsStepPredefinedRow from './c_profile_pic_step/PicsStepPredefinedRow.svelte';
	import PicsStepPreviewWithInput from './c_profile_pic_step/PicsStepPreviewWithInput.svelte';

	interface Props {
		profilePic: string;
	}
	let { profilePic = $bindable() }: Props = $props();
	function updateCurrentPic(newPic: string) {
		if (!profilePic.startsWith('default-profile-pics/')) {
			let picsHistory = picPresets['previous'];

			const newHistory = [
				profilePic,
				...picsHistory.filter((p) => p !== profilePic && p !== newPic),
				'default-profile-pics/black.webp'
			];

			picPresets['previous'] = newHistory.slice(0, 5);

			console.log(picPresets);
		}

		profilePic = newPic;
	}

	let picPresets: Record<string, string[]> = $state({
		['previous']: [
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp'
		],
		['meme']: [
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp'
		],
		['boykisser']: [
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp'
		],
		['dasha']: [
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp',
			'default-profile-pics/black.webp'
		]
	});
</script>

<div class="profile-pic-step">
	<PicsStepPreviewWithInput setCurrent={updateCurrentPic} currentPic={profilePic} />
	<div class="rows">
		<PicsStepPredefinedRow
			rowName="Previous"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['previous']}
		/>
		<PicsStepPredefinedRow
			rowName="Meme"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['meme']}
		/>
		<PicsStepPredefinedRow
			rowName="Boykisser"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['boykisser']}
		/>
		<PicsStepPredefinedRow
			rowName="Dasha"
			onPicClick={updateCurrentPic}
			picsArr={picPresets['dasha']}
		/>
	</div>
</div>

<style>
	.profile-pic-step {
		display: grid;
		gap: 1.25rem;
	}
	.rows {
		display: grid;
		grid-template-columns: 1fr 1fr;
		grid-template-rows: 1fr 1fr;
		column-gap: 3rem;
		row-gap: 1.5rem;
		padding: 0 2rem;
		justify-items: center;
	}
</style>
