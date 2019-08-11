<template>
    <div class="source-choise-container">
        <a-dropdown>
            <a-menu slot="overlay" @click="handleSourceClick">
                <a-menu-item v-for="source in sources" :key="source.id"><a-icon type="user" />{{source.url}}</a-menu-item>
            </a-menu>
            <a-button style="margin-left: 8px">
                Источники <a-icon type="down" />
            </a-button>
        </a-dropdown>
    </div>
	
</template>

<script lang="ts" src="./SourceChoise.ts"></script>

<style lang="scss">
    @import "./SourceChoise.scss";
</style>