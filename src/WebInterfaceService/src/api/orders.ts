import { api } from './gateway';

export function createOrder(userId: number, amount: number, description: string) {
    return api('/orders', {
        method: 'POST',
        body: JSON.stringify({ userId, amount, description }),
    });
}

export function listOrders(userId: number) {
    return api(`/orders?userId=${userId}`);
}

export function getOrderStatus(userId: number, orderId: string) {
    return api(`/orders/${orderId}/status?userId=${userId}`);
}
