<template>
  <q-page padding class="fixed-center">
    <form @submit.prevent="login">
      <div class="row">
        <div class="col text-center">
          <h2>Login</h2>
        </div>
      </div>
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input autofocus v-model="formData.username" :label="$t('username')" style="width:350px"/>
        </div>
      </div>
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input type="password" maxlength v-model="formData.password" :label="$t('password')" />
        </div>
      </div>
      <div class="row q-my-md form-input">
        <div class="col text-center">
          <q-btn @click="login" color="primary" :label="$t('Login')"></q-btn>
        </div>
      </div>
      <div>
        {{ inputError }}
      </div>
    </form>
  </q-page>
</template>

<script>
import axios from 'axios'
export default {
  data() {
    return {
      inputError:'',
      formData: {
          username: '',
          password: ''
        }
    };
  },
  created(){
    this.isAuthorized();
  },
  methods:{
    isAuthorized(){
      if(localStorage.getItem("token") != null){
        window.location.href=this.$router.resolve({name:'home'}).href;
      }
//      this.inputError.color="red";
    },
    login() {
      this.$axios.post('https://localhost:7180/Mess/login', this.formData).then(response=>{
        console.log(response.data);
        localStorage.setItem('token', response.data);
        localStorage.setItem('username', this.formData.username);
        window.location.href = this.$router.resolve({name:'home'}).href;
    }).catch(
        this.inputError="Your login data is incorrect"
//      window.location.reload()
      );
//      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    },
  }
}
</script>


<style lang="scss" scoped>

</style>
