import { createApp } from 'vue';
import App from './App.vue';
import Enum from "./js/Enum.js";
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";
import VueNumberFormat from 'vue-number-format'

const app = createApp(App);

app.config.globalProperties.$filters = {
    formatCalculation(cal) {
        let newCal;
        if (cal) {
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

app.directive("clickoutside", {
    mounted(el, binding, vnode, prevVnode) {
        el.clickOutsideEvent = (event) => {
            // Nếu element hiện tại không phải là element đang click vào
            // Hoặc element đang click vào không phải là button trong combobox hiện tại thì ẩn đi.
            if (
                !(
                    (
                        el === event.target || // click phạm vi của combobox__data
                        el.contains(event.target) || //click vào element con của combobox__data
                        el.previousElementSibling.contains(event.target)
                    ) //click vào element button trước combobox data (nhấn vào button thì hiển thị)
                )
            ) {
                // Thực hiện sự kiện tùy chỉnh:
                binding.value(event, el);
                // vnode.context[binding.expression](event); // vue 2
            }
        };
        document.body.addEventListener("click", el.clickOutsideEvent);
    },
    beforeUnmount: (el) => {
        document.body.removeEventListener("click", el.clickOutsideEvent);
    },
});

app.use(VueNumberFormat, {prefix: '', decimal: ',', thousand: '.'})

app.use(Toast, {
    transition: "Vue-Toastification__bounce",
    maxToasts: 20,
    newestOnTop: true,
    position: POSITION.TOP_CENTER,
});

app.mount('#app')
