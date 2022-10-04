import { createApp } from 'vue'
import App from './App.vue'
import Enum from "./js/Enum.js";

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

app.mount('#app')
