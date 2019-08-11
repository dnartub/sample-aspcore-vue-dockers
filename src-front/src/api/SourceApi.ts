import Vue from 'vue';

/**
 * Api-функции по работе с иточниками
 * */
export class SourceApi {

    /**
     * Список Источников
     */
    async getSources() {
        let result = await Vue.prototype.$http.get(`/api/Source`);
        return result;
    }
}