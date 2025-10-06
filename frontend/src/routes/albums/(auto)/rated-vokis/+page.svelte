<script lang="ts">
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
	function vokisWithLastTakenDate() {
		if (data.response.isSuccess === false) {
			return [];
		}

		return Object.entries(data.response.data.vokiIdWithRatingDate).sort(
			(a, b) => b[1].getTime() - a[1].getTime()
		);
	}
</script>

{#if data.response.isSuccess}
	<div class="albums-container">
		{#each vokisWithLastTakenDate() as voki}
			<div>
				{voki[0]}
			</div>
		{/each}
	</div>
{/if}

