export const RequestJsonOptions = {
    POST: (data: any): RequestInit => create(data, "POST"),
    PUT:  (data: any): RequestInit => create(data, "PUT"),
    PATCH: (data: any): RequestInit => create(data, "PATCH"),
    DELETE: (data: any): RequestInit => create(data, "DELETE"),
};

function create(data: any, method: string): RequestInit {
    return {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    };
}