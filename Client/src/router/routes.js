
const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', name:'home' , component: () => import('pages/IndexPage.vue') },
      { path: 'login' , name: 'login', component: () => import('pages/Login.vue')},
      { path: 'register', name: 'register', component: () => import('pages/Register.vue')},
      { path: 'friends', name: 'friends', component: () => import('pages/Friends.vue')},
      { path: 'profile/:username',name: 'profile', component: () => import('pages/profile.vue') , props:true },
      { path: 'chats', name: 'chat' , component: () => import('pages/Chats.vue')},
      { path: 'chats/:username',name: 'chatwith', component: () => import('pages/ChatWiz.vue') , props:true }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
