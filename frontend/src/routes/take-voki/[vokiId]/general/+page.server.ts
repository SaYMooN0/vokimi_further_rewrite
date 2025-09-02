import { ApiVokiTakingGeneral } from "$lib/ts/backend-communication/backend-services";
import { redirect, type ServerLoad } from "@sveltejs/kit";
import type { GeneralVokiTakingData } from "./types";
import { RequestJsonOptions } from "$lib/ts/request-json-options";

export const load: ServerLoad = async ({ cookies, params, fetch }) => {
    const isVokiStarted = cookies.get(`${params.vokiId}-started`) === 'true';
    if (!isVokiStarted) {
        console.log('redirecting to catalog, cookie:', cookies.get(`${params.vokiId}-started`));
        throw redirect(301, `/catalog/${params.vokiId}`);
    }

    return {
        response: await ApiVokiTakingGeneral.serverFetchJsonResponse<GeneralVokiTakingData>(
            fetch, `/vokis/${params.vokiId}/start-taking`, RequestJsonOptions.POST({})
        ),
        vokiId: params.vokiId,
        vokiTypeName: "General"
    }
}
