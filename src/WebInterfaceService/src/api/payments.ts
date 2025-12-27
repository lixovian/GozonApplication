import { api } from './gateway';

export function createAccount(userId: number) {
    return api('/payments/accounts', {
        method: 'POST',
        body: JSON.stringify({ userId }),
    });
}

export function topUp(userId: number, amount: number) {
    return api('/payments/accounts/top-up', {
        method: 'POST',
        body: JSON.stringify({
            userId,
            amount,
            key: crypto.randomUUID(),
        }),
    });
}

export function getBalance(userId: number) {
    return api(`/payments/accounts/balance?userId=${userId}`);
}
