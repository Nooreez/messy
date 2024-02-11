<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated style="display: flex; ">
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />
        <q-toolbar-title>
          Mess
        </q-toolbar-title>
      </q-toolbar>
      <q-toolbar style="justify-content: flex-end;">
        <q-select
          :model-value="searchText"
          @keyup="searchUsers"
          filled
          dark
          use-input
          fill-input
          hide-dropdown-icon
          @input-value="setModel"
          :options="searchResults"
          @submit.prevent="searchProfile"
        >
        </q-select>
        <q-btn @click="searchProfile" style="marin-left:15px" icon="search" />
        <q-btn v-if="!authorized" style="margin:15px" @click="goToLogin">login</q-btn>
        <q-btn v-else style="margin:15px" @click="goToProfile">{{ username }}</q-btn>
        <q-btn v-if="authorized" @click="logout()">logout</q-btn>
        <q-btn v-else @click="goToRegister">register</q-btn>
      </q-toolbar>
    </q-header>
    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      bordered
    >
      <q-list>
        <q-item-label
          header
        >
        </q-item-label>

        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent, ref } from 'vue'
import EssentialLink from 'components/EssentialLink.vue'
import axios from 'axios'

const linksList = [

  {
    title: 'Chats',
    caption: '',
    icon: 'chat',
    link: '/chats'
  },
  {
    title: 'Friends',
    caption: '',
    icon: 'record_voice_over',
    link: '/friends'
  }
]

export default defineComponent({
  name: 'MainLayout',
  components: {
    EssentialLink
  },
  data(){
    return{
      exText:'',
      searchResults:ref([])
    }
  },
  setup () {
    const leftDrawerOpen = ref(false)
    const searchText=ref(null)
    return {
      searchText,
      authorized:false,
      username:ref(),
      essentialLinks: linksList,
      leftDrawerOpen,
      toggleLeftDrawer () {
        leftDrawerOpen.value = !leftDrawerOpen.value
      },
      setModel (val) {
        searchText.value = val
      }
    }
  },
  created(){
    this.isAuthorized();
  },
  methods:{
    searchUsers() {
        console.log(this.searchText);
        axios.get(`https://localhost:7180/Mess/search/${this.searchText}`).then(response => {
          this.searchResults = response.data;
          console.log(this.searchResults);
        })
        .catch(error => {
          console.error('Error searching users:', error);
        });
      },
    isAuthorized(){
      this.authorized=(localStorage.getItem("token") != null);
      if(this.authorized) this.username=localStorage.getItem("username");
    },
    goToLogin(){
      window.location.href = this.$router.resolve({name:'login'}).href;
    },
    goToRegister(z){
      window.location.href = this.$router.resolve({name:'register'}).href;
    },
    logout(){
      localStorage.clear();
      window.location.href = this.$router.resolve({name:'home'}).href;
    },
    goToProfile(){
      window.location.href=this.$router.resolve({ name: 'profile', params: {username:localStorage.getItem('username')}}).href;
    },
    searchProfile(){
      console.log(this.$router.resolve(({ name: 'profile', params: {username:this.searchText}})));
      window.location.href=this.$router.resolve(({ name: 'profile', params: {username:this.searchText}})).href;
    }
  }
})
</script>
