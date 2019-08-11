import Vue from 'vue';

/**
 * Api-функции по работе с вакансиями
 * */
export class VacancyApi {

    /**
     * »з источника по умолчанию
     */
    async getDefault() {
        let result = await Vue.prototype.$http.get(`/api/Vacancy`);
        return result;
    }

    /**
      * »з источника
    */
    async getFromSource(sourceId:string) {
        let result = await Vue.prototype.$http.get(`/api/Vacancy/${sourceId}`);
        return result;
    }

}