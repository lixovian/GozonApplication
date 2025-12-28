import { api } from './gateway';
import type { BalanceResponse } from '../types/payments';

type AddAccountResponse = {
    id: string;
    userId: number;
    createdAt: string;
};

type TopUpAccountResponse = {
    accountId: string;
    userId: number;
    amount: number;
    key: string;
    createdAt: string;
};

export function addAccount(userId: number) {
    return api<AddAccountResponse>('/payments/accounts', {
        method: 'POST',
        body: JSON.stringify({ userId }),
    });
}

export function topUpAccount(userId: number, amount: number) {
    return api<TopUpAccountResponse>('/payments/accounts/top-up', {
        method: 'POST',
        body: JSON.stringify({
            userId,
            amount,
            key: crypto.randomUUID(),
        }),
    });
}

export function getBalance(userId: number) {
    return api<BalanceResponse>(`/payments/balance?userId=${userId}`);
}
