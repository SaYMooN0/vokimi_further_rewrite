<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';

	interface Props {
		vokiTypes: VokiType[];
		sort: 'fromAToZ' | 'fromZToA' | 'fromNewToOld' | 'fromOldToNew';
	}
    let { vokiTypes=$bindable(), sort=$bindable() }: Props = $props();
</script>
