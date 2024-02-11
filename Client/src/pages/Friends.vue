<template>
  <div>
    <div class="">
      <q-btn class="q-my-md" style="width:49%;margin-right:5px;height:100px" @click="forFriends=true">Friends</q-btn>
      <q-btn class="q-my-md" style="width:49%;height:100px" @click="forFriends=false">Friends Requests</q-btn>
    </div>
  </div>
  <div class="q-pa-md q-my-md" style="height:500px;">
    <div class="row">
      <div class="flex" style="width:100%" >
        <div class="row" style="width:100%" >
          <div v-if="forFriends" style="width:100%">
            <div v-if="friendsList.length === 0">
              <p>No friends</p>
            </div>
            <div v-else style="width:100%" >
              <p>{{ friendsList.length }} friends</p>
              <q-virtual-scroll
              style="max-height:100%;width:100%"
              :items="friendsList"
              separator
              v-slot="{ item }"
            >
              <q-item
                dense
                style="height:100px;"
              >
                <q-item-section style="margin-left:25%;margin-right:25%">
                  <q-item-label>

                    <router-link :to="{ name: 'profile', params: { username: item } }">
                      {{ item }}
                    </router-link>
                    <div style="margin-left:100%;" class="flex">
                      <q-btn style="width:60px" color="primary" @click="goToChat(item)">Chat</q-btn>
                      <q-btn style="width:60px" color="negative" @click="deleteFriend(item)">Delete</q-btn>
                    </div>
                  </q-item-label>
                </q-item-section>
              </q-item>
            </q-virtual-scroll>
            </div>
          </div>
          <div v-else style="width:100%">
            <div v-if="requestGetList.length + requestSendList.length === 0" style="width:100%">
              <p>No requests</p>
            </div>
            <div v-else style="width:100%" >
              <p>{{ requestGetList.length }} requests get</p>
              <q-virtual-scroll
              style="max-height:100%;width:100%"
              :items="requestGetList"
              separator
              v-slot="{ item }"
            >
              <q-item
                dense
                style="height:100px;width:100%"
              >
                <q-item-section style="margin-left:25%;margin-right:25%;width:100%">
                  <q-item-label>
                    <router-link :to="{ name: 'profile', params: { username: item.from } }">
                      {{ item.from }}
                    </router-link>
                    <div style="justify-content: flex-end">
                      <q-btn style="width:100px" color="positive" @click="acceptFriendRequest(item.from)">Accept</q-btn>
                      <q-btn style="width:100px" color="negative" @click="declineFriendRequest(item.from)">Decline</q-btn>
                    </div>
                  </q-item-label>
                </q-item-section>
              </q-item>
            </q-virtual-scroll>
            <p>{{ requestSendList.length }} requests send</p>
              <q-virtual-scroll
              style="max-height:100%;width:100%"
              :items="requestSendList"
              separator
              v-slot="{ item }"
            >
              <q-item
                dense
                style="height:100px;width:100%"
              >
                <q-item-section style="margin-left:25%;margin-right:25%;width:100%">
                  <q-item-label>
                    <router-link :to="{ name: 'profile', params: { username: item.to } }">
                      {{ item.to }}
                    </router-link>
                    <div style="justify-content: flex-end;" class="flex">
                      <q-btn style="width:100px" color="negative" @click="declineFriendRequest(item.to)">Cancel</q-btn>
                    </div>
                  </q-item-label>
                </q-item-section>
              </q-item>
            </q-virtual-scroll>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import axios from 'axios';
import {ref} from 'vue';

export default {
  setup () {
    return {
      friendsList:ref([]),
      requestSendList:ref([]),
      requestGetList:ref([]),
      forFriends:ref(true)
    }
  },
  created(){
    this.isAuthorized();
    this.fetchData();
  },
  methods:{
    isAuthorized(){
      if(localStorage.getItem("token") == null){
        window.location.href = this.$router.resolve({name:'login'}).href;
      }
    },
    fetchData(){
      var token = localStorage.getItem("token");
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      axios.get('https://localhost:7180/api/User/friends').then(response=>{this.friendsList=response.data;console.log(response.data)}).catch(error=>{localStorage.clear();goToLogin()});
      axios.get('https://localhost:7180/api/User/requests/get',{params:{username:localStorage.getItem('username')}}).then(response=>{this.requestGetList=response.data;console.log(response.data)}).catch(error=>{localStorage.clear();goToLogin()});
      axios.get('https://localhost:7180/api/User/requests/send',{params:{username:localStorage.getItem('username')}}).then(response=>{this.requestSendList=response.data;console.log(response.data)}).catch(error=>{localStorage.clear();goToLogin()});

    },
    goToLogin(){
      window.location.href = this.$router.resolve({name:'login'}).href;
    },
    acceptFriendRequest(from) {
      axios.post('https://localhost:7180/api/User/friends/add',null,{
        params: { username2: from }
      }).catch(error=> console.log(error));
      window.location.reload();
    },
    declineFriendRequest(from) {
      axios.delete('https://localhost:7180/api/User/request/delete', {
        params: {to: localStorage.getItem("username") , from:from } // Pass the 'to' parameter as a query parameter
      });
      window.location.reload();
    },
    deleteFriend(from){
      axios.delete('https://localhost:7180/api/User/friends/delete', {
        params: {to: localStorage.getItem("username") , from:from } // Pass the 'to' parameter as a query parameter
      });
      window.location.reload();
    },
    goToChat(friend){
      window.location.href=this.$router.resolve({ name: 'chatwith', params: {username:friend}}).href;
    }
  }
}
</script>

<style lang="scss" scoped>

</style>
