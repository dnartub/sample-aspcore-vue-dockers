import Vue from 'vue';
import Axios from 'axios'
import App from './App.vue';
import router from './router/index';
import store from './store/index';
import './plugins/index';
import { HttpClient } from './lib/HttpClient';


Vue.config.productionTip = false;

Vue.prototype.$http = new HttpClient(Axios);

new Vue({
	router,
	store,
	render: (h) => h(App),
}).$mount('#app');
