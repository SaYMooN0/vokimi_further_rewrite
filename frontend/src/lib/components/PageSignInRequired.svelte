<script lang="ts">
	import { getSignInDialogOpenFunction } from '../../routes/c_layout/ts_layout_contexts/sign-in-dialog-context';
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

<div>
	Page sign in required
	<div class="btns-container">
		<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
		<button class="signup-btn" onclick={() => openSignInDialog('signup')}>Sign up</button>
	</div>
</div>
