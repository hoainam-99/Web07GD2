<template lang="">
  <div class="field-input">
    <input
      class="input"
      maxlength="11"
      :type="type || 'text'"
      :class="{
        'error-input': valRequired,
        'input-text-left': textLeft,
        'input-text-center': textCenter,
        'input-text-right': textRight,
      }"
      :placeholder="placeholder || ''"
      :style="styleVal"
      :value="type == 'date' ? val : value"
      ref="inputFocus"
      @input="onInput"
      @change="getValue"
      @mouseover="mouseOver"
      @mouseout="mouseOut"
      @keyup="onKeyUp"
      @blur="onBlur"
      @paste.prevent="onPaste"
    />
    <div v-show="showErorr" class="error-input-alert">
      <p class="error-input-arrow"></p>
      <div class="error-input-content">
        <p>
          {{ messError }}
        </p>
      </div>
    </div>
  </div>
</template>
<script>
import { thisExpression } from "@babel/types";

export default {
  data() {
    return {
      val: "", // val
      modelInput: "", // model input
      valRequired: false, // không được bỏ trống dữ liệu
      showErorr: false, // Thông báo lỗi
      valueChangeInput: null, // chỉ sử dung cho type = float
    };
  },
  props: {
    placeholder: String, // giá trị của placeholder input
    messError: String, // nội dung lỗi khi validate input
    type: [String, Number], // loại input
    value: [String, Number], // giá trị của input nhậ từ parent
    fieldName: String, // Trường dữ liệu trả về cho parent
    focus: Boolean, // focus vào input hay không
    styleVal: String, // css cho input
    required: Boolean, // bắt buộc phải nhập dữ liệu
    maxLength: Number, // Độ dài tối đa của input
    id: [String, Number],
    lengthAfterComma: [String, Number], // số phần tủ sau dấu phẩy type = float
    textLeft: Boolean,
    textRight: Boolean,
    textCenter: Boolean,
  },
  methods: {
    /**
     * Lấy giá trị input trả về cho parent
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    getValue() {
      this.$emit("getValue", {
        val: this.val,
        fieldName: this.fieldName,
        id: this.id,
      });
    },

    /**
     * Check dữ liệu khi ng dùng nhập
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    validate() {
      if (this.required) {
        this.valRequired = true;
        this.showErorr = false;
        return {
          status: false,
          messError: this.messError,
        };
      } else {
        this.valRequired = false;
        return {
          status: true,
        };
      }
    },

    /**
     * sự kiện oninput
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    onInput(valueInput) {
      // check độ dài tối đa
      if (this.maxLength) {
        if (parseInt(this.maxLength) <= event.target.value.length) {
          event.target.value = event.target.value.substr(0, this.maxLength);
        }
        this.val = event.target.value;
        this.$emit("update:value", event.target.value);
        this.showErorr = false;
        this.valRequired = false;
      } else {
        this.val = event.target.value;
        this.$emit("update:value", event.target.value);
        this.showErorr = false;
        this.valRequired = false;
      }
      // Check format lại nếu là double
      if (this.type == "float") {
        var selectionStart = valueInput.target.selectionStart;
        if (valueInput.data >= 0 && valueInput.data <= 9) {
          var positionComma = this.val.indexOf(","); // vị trí dấu phẩy

          // nếu con trỏ ở trên dấu phẩy
          if (selectionStart <= positionComma) {
            this.positionCursor = selectionStart;
            var valueAfterComma = this.val.slice(positionComma);
            let dotsBeforeFormat = this.val.slice(0, selectionStart).split(".");
            let valueBeforeComma = this.val
              .slice(0, positionComma)
              .replace(/[.]/g, "")
              .replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            let dotsAfterFormat = valueBeforeComma
              .slice(0, selectionStart)
              .split(".");
            this.val = valueBeforeComma + valueAfterComma;
            if (!valueBeforeComma && valueInput.data == null) this.val = "";

            this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));

            this.$nextTick(() => {
              if (
                dotsBeforeFormat.length != dotsAfterFormat.length &&
                valueInput.data == null
              ) {
                this.$refs.inputFocus.selectionEnd = selectionStart - 1;
              } else if (
                dotsBeforeFormat.length != dotsAfterFormat.length &&
                valueInput.data != null
              ) {
                this.$refs.inputFocus.selectionEnd = selectionStart + 1;
              } else {
                this.$refs.inputFocus.selectionEnd = selectionStart;
              }
            });
          } else if (positionComma < 0 && valueInput.data != null) {
            let listZero = Array.from({ length: this.lengthAfterComma })
              .fill("0")
              .join("");
            this.val = `${this.val.slice(0, this.val.length - 1)}${
              valueInput.data
            },${listZero}`;
            this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
            this.$nextTick(() => {
              this.$refs.inputFocus.selectionEnd = selectionStart;
            });
          } else if (positionComma < 0 && valueInput.data == null) {
            var positionCommaValueOld = this.inputBeforeChange.indexOf(",");
            if (valueInput.inputType == "deleteContentBackward") {
              this.val =
                this.inputBeforeChange.slice(0, positionCommaValueOld - 1) +
                this.inputBeforeChange.slice(positionCommaValueOld);
              let dotsBeforeFormat = this.val
                .slice(0, selectionStart)
                .split(".");
              let valueBeforeComma = this.val
                .slice(0, positionCommaValueOld - 1)
                .replace(/[.]/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ".");
              let dotsAfterFormat = valueBeforeComma
                .slice(0, selectionStart)
                .split(".");

              if (valueBeforeComma)
                this.val =
                  valueBeforeComma +
                  this.inputBeforeChange.slice(positionCommaValueOld);
              else this.val = "";
              this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
              this.$nextTick(() => {
                if (dotsBeforeFormat.length != dotsAfterFormat.length)
                  this.$refs.inputFocus.selectionEnd =
                    positionCommaValueOld - 2;
                else
                  this.$refs.inputFocus.selectionEnd =
                    positionCommaValueOld - 1;
              });
            } else if (valueInput.inputType == "deleteContentForward") {
              this.val = JSON.parse(JSON.stringify(this.inputBeforeChange));
              this.$nextTick(() => {
                this.$refs.inputFocus.selectionEnd = positionCommaValueOld;
              });
            }
          } else {
            if (valueInput.data) {
              if (selectionStart == this.val.length) {
                this.val =
                  this.val.slice(0, selectionStart - 2) + valueInput.data;
              }
              this.val =
                this.val.slice(0, selectionStart) +
                this.val.slice(selectionStart + 1);
              this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
              this.$nextTick(() => {
                // this.val = value
                this.$refs.inputFocus.selectionEnd = selectionStart;
              });
            } else {
              if (valueInput.inputType == "deleteContentBackward") {
                if (selectionStart == this.val.length) {
                  this.val = this.val.slice(0, selectionStart) + "0";
                } else {
                  this.val =
                    this.val.slice(0, selectionStart) +
                    this.val.slice(selectionStart) +
                    "0";
                }
                this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
                this.$nextTick(() => {
                  this.$refs.inputFocus.selectionEnd = selectionStart;
                });
              } else if (valueInput.inputType == "deleteContentForward") {
                this.val =
                  this.val.slice(0, selectionStart) +
                  this.val.slice(selectionStart) +
                  "0";
                this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
                this.$nextTick(() => {
                  this.$refs.inputFocus.selectionEnd = selectionStart;
                });
              }
            }
          }
        } else if (valueInput.data == "." || valueInput.data == ",") {
          this.val =
            this.val.slice(0, selectionStart - 1) +
            this.val.slice(selectionStart);

          let positionComma = this.val.indexOf(",");
          this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
          this.$nextTick(() => {
            this.$refs.inputFocus.selectionEnd = positionComma + 1;
          });
        } else {
          this.val =
            this.val.slice(0, selectionStart - 1) +
            this.val.slice(selectionStart);
          this.inputBeforeChange = JSON.parse(JSON.stringify(this.val));
          this.$nextTick(() => {
            this.$refs.inputFocus.selectionEnd = selectionStart - 1;
          });
        }
        this.$emit("update:value", this.val);
        this.$refs.inputFocus.value = this.val;
      }
      this.$emit("onInput", {
        val: this.val,
        fieldName: this.fieldName,
        id: this.id,
      });
    },

    /**
     * keyboard
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    onKeyUp() {
      this.$emit("keyboard", {
        val: this.val,
        fieldName: this.fieldName,
        id: this.id,
        keyCode: event.keyCode,
      });
    },

    /**
     * Hover input
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    mouseOver() {
      if (this.valRequired) this.showErorr = true;
    },

    /**
     * mouseOut
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    mouseOut() {
      this.showErorr = false;
    },

    /**
     * sự kiện khi blur
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    onBlur() {
      this.$emit("onBlur", {
        val: this.val,
        fieldName: this.fieldName,
        id: this.id,
      });
    },

    /**
     * Sự kiện khi paste văn bản vào input
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    onPaste(valueInput) {
      if (this.type == "float") {
        let dataInput = valueInput.clipboardData.getData("text");
        let format = /[A-Za-z!@#$%^&*()_+\-=\[\]{};':"\\|<>\/? ]/;
        if (format.test(dataInput)) {
          this.$refs.inputFocus.value = "";
        } else {
          dataInput = dataInput.replace(/[.]/g, "").split(",");
          let beforeComma = dataInput[0];
          let afterComma = "";
          let afterComma1 = "";
          dataInput.shift();
          if (dataInput.length > 0) {
            afterComma = dataInput.join("");
          }

          for (let i = 0; i < this.lengthAfterComma; i++) {
            if (afterComma[i]) {
              afterComma1 += afterComma[i];
            } else afterComma1 += "0";
          }
          let val = `${beforeComma
            .replace(/[.]/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ".")},${afterComma1}`;
          this.$refs.inputFocus.value = val;
          this.val = val;
        }
        this.$emit("onInput", {
          val: this.val,
          fieldName: this.fieldName,
          id: this.id,
        });
        this.$emit("getValue", {
          val: this.val,
          fieldName: this.fieldName,
          id: this.id,
        });
      }
    },

    /**
     * focus
     * CreatedBy: NDCHIEN (18/8/2022)
     */
    focusFunc() {
      this.$refs.inputFocus.focus();
    },
    /**
     * Hàm xử lý dữ liệu khi component khởi động
     * Author: LHNAM (12/10/2022)
     */
    setupData() {
      switch (this.type) {
        case "date":
          if (this.value) {
            this.val = new Date(this.value);
            this.val = new Date(this.value).toISOString().split("T")[0];
          } else this.val = ``;
          break;
        case "float":
          if (this.value) {
            let a = parseInt(this.value);
            let checklengthAfterComma = 2;
            if (this.lengthAfterComma)
              checklengthAfterComma = this.lengthAfterComma;
            let b = (parseFloat(this.value) - parseInt(this.value))
              .toFixed(checklengthAfterComma)
              .split(".")[1];
            a = `${a}`
              .replace(/[.]/g, "")
              .replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            this.$emit("update:value", `${a},${b}`);
            this.$nextTick(() => {
              this.$refs.inputFocus.value = `${a},${b}`;
            });
            this.inputBeforeChange = `${a},${b}`;
          }
          break;
        default:
          this.modelInput = this.value || "";
          this.val = this.value || "";
      }
    },
  },
  watch: {
    value(newVal, oldValue) {
      if (this.type == "date") {
        if (newVal) this.val = new Date(newVal).toISOString().split("T")[0];
        else this.val = null;
      }
      if(oldValue == '' || oldValue == null){
          if (this.setupData) {
            this.setupData();
          }
      }
    },
    valueChangeInput(newVal) {
      if (this.type == "float") {
        this.$refs.inputFocus.value = newVal;
      }
    },
  },
  created() {
    if (this.setupData) {
      this.setupData();
    }
  },
  mounted() {
    if (this.focus) {
      this.$refs.inputFocus.focus();
    }
  },
};
</script>
<style scoped>
@import url("./input-default.css");
</style>
