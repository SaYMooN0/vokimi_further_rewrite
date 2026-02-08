import type { Cookies } from '@sveltejs/kit';
import { CookieUtils } from '../utils/cookie-utils';

function cookieName(vokiId: string) {
    return `voki-continue-session-${vokiId}`;
}

export namespace ContinueVokiTakingSessionMarkerCookie {

    export function markFor2Min(vokiId: string, sessionId: string) {
        CookieUtils.setCookie(cookieName(vokiId), sessionId, {
            seconds: 60 * 2
        });
    }

    export function clear(vokiId: string) {
        CookieUtils.deleteCookie(cookieName(vokiId));
    }

    export function get(cookies: Cookies, vokiId: string): string | null {
        return cookies.get(cookieName(vokiId)) ?? null;
    }
}
