<script lang="ts">
	import SignInDialogHeader from './c_states_shared/SignInDialogHeader.svelte';

	let { email }: { email: string } = $props<{ email: string }>();
</script>

<SignInDialogHeader text="Check your inbox" />
<p class="msg">Confirmation link has been sent to</p>
<p class="email">{email}</p>

<style>
	.msg {
		font-size: 1.25rem;
	}
	.email {
		font-size: 1.25rem;
		color: var(--primary);
	}
</style>
