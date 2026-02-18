<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiType } from '$lib/ts/voki-type';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import VokiInitializingDialogInputsState from './_c_initializing_dialog/VokiInitializingDialogInputsState.svelte';
	import VokiInitializingDialogLoadingState from './_c_initializing_dialog/VokiInitializingDialogLoadingState.svelte';
	import VokiInitializingDialogSuccessState from './_c_initializing_dialog/VokiInitializingDialogSuccessState.svelte';
	import VokiInitializingDialogExistsCheckFailedState from './_c_initializing_dialog/VokiInitializingDialogExistsCheckFailedState.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	type DialogState =
		| { state: 'input' }
		| { state: 'initLoading' }
		| { state: 'existsCheckLoading' }
		| { state: 'existsCheckFailed'; vokiId: string }
		| { state: 'initialized'; vokiId: string; vokiType: VokiType; vokiName: string };

	let dialogState = $state<DialogState>({ state: 'input' });

	let selectedVokiType = $state<VokiType>('General');
	let vokiName = $state('');
	let errs: Err[] = $state([]);

	export function open() {
		// Reset state on open
		dialogState = { state: 'input' };
		selectedVokiType = 'General';
		vokiName = '';
		errs = [];
		dialog.open();
	}

	async function onSubmitBtnClicked() {
		validateForm();
		if (errs.length > 0) {
			return;
		}
		dialogState = { state: 'initLoading' };
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			id: string;
			name: string;
			type: VokiType;
		}>('/initialize-new-voki', RJO.POST({ newVokiName: vokiName, vokiType: selectedVokiType }));
		if (response.isSuccess) {
			if (response.data.type === 'General') {
				checkIfVokiExistsInSpecificService(response.data);
			} else {
				onNewVokiInitialized(response.data);
			}
		} else {
			dialogState = { state: 'input' };
			errs = response.errs;
		}
	}

	async function checkIfVokiExistsInSpecificService(vokiData: {
		id: string;
		name: string;
		type: VokiType;
	}) {
		dialogState = { state: 'existsCheckLoading' };
		const vokiId = vokiData.id;
		const startTime = Date.now();
		const timeout = 5000;

		while (Date.now() - startTime < timeout) {
			const existsResult = await ApiVokiCreationGeneral.ensureVokiExists(vokiId);
			if (existsResult) {
				onNewVokiInitialized(vokiData);
				return;
			}
			await new Promise((resolve) => setTimeout(resolve, 500));
		}

		dialogState = { state: 'existsCheckFailed', vokiId };
	}

	function onNewVokiInitialized(newVokiData: { id: string; name: string; type: VokiType }) {
		dialogState = {
			state: 'initialized',
			vokiId: newVokiData.id,
			vokiType: newVokiData.type,
			vokiName: newVokiData.name
		};
	}

	function validateForm(): Err[] {
		errs = [];
		if (StringUtils.isNullOrWhiteSpace(vokiName)) {
			errs.push({ message: 'Voki name cannot be empty' });
		}
		if (selectedVokiType != 'General') {
			errs.push({ message: 'Unsupported voki type' });
		}
		return errs;
	}
</script>

<DialogWithCloseButton dialogId="voki-initializing-dialog" bind:this={dialog}>
	{#if dialogState.state === 'input'}
		<VokiInitializingDialogInputsState
			bind:vokiName
			bind:selectedVokiType
			{errs}
			onSubmit={onSubmitBtnClicked}
		/>
	{:else if dialogState.state === 'initLoading'}
		<VokiInitializingDialogLoadingState text="Initializing new voki" />
	{:else if dialogState.state === 'existsCheckLoading'}
		<VokiInitializingDialogLoadingState text="Verifying voki creation..." />
	{:else if dialogState.state === 'existsCheckFailed'}
		<VokiInitializingDialogExistsCheckFailedState vokiId={dialogState.vokiId} />
	{:else if dialogState.state === 'initialized'}
		<VokiInitializingDialogSuccessState
			id={dialogState.vokiId}
			type={dialogState.vokiType}
			name={dialogState.vokiName}
		/>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#voki-initializing-dialog .dialog-content) {
		width: 48rem;
		min-height: 38rem;
	}

	:global(#voki-initializing-dialog .dialog-content:has(.voki-initialized-container)) {
		width: unset;
		height: unset;
	}
</style>
