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

export async function api<T>(path: string, options?: RequestInit): Promise<T> {
    const res = await fetch(`${API_BASE}${path}`, {
        headers: { 'Content-Type': 'application/json' },
        ...options,
    });

    if (!res.ok) {
        const body = await res.text().catch(() => '');
        throw new ApiError(res.status, res.statusText, body);
    }

    const text = await res.text();
    return (text ? JSON.parse(text) : (undefined as any)) as T;
}
