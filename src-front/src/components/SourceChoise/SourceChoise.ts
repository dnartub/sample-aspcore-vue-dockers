import { mapActions, mapGetters, mapState } from 'vuex'
import { Source } from '@/models/Source';

export default {
    name: 'SourceChoise',
    components: {
    },
    props: [
    ],
    data: function () {
        return {
           
        }
    },

    async created() {
        // загрузка источников при мантировании компонента
        try {
            await this.loadSources();
            // установка текущего истоника -> реактивное слежение в SourceResult.watch -> загрузка из источника
            this.setCurrentSource(this.sources[1]);
        } catch (e) {
            console.log(e);
        }
    },

    watch: {

    },

    methods: {
        // actions из store
        ...mapActions("app", [
            "loadSources",
            "loadVacancies",
            "setCurrentSource"
        ]),
        // при клике на источник в списке
        handleSourceClick(e:any) {
            var source = this.sources.find((x: Source) => x.id == e.key);
            this.setCurrentSource(source); // store-action
        },
    },

    computed: {
        // getter-ы из сторы
        ...mapGetters("app", [
            "getSources"
        ]),

        // какие источники ппоказываем в списке
        sources() {
            return this.getSources(); // store-getter
        }

    },
}