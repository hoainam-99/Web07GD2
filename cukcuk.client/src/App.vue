<template>
  <TheHeader />
  <div class="container">
    <TheNavbar />
    <TheContent />
  </div>
</template>

<script>
import TheHeader from "./components/views/header/TheHeader.vue";
import TheNavbar from "./components/views/navbar/TheNavbar.vue";
import TheContent from "./components/views/content/TheContent.vue";

export default {
  name: "App",
  components: {
    TheHeader,
    TheNavbar,
    TheContent,
  },
};
</script>

<style>
  @import url(./css/main.css);
@font-face {
  font-family: TahomaRegular;
  src: url(./assets/TahomaFont/Tahoma\ Regular\ font.ttf);
} 

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: TahomaRegular;
  font-size: 14px;
}

#app {
  min-width: 1024px;
  min-height: 100vh;
}

.container{
  display: flex;
}
</style>
