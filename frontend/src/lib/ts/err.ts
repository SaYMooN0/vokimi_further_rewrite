export type Err = {
    message: string;
    code?: number;
    details?: string;
}

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
}
