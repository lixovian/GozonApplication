import { api } from './gateway';
import type { OrderListItem, GetOrderStatusResponse, CreateOrderResponse } from '../types/orders';

type ListOrdersResponse = {
    orders: OrderListItem[];
};

export function createOrder(userId: number, amount: number, description: string) {
    // Gateway POST /orders -> возвращает объект заказа (Id/UserId/Amount/Description/Status)
    return api<CreateOrderResponse>('/orders', {
        method: 'POST',
        body: JSON.stringify({ userId, amount, description }),
    });
}

export async function listOrders(userId: number) {
    // Gateway GET /orders?userId=... -> { orders: [...] }
    const res = await api<ListOrdersResponse>(`/orders?userId=${userId}`);
    return res.orders;
}

export function getOrderStatus(userId: number, orderId: string) {
    return api<GetOrderStatusResponse>(`/orders/${orderId}/status?userId=${userId}`);
}
