<template>
    <div class="loader" :style="{'top': top + 'px'}">
        <div class="loader__icon"></div>
    </div>
</template>
<script>
export default {
    props: {
        top: {
            title: Number,
            default: 0
        },
    },
    setup() {
        
    },
}
</script>
<style scoped>
    .loader{
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.2);
        z-index: 12;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
    }

    .loader__icon{
        width: 50px;
        height: 50px;
        border-radius: 50%;
        border-width: 5px;
        border-style: solid;
        border-right-color: #0072bc;
        border-left-color: rgba(0, 0, 0, 0.3);
        border-top-color: rgba(0, 0, 0, 0.3);
        border-bottom-color: rgba(0, 0, 0, 0.3);
        animation: spin 1.5s linear infinite;
    }

    @keyframes spin{
        0%{
            transform: rotate(0deg);
        }
        100%{
            transform: rotate(360deg);
        }
    }
</style>