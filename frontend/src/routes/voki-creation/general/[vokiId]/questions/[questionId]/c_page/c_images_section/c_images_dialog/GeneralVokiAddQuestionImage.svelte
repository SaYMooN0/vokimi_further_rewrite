<script lang="ts">
		const {
		questionId,
		vokiId,
		uploadImage
	}: { questionId: string; vokiId: string; uploadImage: (image: File) => Promise<void> } = $props<{
		questionId: string;
		vokiId: string;
		uploadImage: (image: File) => Promise<void>
	}>();
</script>
