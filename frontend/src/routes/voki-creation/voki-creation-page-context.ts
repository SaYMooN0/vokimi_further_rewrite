import { type IVokiCreationBackendService } from '$lib/ts/backend-communication/voki-creation-backend-service';
import { getContext, setContext } from 'svelte';



const key = Symbol("voki-creation-page-api");
type ContextType = {
	vokiCreationApi: IVokiCreationBackendService,
	headerVokiName: {
		value: string | undefined,
		invalidate: () => void
	}
}
export function setVokiCreationPageContext(
	apiService: IVokiCreationBackendService,
	headerVokiName: {
		value: string | undefined,
		invalidate: () => void
	}
) {

	setContext<ContextType>(key, {
		vokiCreationApi: apiService,
		headerVokiName: headerVokiName
	});
}

export function getVokiCreationPageContext() {
	return getContext<ContextType>(key);
}
