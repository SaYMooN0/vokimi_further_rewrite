<script lang="ts">
	interface Props {
		albumName: string;
		vokiCount: number;
	}
	let { albumName, vokiCount }: Props = $props();
</script>

<h1>
	<a href="/albums" class="back-link">Back</a>
	Your {albumName} Vokis ({vokiCount})
</h1>

<style>
	.back-link{

	}
</style>