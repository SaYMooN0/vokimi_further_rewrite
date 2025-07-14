import { ApiVokiCreationGeneral, type IVokiCreationBackendService } from '$lib/ts/backend-communication/voki-creation-backend-service';
import type { VokiType } from '$lib/ts/voki';
import { getContext, setContext } from 'svelte';



const key = Symbol("voki-creation-page-api");

export function setVokiCreationPageApiService(vokiType: VokiType) {
	const apiService = vokiType === 'General' ? ApiVokiCreationGeneral : undefined;
	if (apiService === undefined) {
		throw new Error(`Unknown voki type: ${vokiType}`);
	}
	setContext(key, apiService);
}

export function getVokiCreationPageApiService() {
	return getContext<IVokiCreationBackendService>(key);
}