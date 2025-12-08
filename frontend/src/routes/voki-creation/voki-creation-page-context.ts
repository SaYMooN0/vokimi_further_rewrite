import { type IVokiCreationBackendService } from '$lib/ts/backend-communication/voki-creation-backend-service';
import { getContext, setContext } from 'svelte';



const key = Symbol("voki-creation-page-api");
type ContextType = {
	vokiCreationApi: IVokiCreationBackendService,
	invalidateVokiName: () => void
}
export function setVokiCreationPageContext(
	apiService: IVokiCreationBackendService,
	invalidateVokiName: () => void
) {

	setContext<ContextType>(key, {
		vokiCreationApi: apiService,
		invalidateVokiName: invalidateVokiName
	});
}

export function getVokiCreationPageContext() {
	return getContext<ContextType>(key);
}
