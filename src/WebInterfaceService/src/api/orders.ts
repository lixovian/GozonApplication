import { api } from './gateway';
import type { OrderListItem, CreateOrderResponse, GetOrderStatusResponse } from '../types/orders';

export function createOrder(userId: number, amount: number, description: string) {
    return api<CreateOrderResponse>('/orders', {
        method: 'POST',
        body: JSON.stringify({ userId, amount, description }),
    });
}

export function listOrders(userId: number) {
    return api<OrderListItem[]>(`/orders?userId=${userId}`);
}

export function getOrderStatus(userId: number, orderId: string) {
    return api<GetOrderStatusResponse>(`/orders/${orderId}/status?userId=${userId}`);
}
