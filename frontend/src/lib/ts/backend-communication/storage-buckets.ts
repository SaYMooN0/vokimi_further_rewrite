import { ErrUtils, type Err } from "../err";
import { StringUtils } from "../utils/string-utils";
import type { ResponseResult } from "./result-types";

class VokimiStorageBucket {
    private _baseUrl: string;
    private _bucketName: string;


    constructor(baseUrl: string, bucketName: string) {
        this._baseUrl = baseUrl;
        this._bucketName = bucketName;
    }
    public fileSrc(key: string): string {
        if (key.startsWith('/')) {
            return `${this._baseUrl}/${this._bucketName}${key}`;
        }
        return `${this._baseUrl}/${this._bucketName}/${key}`;
    }

    public fileSrcWithVersion(key: string, version: string | number | undefined = undefined): string {
        if (!version) {
            version = StringUtils.rndStr(4);
        }
        return `${this.fileSrc(key)}?v=${version}`;
    }
    public async uploadFile(key: string, file: Blob | File): Promise<ResponseResult<string>> {
        try {
            const formData = new FormData();
            formData.append('file', file);

            const response = await fetch(`${this._baseUrl}/${this._bucketName}/upload/${encodeURIComponent(key)}`, {
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
export const StorageBucketMain = new VokimiStorageBucket('/api/voki-storage', 'main');
