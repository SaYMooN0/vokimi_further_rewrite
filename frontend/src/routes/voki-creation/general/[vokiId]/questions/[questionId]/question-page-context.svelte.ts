

import { getContext, setContext } from 'svelte';
import type { ResultIdWithName } from './types';

const key = "general-voki-creation-question-page-context";

export function setQuestionPageContext(results: ResultIdWithName[]) {
    // eslint-disable-next-line prefer-const
    let ctx = $state<ContextType>({ results });
    setContext(key, ctx);
}

export function getQuestionPageContext(): ContextType {
    return getContext(key) as ContextType;
}
type ContextType = {
    results: ResultIdWithName[],
}