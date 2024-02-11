<template>

  <div>
    <ul>
      <li v-for="friends in chats" style="height:100px">
        <q-btn style="margin:100px;" class="q-my-md q-pa-md" @click="goTo(friends)" >
          <p v-if="friends.username1 != username"> {{ friends.username1 }}</p>
          <p v-else>{{ friends.username2 }}</p>
          <div style="justify-content: flex-end" v-if="friends.lastMessage != '0001-01-01T00:00:00+00:00'">{{ friends.lastMessage }}</div>
        </q-btn>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios'

import { defineComponent, ref } from 'vue'
import { DatetimeFormat } from 'vue-i18n';

export default {
  data() {
    return {
      username:'',
      token:'',
      chats:[],
      authorized:ref(false)
    };
  },
  created() {
    this.fetchProfile();
  },
  methods: {
    fetchProfile() {
      if(localStorage.getItem('token') != null) this.authorized=true;
      else
        window.location.href="/login";
      this.username = localStorage.getItem('username');
      this.token = localStorage.getItem('token');
      axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
      axios.get('https://localhost:7180/api/User/chats').then(response=>{this.chats=response.data});
    },
    goTo(model){
      if(model.username1 != this.username) window.location.href=this.$router.resolve({ name: 'chatwith', params: {username:model.username1}}).href;
      else
        window.location.href=this.$router.resolve({ name: 'chatwith', params: {username:model.username2}}).href;
    }
  }
};
</script>
