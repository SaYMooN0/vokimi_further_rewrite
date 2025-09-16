import { ApiAuth } from "../backend-communication/backend-services";
import { StringUtils } from "../utils/string-utils";


const EMPTY_DATE = new Date(1970, 0, 1);
class AuthStore {
    private _userId: string | null = null;
    private _lastFetched: Date;
    constructor(userId: string | null, lastFetched: Date) {
        this._userId = userId;
        this._lastFetched = lastFetched;
    }

    get userId(): string | null {
        return this._userId;
    }

    get lastFetched(): Date {
        return this._lastFetched;
    }
    isAuthenticated: boolean = $derived(!StringUtils.isNullOrWhiteSpace(this._userId));


    update(userId: string | null): void {
        this._userId = userId;
        this._lastFetched = new Date();
    }
    setEmpty(): void {
        this._userId = null;
        this._lastFetched = EMPTY_DATE;
    }

}
async function pingUser(): Promise<{ userId: string } | null> {
    const response = await ApiAuth.fetchJsonResponse<{ userId: string }>(
        "/ping", { method: "POST" }
    );
    if (response.isSuccess) {
        return response.data;
    } else {
        return null;
    }
}
const authStore = $state(new AuthStore(null, EMPTY_DATE));
let ongoingRefresh: Promise<AuthStore> | null = null;

export async function getAuthStore(): Promise<AuthStore> {
    const lastFetched = authStore?.lastFetched ?? EMPTY_DATE;

    const now = new Date();
    const twoMinutes = 2 * 60 * 1000;
    const expired = now.getTime() - lastFetched.getTime() > twoMinutes;

    if (expired) {
        if (ongoingRefresh) {
            return await ongoingRefresh;
        }
        ongoingRefresh = forceGetAuthStore().finally(() => {
            ongoingRefresh = null;
        });
        return await ongoingRefresh;
    }
    return authStore;
}
export async function forceGetAuthStore(): Promise<AuthStore> {
    const pingedUserData = await pingUser();
    if (pingedUserData !== null) {
        authStore.update(pingedUserData.userId);
    }
    else {
        authStore.setEmpty();
    }
    return authStore;
}