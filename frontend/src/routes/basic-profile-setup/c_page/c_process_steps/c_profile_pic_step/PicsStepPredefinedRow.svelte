<script lang="ts">
	type PicData = { src: string; value: string };
	type Props = {
		picsArr: PicData[];
		rowName: string;
		onPicClick: (pic: PicData) => void;
	};
	let { picsArr, rowName }: Props = $props();
</script>

<div class="predefined-row">
	<div class="row-name">{rowName}</div>
	<div class="pics-container">
		{#each picsArr as pic}
			<div>{pic.value} {pic.value}</div>
		{/each}
	</div>
</div>
