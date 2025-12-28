const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:9000';

export class ApiError extends Error {
    status: number;
    body?: string;

    constructor(status: number, message: string, body?: string) {
        super(message);
        this.status = status;
        this.body = body;
    }
}

export async function api<T>(path: string, options: RequestInit = {}): Promise<T> {
    const headers = new Headers(options.headers);

    // content-type ставим только если есть body (GET/HEAD не надо)
    if (options.body && !headers.has('Content-Type')) {
        headers.set('Content-Type', 'application/json');
    }

    const res = await fetch(`${API_BASE}${path}`, {
        ...options,
        headers,
    });

    if (!res.ok) {
        const body = await res.text().catch(() => '');
        throw new ApiError(res.status, res.statusText, body);
    }

    // если ответа нет — вернём undefined
    const text = await res.text().catch(() => '');
    return (text ? JSON.parse(text) : (undefined as any)) as T;
}
