<script setup lang="ts">
import { ref } from 'vue';
import { listOrders } from '../api/orders';

const props = defineProps<{ userId: number }>();
const orders = ref<any[]>([]);

async function load() {
  orders.value = await listOrders(props.userId);
}
</script>

<template>
  <div>
    <h3>Orders</h3>
    <button @click="load">Load Orders</button>
    <ul>
      <li v-for="o in orders" :key="o.id">
        {{ o.description }} — {{ o.status }}
      </li>
    </ul>
  </div>
</template>
