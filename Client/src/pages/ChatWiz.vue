<template class="fullscreen">
  <div class="">
    <div v-if="messages.length==0" style="width:100%;margin-left:50%;margin-right:50%">
      There is no message yet
    </div>
    <q-virtual-scroll
    style="max-height: 700px;width:100%"
    :items="messages"
    separator
    v-slot="{ item }"
  >
    <q-item
      dense
      :style="{ backgroundColor: item.from != username ? 'coral' : 'initial' }"
    >
    <q-item-section style="height:100px;background-color:">
      <q-item-label style="font-size:25px">From: {{item.from}}</q-item-label>
      <q-item-label style="font-size:15px" caption lines="2">{{item.text}}</q-item-label>
    </q-item-section>
    <q-item-section side top>
      <q-item-label caption>{{item.date}}</q-item-label>
    </q-item-section>
    </q-item>
  </q-virtual-scroll>
  <div class="flex" style="width:100%;margin-top:5px">
    <q-input
      v-model="messager" style="height:100px;width:60%;margin-right:1%;margin-left:15%" @submit="newMessage(messager.text)" :disabled="!isFriend"
      filled
      type="textarea"
    />
    <q-btn style="width:50px" @click="newMessage(messager.text)" color="primary"> send</q-btn>
  </div>
  </div>
</template>

<script>
import axios from 'axios'

import { defineComponent, ref } from 'vue'

export default {
  props: ['username'],
  data() {
    return {
      messager:ref(''),
      messages:ref([]),
      profile: {},
      token:'',
      isRequested:ref(false),
      isFriend:ref(false),
      isYou:ref(false)
    };
  },
  created() {
    this.fetchProfile();
//    console.log("isFriend " +this.isFriend);
  },
  methods: {
    fetchProfile() {
      axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
      axios.get('https://localhost:7180/api/User/messages/get', {
            params: { user2: this.username }
        })
        .then(response=>{
          this.messages=response.data;
        })
        .catch(error => {
          localStorage.clear();
          window.location.href=this.$router.resolve({ name: 'home'}).href;
        });

        axios.get('https://localhost:7180/api/User/friends/check',{
            params: { username1: this.username, username2: localStorage.getItem('username') }
        }).then(response=>{
          this.isFriend = response.data;
        });
      },
      newMessage(){
        if(this.messager != ''){
          axios.post('https://localhost:7180/api/User/messages/add', null, {
            params: {
                to: this.username,
                text: this.messager
            }
        })
        .then(response => {
          this.messager="";
          this.messages.push(response.data);
          console.log(response.data);
//          window.location.reload();
            console.log('Message added successfully');
        })
        .catch(error => {
            console.error('Error adding message:', error);
        });
        }
        else{
          alert("write smth");
        }
      }
  },
};
</script>
