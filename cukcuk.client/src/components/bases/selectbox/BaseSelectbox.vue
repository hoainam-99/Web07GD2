<template>
  <div class="selectbox" ref="selectbox">
    <input type="text" class="sb-input" :value="selectedValue" @keydown="selecItemUpDown" />
    <div
      class="sb-dropicon"
      @click="this.isShowSelectbox = !this.isShowSelectbox"
      @keydown="selecItemUpDown"
    >
      <i class="fa-solid fa-caret-down"></i>
    </div>
    <div
      class="sb-box"
      v-show="isShowSelectbox"
      :style="{ top: selectboxHeight + 1 + 'px' }"
      v-clickoutside="hideSelectbox"
    >
      <div
        class="sb-item"
        v-for="(item, index) in datas"
        :key="index"
        @click="selectItem(index)"
        :ref="'toFocus_' + index"
        :class="{'sb-selected-item': index == indexItemFocus }"
        @keydown="selecItemUpDown"
      >
        {{ item.data }}
      </div>
    </div>
  </div>
</template>

<script>
const keyCode = {
  Enter: 13,
  ArrowUp: 38,
  ArrowDown: 40,
  ESC: 27,
};
export default {
  props: ["selectData", "inputValue"],
  data() {
    return {
      // dữ liệu mảng các select item
      datas: this.selectData,

      // chiều cao của selectbox
      selectboxHeight: 0,

      // biến hiển thị bảng select
      isShowSelectbox: false,

      // biến chứa item được select
      selectedValue: null,

      // biến chứa index của item được select
      indexItemFocus: null,
    };
  },
  watch: {
    selectData() {
      this.datas = this.selectData;
    },
    inputValue() {
      this.setValue();
    },
  },
  methods: {
    /**
     * Hàm ẩn danh sách item
     * Author: LHNAM (10/10/2022)
     */
    hideSelectbox(){
      this.isShowSelectbox = false;
    },

    /**
     * Hàm lựa chọn item bằng các phím lên xuống và phím enter
     * Author: LHNAM (10/10/2022)
     */
    selecItemUpDown() {
      var me = this;
      var keyCodePress = event.keyCode;
      var elToFocus = null;
      switch (keyCodePress) {
        case keyCode.Esc:
          this.isShowSelectbox = false;
          break;
        case keyCode.ArrowDown:
          this.isShowSelectbox = true;
          elToFocus = this.$refs[`toFocus_${me.indexItemFocus + 1}`];
          if (
            this.indexItemFocus == null ||
            !elToFocus ||
            elToFocus.length == 0
          ) {
            this.indexItemFocus = 0;
          } else {
            this.indexItemFocus += 1;
          }
          break;
        case keyCode.ArrowUp:
          this.isShowSelectbox = true;
          elToFocus = this.$refs[`toFocus_${me.indexItemFocus - 1}`];
          if (
            this.indexItemFocus == null ||
            !elToFocus ||
            elToFocus.length == 0
          ) {
            this.indexItemFocus = this.dataFilter.length - 1;
          } else {
            this.indexItemFocus -= 1;
          }
          break;
        case keyCode.Enter:
          elToFocus = this.$refs[`toFocus_${me.indexItemFocus}`];
          if (elToFocus && elToFocus.length > 0) {
            elToFocus[0].click();
            this.isShowSelectbox = false;
          }
          break;
        default:
          break;
      }
    },
    /**
     * Hàm reset lại giá trị filter
     * Author: LHNAM (07/10/2022)
     */
    refreshValue() {
      this.setValue();
    },

    /**
     * Hàm set giá trị cho datas
     * Author: LHNAM (02/10/2022)
     */
    setValue() {
      if (this.inputValue) {
        this.datas.forEach((item, index) => {
          if (item.value == this.inputValue) {
            item.isChecked = true;
            this.indexItemFocus = index;
          } else {
            item.isChecked = false;
          }
        });

        if (this.checkData && typeof this.checkData == "function") {
          this.checkData();
        }
      }
    },

    /**
     * Hàm lấy chiều cao của selectbox
     * Author: LHNAM (28/09/2022)
     */
    getSelectboxHeight() {
      if (this.$refs["selectbox"]) {
        this.selectboxHeight = this.$refs["selectbox"].clientHeight;
      }
    },

    /**
     * Hàm được gọi dến sau khi chọn giá trị
     * @param {int} index index của phần tử trong mảng được chọn
     * Author: LHNAM (28/09/2022)
     */
    selectItem(index) {
      try {
        if (index != null || index != undefined) {
          // reset isChecked của phần tử được chọn trước đó về false
          this.datas.find((item) => {
            if (item.isChecked) {
              item.isChecked = false;
            }
          });

          // gán giá trị true cho isChecked của phần tử được chọn
          this.datas[index].isChecked = true;
          this.indexItemFocus = index;

          // Gán giá trị data của phần tử được chọn cho biến selectedValue
          if (this.datas[index].data) {
            this.selectedValue = this.datas[index].data;
          }

          // Trả về giá trị cho component cha
          this.$emit("getFilter", this.datas[index].value);

          // Tắt selectbox
          this.isShowSelectbox = false;
        }
      } catch (error) {
        console.error(error);
      }
    },
    
    /**
     * Hàm check data đầu vào
     * Author: LHNAM (28/09/2022)
     */
    checkData() {
      if (this.datas) {
        let selectedItem = this.datas.find((item) => {
          return item.isChecked == true;
        });

        if (selectedItem) {
          this.selectedValue = selectedItem.data;
          this.$emit("getFilter", selectedItem.value);
        }
      }
    },
  },
  created() {
    // set data đầu vào
    this.setValue();
  },
  mounted() {
    // lấy chiều cao của selectbox
    this.getSelectboxHeight();
  },
};
</script>

<style scoped>
.selectbox {
  width: 100%;
  height: 100%;
  position: relative;
}

.sb-input {
  width: 100%;
  height: 100%;
  border: 1px solid #ccc;
  padding: 2px 4px;
  outline: none;
}

.sb-dropicon,
.sb-addicon {
  height: 100%;
  width: auto;
  position: absolute;
  top: 0;
  right: 8px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.sb-dropicon {
  color: rgb(156, 156, 156);
}

.sb-dropicon .fa-caret-down{
  font-size: 16px;
}

/* .sb-dropicon .fa-caret-down:hover{
  color: #ccc;
} */

.sb-box {
  width: 100%;
  position: absolute;
  left: 0;
  background-color: #fff;
  z-index: 10;
}

.sb-item {
  width: 100%;
  height: 28px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 1px solid #ccc;
  cursor: pointer;
}

.sb-item:hover,
.sb-item-hover {
  background-color: #ccc;
}

.sb-selected-item {
  background-color: #0072bc;
  color: #fff;
}

.sb-selected-item:hover{
  background-color: #0072bc;
}
</style>
