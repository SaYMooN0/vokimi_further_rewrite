import type { ResponseResult } from "./backend-services";
import { ErrUtils, type Err } from "./err";
import { StringUtils } from "./utils/string-utils";

class VokimiStorageService {
    private _baseUrl: string;

    constructor(baseUrl: string) {
        this._baseUrl = baseUrl;
    }
    public fileSrc(key: string): string {
        return `${this._baseUrl}/main/files/${key}`;
    }

    public fileSrcWithVersion(key: string): string {
        return `${this.fileSrc(key)}?v=${StringUtils.rndStr(4)}`;
    }
    public async uploadFile(key: string, file: Blob | File): Promise<ResponseResult<string>> {
        try {
            const formData = new FormData();
            formData.append('file', file);

            const response = await fetch(`${this._baseUrl}/main/upload/${encodeURIComponent(key)}`, {
                method: 'PUT',
                body: formData
            });


            if (response.ok) {
                const data = await response.json();
                return { isSuccess: true, data: data.fileKey };

            }

            const errs = await this.parseErrResponse(response);
            return { isSuccess: false, errs };
        }
        catch (e: any) {
            return {
                isSuccess: false,
                errs: [ErrUtils.createUnknown("Error: " + e.message)]
            };
        }
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
export const ApiVokimiStorage = new VokimiStorageService('/api/voki-storage');
