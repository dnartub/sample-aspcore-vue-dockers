import Vue from "vue";
import store from '@/store/index'
import Router from "vue-router";
import MainPage from "@/views/MainPage/MainPage.vue";


Vue.use(Router);

const routes = [
	{
		path: '/',
		name:'/',
		redirect: { path: '/main' }
    },
	{
		path: "/main",
		name: "main",
		component: MainPage
	},
];

const router = new Router({
	mode: "history",
	routes
});

export default router;
