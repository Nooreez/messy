<template class="fullscreen">
  <div v-if="!goToEdit" class="flex flex-center">
    <q-page padding >
      <h4 v-if="isYou">Your Profile </h4>
      <div v-else class="flex">
        <div style="height:50px;width:50px;border-radius:50%" :style="{ backgroundColor: isFriend ? 'green' : isRequested || isSendRequest?'orange':'blue' }"/>
        <h4>Profile Page for {{ username }}</h4>

      </div>
      <table style="max-width:350px">
        <tr>
          <td>
            Name:
          </td>
          <td>
            {{ profile.name }}
          </td>
        </tr>
        <tr>
          <td>
            Email:
          </td>
          <td>
            {{ profile.email }}
          </td>
        </tr>
        <tr v-if="isYou">
          <td>Password:</td>
          <td>
            <q-input :readonly="true" v-model="profile.password" :type="isPwd ? 'password' : 'text'" style="font-size:25px;">
              <template v-slot:append>
                <q-icon
                  :name="isPwd ? 'visibility_off' : 'visibility'"
                  class="cursor-pointer"
                  @click="isPwd = !isPwd"
                />
              </template>
            </q-input>
          </td>
        </tr>
      </table>
      <div v-if="authorized" style="">
        <div v-if="!isYou" >
          <div v-if="!isFriend">
            <div v-if="isSendRequest">
              <q-btn @click="AcceptFriend" color="positive">Accept Request</q-btn>
            </div>
            <div v-else>
              <q-btn @click="AddRequest" color="primary" v-if="!isRequested">Send Request</q-btn>
              <q-btn @click="DeleteRequest" color="negative" v-else>Delete Request</q-btn>
            </div>
          </div>
          <div v-else>
            <q-btn @click="DeleteFriend" color="negative">Delete Friend</q-btn>
          </div>
        </div>
        <div v-else>
          <q-btn @click="goToEdit=true" color="dark">Edit</q-btn>
        </div>
      </div>
    </q-page>
  </div>
  <div v-else>
    <q-btn @click="goToEdit=false">
      back
    </q-btn>
    <q-page padding class="fixed-center">
      <form @submit.prevent="post" padding>
        <div class="row">
          <div class="col text-center">
            <h2>Edit</h2>
          </div>
        </div>
        <div class="row q-my-md form-input" padding>
          <div class="col">
            <q-input type="name" v-model="profile.name" :label="$t('name')" />
          </div>
        </div>

        <div class="row q-my-md form-input" padding>
          <div class="col">
            <q-input type="email" v-model="profile.email" :label="$t('email')" />
          </div>
        </div>
        <div class="row q-my-md form-input" padding>
          <div class="col">
            <q-input v-model="profile.password" :type="isPwd ? 'password' : 'text'" :label="$t('password')">
              <template v-slot:append>
                <q-icon
                  :name="isPwd ? 'visibility_off' : 'visibility'"
                  class="cursor-pointer"
                  @click="isPwd = !isPwd"
                />
              </template>
            </q-input>
          </div>
        </div>
        <div class="row q-my-md form-input">
          <div class="col text-center">
            <q-btn @click="Edit" color="dark" :label="$t('edit')"></q-btn>
          </div>
        </div>
      </form>
    </q-page>
  </div>
</template>

<script>
import axios from 'axios'

import { defineComponent, ref } from 'vue'

export default {
  props: ['username'],
  data() {
    return {
      isPwd: ref(true),
      profile: {},
      token:'',
      authorized:ref(false),
      isRequested:ref(false),
      isSendRequest:ref(false),
      isFriend:ref(false),
      isYou:ref(false),
      goToEdit: ref(false),
    };
  },
  created() {
    this.fetchProfile();
  },
  methods: {
    fetchProfile() {
      if(localStorage.getItem('token') != null) this.authorized=true;
      axios.get('https://localhost:7180/Mess/get/'+this.username).then(response=>this.profile=response.data).catch(error=>window.location.href = this.$router.resolve({name:'home'}).href);
      if(this.authorized){
        axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
        this.isYou=(this.username==localStorage.getItem("username"));
        axios.get('https://localhost:7180/api/User/request/check', {
            params: { from: localStorage.getItem('username'),to:this.username }
        }).then(response=>this.isRequested=response.data);
        axios.get('https://localhost:7180/api/User/friends/check', {
            params: {username1:localStorage.getItem('username') ,username2: this.username }
        }).then(response=>this.isFriend=response.data);

        axios.get('https://localhost:7180/api/User/request/check', {
            params: { from: this.username,to:localStorage.getItem('username') }
        }).then(response=>{console.log(response.data);this.isSendRequest=response.data});
      }
    },
    AddRequest(){
      axios.post('https://localhost:7180/api/User/request/add', null, {
            params: { to: this.username } // Pass the 'from' parameter as a query parameter
        }).then(this.isRequested=true);
    },
    DeleteRequest() {
      axios.delete('https://localhost:7180/api/User/request/delete', {
        params: {to: this.username , from:localStorage.getItem("username") } // Pass the 'to' parameter as a query parameter
      }).then(this.isRequested=false);
    },
    Edit(){
      axios.put('https://localhost:7180/api/User/user/edit',this.profile).then(response => {window.location.reload()});
    },
    DeleteFriend(){
      axios.delete('https://localhost:7180/api/User/friends/delete', {
        params: {to:this.username , from:localStorage.getItem("username")}
      });
      window.location.reload();
    },
    AcceptFriend(){
      axios.post('https://localhost:7180/api/User/friends/add',null,{
        params: { username2: this.username }
      })
      .then(window.location.reload())
      .catch(error=> console.log(error));
    }
  },
};
</script>
<style>
  td{
    font-size:30px;
  }
</style>
