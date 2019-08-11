/*
 *
 *  ��������� ������ ��� ����������
 *  
 *  ���������: 
 *  ������� ������ � ����������:
 *  - ������ ����� action->mutation
 *  - ������ ����� getter
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
     * ���������
     */
    sources: Source[] = [];
    /*
     * �������� �� ���������
     */
    vacancies: Vacancy[] = [];
    /*
     * ������� ��������
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
     * �������� ���������� � store
     */
    async loadSources({ commit, state }) {
        try {

            let result = await sourceApi.getSources();

            // ������������� ���� ������� ������ ts, ���������� ��� ������ �� �������
            let sources = Source.fromArray(result);

            commit('setSources', sources);

        } catch (e) {
            throw e;
        }
    },

    /**
     * �������� �������� � store
     * @param sourceId � ������ ���������
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
            
            // ������������� ���� ������� ������ ts, ���������� ��� ������ �� �������
            let vacancies = Vacancy.fromArray(result);

            commit('setVacancies', vacancies);

        } catch (e) {
            throw e;
        }
    },

    /**
     * ��������� �������� ����������
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
