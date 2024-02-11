<template>
  Rahmet
</template>

<script>
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'ErrorNotFound',
  created(){
    window.location.href = this.$router.resolve({name:'home'}).href;
  }
})
</script>
