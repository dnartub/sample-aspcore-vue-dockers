import { mapActions, mapGetters, mapState } from 'vuex'
import Vacancy from "@/components/Widgets/Vacancy/Vacancy"
import { Source } from '@/models/Source';

export default {
    name: 'SourceResult',
    components: {
        Vacancy
    },
    props: [

    ],
    data: function () {
        return {
        }
    },

    mounted: function () {
        
    },

    watch: {
        // измнение источника в store -> загрузка вакансий
        currentSource: {
            handler: function (newSource: Source) {
                console.log("Источник изменен. Загрузка вакансий..."); // TODO: loading-spinner

                // загружаем вакансии в store
                if (newSource == null) {
                    this.loadVacancies() // store-action
                        .catch(e => alert(e)); // TODO: show notify or modal error
                }
                else {
                    this.loadVacancies(newSource.id) // store-action
                        .catch(e => alert(e)); // TODO: show notify or modal error
                }
            },
            deep: false
        }
    },

    methods: {
        // actions из store
        ...mapActions("app", [
            "loadVacancies"
        ]),
    },

    computed: {
        // getter-ы из store
        ...mapGetters("app", [
            "getVacancies",
            "getCurrentSource"
        ]),

        // какие вакансии отрисовываем
        vacancies() {
            return this.getVacancies(); // store-getter
        },

        // информация о текущем источнике
        currentSource() {
            return this.getCurrentSource();// store-getter
        }
    },
}