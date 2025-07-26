<script lang="ts">
	const {
		questionId,
		vokiId,
		addImage
	}: { questionId: string; vokiId: string; addImage: (image: string) => void } = $props<{
		questionId: string;
		vokiId: string;
		addImage: (image: string) => void;
	}>();
</script>
