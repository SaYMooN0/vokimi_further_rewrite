import { ApiAuth } from "../backend-communication/backend-services";
import type { Err } from "$lib/ts/err";

export namespace AuthStore {
    export type AuthState =
        | { state: "loading" }
        | { state: "error"; isAuthenticated: false; errs: Err[] }
        | { state: "authenticated"; isAuthenticated: true; userId: string }
        | { state: "unauthenticated"; isAuthenticated: false };

    const TTL_MS = 2 * 60 * 1000;

    const value = $state<AuthState>({ state: "loading" });
    let expiresAt = 0;
    let inflight: Promise<void> | null = null;


    export function Get(): AuthState {
        ensureFresh(false);
        return value;
    }

    export async function GetWithForceRefresh(): Promise<AuthState> {
        ensureFresh(true);
        if (inflight) {
            await inflight;
        }
        return value;
    }

    function ensureFresh(force: boolean): void {
        if (inflight) {
            return;
        }

        const now = Date.now();
        const isAuthedAndFresh = value.state === "authenticated" && now < expiresAt;

        if (!force && isAuthedAndFresh) {
            return;
        }

        value.state = "loading";
        inflight = fetchAndApply().finally(() => { inflight = null; });
    }

    async function fetchAndApply(): Promise<void> {
        try {
            const response = await ApiAuth.fetchJsonResponse<{ userId: string; isAuthenticated: boolean }>("/ping", { method: "POST" });

            if (response.isSuccess) {
                const { userId, isAuthenticated } = response.data;
                if (isAuthenticated && userId?.trim()) {
                    value.state = "authenticated";
                    (value as any).isAuthenticated = true;
                    (value as any).userId = userId.trim();
                    delete (value as any).errs;
                    expiresAt = Date.now() + TTL_MS;
                } else {
                    value.state = "unauthenticated";
                    (value as any).isAuthenticated = false;
                    delete (value as any).userId;
                    delete (value as any).errs;
                    expiresAt = 0;
                }
            } else {
                value.state = "error";
                (value as any).isAuthenticated = false;
                (value as any).errs = response.errs as Err[];
                delete (value as any).userId;
                expiresAt = 0;
            }
        } catch {
            value.state = "error";
            (value as any).isAuthenticated = false;
            (value as any).errs = [{ message: "Network error", code: 0 } as Err];
            delete (value as any).userId;
            expiresAt = 0;
        }
    }
}