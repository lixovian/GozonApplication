<script setup lang="ts">
import { ref } from 'vue';
import TopNav, { type TabKey } from './components/TopNav.vue';
import ToastHost from './components/ToastHost.vue';

import HomePage from './pages/HomePage.vue';
import CreateOrderPage from './pages/CreateOrderPage.vue';
import OrdersListPage from './pages/OrdersListPage.vue';
import BalancePage from './pages/BalancePage.vue';
import OrderStatusPage from './pages/OrderStatusPage.vue';
import UserPage from './pages/UserPage.vue';

const tab = ref<TabKey>('home');
const userId = ref<number>(1);

const toastRef = ref<InstanceType<typeof ToastHost> | null>(null);
function toast() {
  return toastRef.value;
}
</script>

<template>
  <TopNav v-model="tab" :userId="userId" />
  <ToastHost ref="toastRef" />

  <div class="container">
    <HomePage v-if="tab === 'home'" :userId="userId" />

    <CreateOrderPage v-else-if="tab === 'create'" :userId="userId" :toast="toast()" />
    <OrdersListPage v-else-if="tab === 'list'" :userId="userId" :toast="toast()" />
    <BalancePage v-else-if="tab === 'balance'" :userId="userId" :toast="toast()" />
    <OrderStatusPage v-else-if="tab === 'status'" :userId="userId" :toast="toast()" />

    <UserPage v-else-if="tab === 'user'" :userId="userId" @update:userId="userId = $event" />
  </div>
</template>
