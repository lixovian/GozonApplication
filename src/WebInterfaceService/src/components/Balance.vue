<script setup lang="ts">
import { ref } from 'vue';
import { getBalance, topUp } from '../api/payments';

const props = defineProps<{ userId: number }>();
const balance = ref<number | null>(null);

async function load() {
  const res = await getBalance(props.userId);
  balance.value = res.balance;
}

async function add() {
  await topUp(props.userId, 100);
  await load();
}
</script>

<template>
  <div>
    <button @click="load">Load Balance</button>
    <button @click="add">+100</button>
    <div v-if="balance !== null">Balance: {{ balance }}</div>
  </div>
</template>
