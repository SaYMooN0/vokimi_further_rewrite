<script lang="ts">
	import type { UsersRecommendedToInvite } from '../../../types';
	interface Props {
		usersRecommendedToInvite: UsersRecommendedToInvite;
	}
	let { usersRecommendedToInvite }: Props = $props();
</script>

<div class="input-empty-state">
	<p class="title">If you want to invite specific user use input above to search for them</p>
	<p class="subtitle">Search by display name or @uniqueName</p>

	{JSON.stringify(usersRecommendedToInvite)}
</div>
