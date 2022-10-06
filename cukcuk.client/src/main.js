import { createApp } from 'vue'
import App from './App.vue'
import Enum from "./js/Enum.js";
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";

const app = createApp(App);

app.config.globalProperties.$filters = {
    formatCalculation(cal){
        let newCal;
        if(cal){
            switch (cal) {
                case Enum.Calculation.Multiplication:
                    newCal = "*";
                    break;
                case Enum.Calculation.Division:
                    newCal = "/";
                    break;
            }

            return newCal;
        }
    }
}

app.use(Toast, {
    transition: "Vue-Toastification__bounce",
    maxToasts: 20,
    newestOnTop: true,
    position: POSITION.TOP_CENTER,
});

app.mount('#app')
