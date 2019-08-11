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

    mounted: function () {
        // загрузка источников при мантировании компонента
        this.loadSources()
            .catch(e => console.error(e))
            // установка текущего истоника -> реактивное слежение в SourceResult.watch -> загрузка из источника
            .then(() => this.setCurrentSource(this.sources[1]));
    },

    watch: {

    },

    methods: {
        // actions из store
        ...mapActions("app", [
            "loadSources",
            "setCurrentSource"
        ]),
        // при клике на источник в списке
        handleSourceClick(e:any) {
            var source = this.sources.find((x: Source) => x.id == e.key);
            this.setCurrentSource(source); // store-action
        }
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