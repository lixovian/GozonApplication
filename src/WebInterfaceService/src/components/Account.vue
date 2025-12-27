<script setup lang="ts">
import { createAccount } from '../api/payments';

const props = defineProps<{ userId: number }>();

async function onCreate() {
  await createAccount(props.userId);
  alert('Account created');
}
</script>

<template>
  <button @click="onCreate">Create Account</button>
</template>
