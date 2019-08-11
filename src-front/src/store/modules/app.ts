/*
 *
 *  Хранилище данных для приложения
 *  
 *  Замечание: 
 *  Паттерн работы с хранилищем:
 *  - запись через action->mutation
 *  - чтение через getter
 * 
 */

import Vue from 'vue';
import { GetterTree, MutationTree, ActionTree } from 'vuex';
import { Source } from '@/models/Source';
import { Vacancy } from '@/models/Vacancy';
import { SourceApi } from '@/api/SourceApi';
import { VacancyApi } from '@/api/VacancyApi';

const sourceApi = new SourceApi();
const vacancyApi = new VacancyApi();

export class AppState {
    /*
     * источники
     */
    sources: Source[] = [];
    /*
     * вакансии из источника
     */
    vacancies: Vacancy[] = [];
    /*
     * текущий источник
     */
    currentSource: Source = null;
}

const getters = <GetterTree<AppState, any>>{
    getSources: (state: AppState) => (): Source[] => {
        return state.sources;
    },
    getVacancies: (state: AppState) => (): Vacancy[] => {
        return state.vacancies;
    },
    getCurrentSource: (state: AppState) => (): Source => {
        return state.currentSource;
    },
};

const mutations = <MutationTree<AppState>>{
    setSources(state: AppState, sources: Source[]) {
        Vue.set(state, "sources", sources);
    },
    setVacancies(state: AppState, vacancies: Vacancy[]) {
        Vue.set(state, "vacancies", vacancies);
    },
    setCurrentSource(state: AppState, source: Source) {
        Vue.set(state, "currentSource", source);
    },
};

const actions = <ActionTree<AppState, any>>{
    /**
     * Загрузка источников в store
     */
    async loadSources({ commit, state }) {
        try {

            let result = await sourceApi.getSources();

            // инициализация всех свойств модели ts, независимо что пришло от сервера
            let sources = Source.fromArray(result);

            commit('setSources', sources);

        } catch (e) {
            throw e;
        }
    },

    /**
     * Загрузка вакансий в store
     * @param sourceId с какого источника
     */
    async loadVacancies({ commit, state }, sourceId:string) {
        try {

            let result: any = null;

            if (sourceId == null) {
                result = await vacancyApi.getDefault();
            }
            else {
                result = await vacancyApi.getFromSource(sourceId);
            }
            
            // инициализация всех свойств модели ts, независимо что пришло от сервера
            let vacancies = Vacancy.fromArray(result);

            commit('setVacancies', vacancies);

        } catch (e) {
            throw e;
        }
    },

    /**
     * Установка текущего источнника
     * @param source
     */
    async setCurrentSource({ commit, state }, source: Source) {
        try {
            commit('setCurrentSource', source);
        } catch (e) {
            throw e;
        }
    }
};

export const app = {
    namespaced: true,
    state: new AppState(),
    getters: getters,
    mutations: mutations,
    actions: actions
};
