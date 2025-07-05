import { DateUtils } from "./date-utils";
import { ErrUtils, type Err } from "./err";

export type ResponseResult<T> =
    | { isSuccess: true; data: T }
    | { isSuccess: false; errs: Err[] };
export type ResponseVoidResult =
    | { isSuccess: true }
    | { isSuccess: false; errs: Err[] };

class BackendService {
    private _baseUrl: string;

    constructor(baseUrl: string) {
        this._baseUrl = baseUrl;
    }
    public async fetchJsonResponse<T>(url: string, options: RequestInit): Promise<ResponseResult<T>> {
        try {

            const response = await fetch(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });

            if (response.ok) {
                const text = await response.text();
                const data = BackendService.parseWithDates<T>(text);
                return { isSuccess: true, data };
            }
            const errs = await this.parseErrResponse(response);

            return { isSuccess: false, errs };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: [ErrUtils.createUnknown("Error: " + e.message)]
            };
        }
    }

    public async fetchVoidResponse(url: string, options: RequestInit): Promise<ResponseVoidResult> {
        try {
            const response = await fetch(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });
            if (response.ok) {
                return { isSuccess: true };
            }

            const errs = await this.parseErrResponse(response);
            return { isSuccess: false, errs };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: [ErrUtils.createUnknown("Error: " + e.message)]
            };
        }
    }

    public async serverFetchJsonResponse<T>(
        fetchFunc: typeof fetch,
        url: string,
        options: RequestInit
    ): Promise<ResponseResult<T>> {
        try {
            const response = await fetchFunc(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });

            if (response.ok) {
                const text = await response.text();
                const data = BackendService.parseWithDates<T>(text);
                return { isSuccess: true, data };

            }

            const contentType = response.headers.get("content-type");
            if (contentType?.includes("application/json")) {
                const json = await response.json();
                if (Array.isArray(json.errs)) {
                    return {
                        isSuccess: false,
                        errs: json.errs.map(ErrUtils.fromPlain)
                    };
                }
            }

            return {
                isSuccess: false,
                errs: [ErrUtils.createUnknown("Response was not valid JSON with an 'errs' array")]
            };

        } catch (e: any) {
            return {
                isSuccess: false,
                errs: [ErrUtils.createUnknown("Exception: " + e.message)]
            };
        }
    }

    static parseWithDates<T>(json: string): T {
        return JSON.parse(json, (key, value) => {
            if (typeof value === 'string' && DateUtils.isoDateRegex.test(value)) {
                return new Date(value);
            }
            return value;
        });
    }

    private async parseErrResponse(response: Response): Promise<Err[]> {
        const contentType = response.headers.get("content-type");

        if (contentType?.includes("application/json")) {
            try {
                const json = await response.json();
                if (!Array.isArray(json.errs)) {
                    return [ErrUtils.createUnknown("Expected 'errs' array in response")];
                }
                return json.errs.map(ErrUtils.fromPlain);
            } catch {
                return [ErrUtils.createUnknown("Failed to parse JSON error response")];
            }
        }

        return [ErrUtils.createUnknown("Response not in JSON format")];
    }
}
export const ApiAuth = new BackendService('/api/auth');
export const ApiVokiCreationCore = new BackendService('/api/voki-creation/core');
export const ApiVokiCreationGeneral = new BackendService('/api/voki-creation/general');
