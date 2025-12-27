<script setup lang="ts">
import { ref } from 'vue';
import UserSelector from './components/UserSelector.vue';
import Account from './components/Account.vue';
import Balance from './components/Balance.vue';
import CreateOrder from './components/CreateOrder.vue';
import Orders from './components/Orders.vue';

const userId = ref(1);
</script>

<template>
  <div style="padding: 24px">
    <h1>Web Interface</h1>

    <UserSelector v-model="userId" />

    <Account :userId="userId" />
    <Balance :userId="userId" />
    <CreateOrder :userId="userId" />
    <Orders :userId="userId" />
  </div>
</template>
