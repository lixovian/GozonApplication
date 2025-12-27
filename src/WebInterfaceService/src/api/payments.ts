import { api } from './gateway';
import type { BalanceResponse } from '../types/payments';

export function addAccount(userId: number) {
    return api<void>('/payments/accounts', {
        method: 'POST',
        body: JSON.stringify({ userId }),
    });
}

export function topUpAccount(userId: number, amount: number) {
    return api<void>('/payments/accounts/top-up', {
        method: 'POST',
        body: JSON.stringify({
            userId,
            amount,
            key: crypto.randomUUID(), // если у тебя key обязателен
        }),
    });
}

export function getBalance(userId: number) {
    return api<BalanceResponse>(`/payments/accounts/balance?userId=${userId}`);
}
