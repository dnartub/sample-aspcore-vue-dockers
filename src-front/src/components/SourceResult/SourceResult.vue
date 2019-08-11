<template>
    <div class="source-result-container">
        <div v-if="currentSource != null" class="source-link">
            <span>источник:</span> 
            <a :href="currentSource.url">{{currentSource.url}}</a>
        </div>
            
        <div class="vacancy-list">
            <div class="vacancy-item"  v-for="vacancy in vacancies">
                <Vacancy :vacancy="vacancy"/>
            </div>
        </div>
    </div>
	
</template>

<script lang="ts" src="./SourceResult.ts"></script>

<style lang="scss">
    @import "./SourceResult.scss";
</style>