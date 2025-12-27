<script setup lang="ts">
import { ref } from 'vue';
import { createOrder } from '../api/orders';

const props = defineProps<{ userId: number }>();
const amount = ref(100);
const description = ref('Test order');

async function submit() {
  await createOrder(props.userId, amount.value, description.value);
  alert('Order created');
}
</script>

<template>
  <div>
    <h3>Create Order</h3>
    <input v-model.number="amount" type="number" />
    <input v-model="description" />
    <button @click="submit">Create</button>
  </div>
</template>
