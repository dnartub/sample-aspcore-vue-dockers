<template>
    <div id="app">
        <a-locale-provider :locale="ru_RU">
            <component :is="layout">
                <router-view />
            </component>
        </a-locale-provider>
    </div>

</template>

<script>
``
import MainLayout from './layouts/MainLayout.vue'

import ru_RU from 'ant-design-vue/lib/locale-provider/ru_RU';
import moment from 'moment';
import 'moment/locale/ru';

moment.locale('ru');


export default {
        data() {
            return {
                ru_RU,
            }
        },

	computed: {
		layout() {
			return (this.$route.meta.layout || 'MainLayout')
		}
	},
	components: {
		MainLayout,
	},
}
</script>


<style lang="scss">

</style>
