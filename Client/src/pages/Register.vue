<template>
  <q-page padding class="fixed-center">
    <form @submit.prevent="post" padding>
      <div class="row">
        <div class="col text-center">
          <h2>Register</h2>
        </div>
      </div>
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input autofocus v-model="formData.username" :label="$t('username')" style="width:350px"/>
        </div>
      </div>
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input type="name" v-model="formData.name" :label="$t('name')" />
        </div>
      </div>

      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input type="email" v-model="formData.email" :label="$t('email')" />
        </div>
      </div>
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input v-model="formData.password" :type="isPwd ? 'password' : 'text'" :label="$t('password')">
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
      <div class="row q-my-md form-input" padding>
        <div class="col">
          <q-input type="password" v-model="password2" :label="$t('passwordAgain')" />
        </div>
      </div>
      <div class="row q-my-md form-input">
        <div class="col text-center">
          <q-btn @click="post" color="primary" :label="$t('post')"></q-btn>
        </div>
      </div>
    </form>
  </q-page>
</template>

<script>
import { ref } from 'vue'
import axios from 'axios';
export default {
  data() {
    return {
      isPwd: ref(true),
      password2:'',
      formData: {
          id: 0,
          name: '',
          email: '',
          password: '',
          username: '',
        },
    };
  },
  methods:{
    post() {
      if(this.formData.password == this.password2){
        axios.post('https://localhost:7180/Mess/register',this.formData)
        .then(console.log('???'))
        .catch(error=>{console.log(this.formData);console.log(error)});
      }
      else{
        console.log(password2 + " " + this.formData.password);
      }
    }
  }
}
</script>


<style lang="scss" scoped>

</style>
