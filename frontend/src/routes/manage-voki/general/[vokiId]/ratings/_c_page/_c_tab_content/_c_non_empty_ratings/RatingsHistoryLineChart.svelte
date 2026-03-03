<script lang="ts">
	import type { VokiDailyRatingsSnapshot } from '../../../types';

	interface Props {
		from: Date | null;
		to: Date | null;
		snapshotsToShow: VokiDailyRatingsSnapshot[];
	}
	let { from, to, snapshotsToShow }: Props = $props();
</script>

<div>line chart</div>
