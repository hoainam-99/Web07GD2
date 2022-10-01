<template>
  <div class="selectbox" ref="selectbox">
    <input type="text" class="sb-input" :value="selectedValue" />
    <div
      class="sb-dropicon"
      :class="{ sb_dropicon_v2: isAddBtnExists }"
      @click="this.isShowSelectbox = !this.isShowSelectbox"
    >
      <i class="fa-solid fa-caret-down"></i>
    </div>
    <div class="sb-addicon" v-if="isAddBtnExists">
      <i class="fa-solid fa-plus"></i>
    </div>
    <div
      class="sb-box"
      v-show="isShowSelectbox"
      :style="{ top: selectboxHeight + 1 + 'px' }"
    >
      <div
        class="sb-item"
        v-for="(item, index) in datas"
        :key="index"
        @click="selectItem(index)"
      >
        {{ item.data }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["isAddBtnExists", "selectData"],
  data() {
    return {
      datas: this.selectData,
      selectboxHeight: 0,
      isShowSelectbox: false,
      selectedValue: null,
    };
  },
  watch: {
    selectData(){
      this.datas = this.selectData;
    }
  },
  methods: {
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

          // Gán giá trị data của phần tử được chọn cho biến selectedValue
          if(this.datas[index].data){
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

        this.selectedValue = selectedItem.data;
        this.$emit("getFilter", selectedItem.value);
      }
    },
  },
  created() {
    // check data đầu vào
    this.checkData();
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

.sb_dropicon_v2 {
  right: 28px;
}

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

.sb-item:hover{
  background-color: #ccc;
}

.sb-selected-item {
  background-color: #0072bc;
}
</style>
