<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';

	interface Props {
		vokiType: string;
		vokiId: string;
		vokiName: string;
		closeDialog: () => void;
	}

	let { vokiType, vokiId, vokiName, closeDialog }: Props = $props();
	function vokiCreationLink() {
		return `/voki-creation/${StringUtils.pascalToKebab(vokiType)}/${vokiId}`;
	}
</script>

<p class="main-text">
	You now a co-author of <span class="voki-name">{vokiName}</span> Voki
</p>
<div class="buttons">
	<a class="link-to-voki" href={vokiCreationLink()} aria-label="Open Voki creation page">
		Open Voki creation page
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M9 6.65032C9 6.65032 15.9383 6.10759 16.9154 7.08463C17.8924 8.06167 17.3496 15 17.3496 15M16.5 7.5L6.5 17.5"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
		</svg>
	</a>

	<button class="" onclick={() => closeDialog()}>Close</button>
</div>
