export class DateUtils {
    static toLocale(date: Date): string {
        const locale = navigator.language || 'en-US';
    
        const timePart = date.toLocaleTimeString(locale, {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
        });
    
        const datePart = date.toLocaleDateString(locale, {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
        });
    
        return `${timePart} ${datePart}`;
    }
    
    static readonly isoDateRegex: RegExp = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+-]\d{2}:\d{2})?$/;
}