<template>
    <div class="vacancy-container">
        <div v-if="vacancy != null">
            <a :href="vacancy.url" target="_blank" class="vacancy-name">{{vacancy.name}}</a>
            <div class="vacancy-salary">{{vacancy.salary}}</div>
            <a :href="vacancy.address" target="_blank" class="vacancy-company">{{vacancy.company}}</a>
            <div class="vacancy-annotation">{{vacancy.annotation}}</div>
        </div>
    </div>
	
</template>

<script lang="ts" src="./Vacancy.ts"></script>

<style lang="scss">
    @import "./Vacancy.scss";
</style>