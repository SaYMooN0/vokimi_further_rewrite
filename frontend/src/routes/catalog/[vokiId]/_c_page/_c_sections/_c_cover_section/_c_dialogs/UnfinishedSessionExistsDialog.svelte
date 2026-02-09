<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ContinueVokiTakingSessionMarkerCookie } from '$lib/ts/cookies/continue-voki-taking-session-marker';
	import type { ExistingUnfinishedSessionForVokiData } from '$lib/ts/voki-taking-session';
	import { relativeTime } from 'svelte-relative-time';
	import { toast } from 'svelte-sonner';
	interface Props {
		takeVokiPageLink: string;
	}
	let { takeVokiPageLink }: Props = $props();
	export function open(data: ExistingUnfinishedSessionForVokiData) {
		sessionData = data;
		dialog.open();
	}
	let dialog: DialogWithCloseButton = $state()!;
	let sessionData = $state<ExistingUnfinishedSessionForVokiData>();
	function onContinueBtnClick() {
		if (sessionData) {
			ContinueVokiTakingSessionMarkerCookie.markFor2Min(sessionData.vokiId, sessionData.sessionId);
			goto(`${takeVokiPageLink}?continueExistingUnfinishedSession=true`);
		} else {
			toast.error('Session data not found. Please reload the page');
		}
	}
	function onTerminateBtnClick() {
		goto(`${takeVokiPageLink}?terminateSession=true`);
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="unfinished-session-exists-dialog">
	<p class="title">You have an unfinished session for this Voki</p>
	{#if sessionData}
		<div class="info">
			<label>
				<label>
					Session started:
					<span use:relativeTime={{ date: sessionData.startedAt }} />
				</label>
				Questions answered: {sessionData.questionsWithSavedAnswersCount} / {sessionData.totalQuestionsCount}
			</label>
		</div>
	{/if}
	<span class="note"
		>You can continue where you left off or terminate the current session and start a new one</span
	>
	<button class="continue-btn" onclick={onContinueBtnClick}>Continue</button>
	<button class="terminate-btn" onclick={onTerminateBtnClick}>Terminate old and start new</button>
</DialogWithCloseButton>

<style>
	:global(#unfinished-session-exists-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.25rem;
		padding: 2rem 4rem;
		color: var(--text);
	}

	.title {
		margin: 0;
		color: var(--primary);
		font-size: 1.5rem;
		font-weight: 550;
		text-align: center;
	}
</style>
