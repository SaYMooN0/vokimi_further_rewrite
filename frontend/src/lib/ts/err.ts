export type Err = {
    message: string;
    code?: number;
    details?: string;
}
export type ErrType =
    | 'Unspecified'
    | 'NoAccess'
    | 'AuthRequired'
    | 'NotImplemented'
    | 'NotFound'
    | 'Other';

export class ErrUtils {
    static UnspecifiedErrCode = 0;

    static hasNonEmptyDetails(err: Err): boolean {
        return !!err.details && err.details.trim().length > 0;
    }

    static hasSpecifiedCode(err: Err): boolean {
        return err.code != null && err.code !== ErrUtils.UnspecifiedErrCode;
    }

    static hasSomethingExceptMessage(err: Err): boolean {
        return (
            ErrUtils.hasNonEmptyDetails(err) ||
            ErrUtils.hasSpecifiedCode(err)
        );
    }
    static createUnknown(details?: string): Err {
        return {
            message: "Unknown error",
            code: ErrUtils.UnspecifiedErrCode,
            details
        };
    }
    static fromPlain(obj: any): Err {
        return {
            message: String(obj.message ?? "Unknown error"),
            code: typeof obj.code === 'number' ? obj.code : undefined,
            details: typeof obj.details === 'string' ? obj.details : undefined
        };
    }
    static getErrTypeByCode(e: Err): ErrType {
        if (!e.code || e.code === 0) {
            return 'Unspecified';
        }
        const code = e.code;
        if (code === 1) {
            return 'NotImplemented';
        }
        if (code >= 23000 && code < 24000) {
            return 'NotFound';
        }
        if (code === 31000) {
            return 'NoAccess';
        }
        if (code === 32000) {
            return 'AuthRequired';
        }
        return 'Other';
    }
}
