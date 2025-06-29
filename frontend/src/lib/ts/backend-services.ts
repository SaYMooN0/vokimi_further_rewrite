import { DateUtils } from "./date-utils";
import { ErrUtils, type Err } from "./err";

export type ResponseResult<T> =
    | { isSuccess: true; data: T }
    | { isSuccess: false; errors: Err[] };
export type ResponseVoidResult =
    | { isSuccess: true }
    | { isSuccess: false; errors: Err[] };

class BackendService {
    private _baseUrl: string;

    constructor(baseUrl: string) {
        this._baseUrl = baseUrl;
    }

    public requestJsonOptions(data: any, method = "POST"): RequestInit {
        return {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };
    }

    public async fetchJsonResponse<T>(url: string, options: RequestInit): Promise<ResponseResult<T>> {
        try {
            console.log("in try", url, options);

            const response = await fetch(this._baseUrl + url, {
                ...options,
                credentials: 'include'
            });
            console.log("-------------", response);
            if (response.ok) {
                const text = await response.text();
                const data = BackendService.parseWithDates<T>(text);
                return { isSuccess: true, data };
            }

            const errors = await this.parseErrResponse(response);
            return { isSuccess: false, errors };

        } catch (e: any) {
            console.log("catch", e);

            return {
                isSuccess: false,
                errors: [ErrUtils.createUnknown("Error: " + e.message)]
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

            const errors = await this.parseErrResponse(response);
            return { isSuccess: false, errors };

        } catch (e: any) {
            return {
                isSuccess: false,
                errors: [ErrUtils.createUnknown("Error: " + e.message)]
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
                if (Array.isArray(json.errors)) {
                    return {
                        isSuccess: false,
                        errors: json.errors.map(ErrUtils.fromPlain)
                    };
                }
            }

            return {
                isSuccess: false,
                errors: [ErrUtils.createUnknown("Response was not valid JSON with an 'errors' array")]
            };

        } catch (e: any) {
            return {
                isSuccess: false,
                errors: [ErrUtils.createUnknown("Exception: " + e.message)]
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

                if (!Array.isArray(json.errors)) {
                    return [ErrUtils.createUnknown("Expected 'errors' array in response")];
                }

                return json.errors.map(ErrUtils.fromPlain);
            } catch {
                return [ErrUtils.createUnknown("Failed to parse JSON error response")];
            }
        }

        return [ErrUtils.createUnknown("Response not in JSON format")];
    }
}
export const ApiAuth = new BackendService('/api/auth');
export const ApiVokiCore = new BackendService('/api/voki-creation/core');
export const ApiVokiGeneral = new BackendService('/api/voki-creation/general');
