export type OrderStatus =
    | 'Created'
    | 'PaymentPending'
    | 'Paid'
    | 'PaymentFailed'
    | 'Cancelled'
    | string;

export type OrderListItem = {
    id: string;
    description: string;
    amount: number;
    status: OrderStatus;
    createdAt?: string;
};

export type CreateOrderResponse = {
    orderId: string;
    status: OrderStatus;
};

export type GetOrderStatusResponse = {
    orderId: string;
    status: OrderStatus;
};
