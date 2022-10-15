<template>
  <div class="hn-filter" @keydown.prevent.enter="emitFilter">
    <div class="hn-filter-main">
      <input class="hn-filter__input" v-model="filterValue" />
      <div
        class="hn-filter__btn"
        @click="isShowFilterSelect = !isShowFilterSelect"
      >
        {{ filterSelectedItem }}
      </div>
    </div>
    <div
      class="hn-filter__select"
      v-show="isShowFilterSelect"
      v-clickoutside="selectOnClickoutside"
    >
      <div
        class="hn-filter__item"
        v-for="(item, index) in filterItems"
        :key="index"
        @click="changeFilter(index)"
        :class="{
          'hn-filter__selected-item': item.value == filterSelectedItem,
        }"
      >
        {{ item.value }} : {{ item.note }}
      </div>
    </div>
  </div>
</template>

<script>
import Resource from "@/js/Resource";
// import debounce from "lodash.debounce";
export default {
  props: ["filterType"],
  data() {
    return {
      // biến chứa các loại filter
      filterList: {
        string: [
          { value: "*", note: "Chứa" },
          { value: "=", note: "Bằng" },
          { value: "+", note: "Mở đầu bằng" },
          { value: "-", note: "Kết thúc bằng" },
          { value: "!", note: "Không chứa" },
        ],
        number: [
          { value: "=", note: "Bằng" },
          { value: "<", note: "Nhỏ hơn" },
          { value: "<=", note: "Nhỏ hơn hoặc bằng" },
          { value: ">", note: "Lớn hơn" },
          { value: ">=", note: "Lớn hơn hoặc bằng" },
        ],
      },

      // phương thức filer
      filterItems: [],

      // phương thức filter được chọn
      filterSelectedItem: "",

      // value của filter
      filterValue: "",

      // hiển thị danh sách của phương thức filter
      isShowFilterSelect: false,
    };
  },
  watch: {
    filterSelectedItem() {
      this.emitFilter();
    },
  },
  methods: {
    /**
     * Hàm click out side để tắt danh sách chọn phương thức filter
     * Author: LHNAM (14/10/2022)
     */
    selectOnClickoutside() {
      try {
        if (this.isShowFilterSelect) {
          this.isShowFilterSelect = false;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm set lại giá trị lọc
     * Author: LHNAM (13/10/2022)
     */
    refreshValue() {
      this.filterValue = "";
      this.filterSelectedItem = this.filterItems[0].value;
      this.emitFilter();
    },
    
    /**
     * Hàm thay đổi phép filter khi click chọn
     * @param {int} index index của phần tử được chọn
     * Author: LHNAM (30/09/2022)
     */
    changeFilter(index) {
      try {
        if (index != null) {
          this.filterSelectedItem = this.filterItems[index].value;
          this.isShowFilterSelect = false;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm trả về giá trị filter sau khi được xử lý
     * Author: LHNAM (30/09/2022)
     */
    emitFilter() {
      let filter = "";
      if (this.filterSelectedItem && this.filterValue) {
        switch (this.filterSelectedItem) {
          case "+":
            filter = `%2b${this.filterValue}`;
            break;
          case "-":
            filter = `%2d${this.filterValue}`;
            break;
          case "!":
            filter = `%21${this.filterValue}`;
            break;
          case "=":
            filter = `%3d${this.filterValue}`;
            break;
          case "*":
            filter = `%2a${this.filterValue}`;
            break;
        }
      }
      this.$emit(Resource.Emit.GetFilter, filter);
    },
  },
  created() {
    // tạo độ trễ khi trả về giá trị
    // this.debouncedEmitFilter = debounce(() => {
    //   this.emitFilter();
    // }, 500);
    this.emitFilter();

    // Gán giá trị cho list filter
    this.filterItems = this.filterList[this.filterType];

    // Gán giá trị cho phần tử default
    this.filterSelectedItem = this.filterItems[0].value;
  },
};
</script>

<style scoped>
.hn-filter {
  display: flex;
  width: 100%;
  height: 28px;
  cursor: pointer;
  position: relative;
}

.hn-filter-main{
  display: flex;
  width: 100%;
  height: 100%;
  flex-direction: row-reverse;
}

.hn-filter__btn {
  width: 28px;
  height: 28px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 1px solid #ccc;
  background-color: #fff;
}

.hn-filter__input {
  height: 100%;
  width: calc(100% - 28px);
  outline: none;
  border: 1px solid #ccc;
  padding: 0 6px;
}

.hn-filter__select {
  position: absolute;
  top: 29px;
  left: 0;
  background: #fff;
  text-align: left;
}

.hn-filter__item {
  padding: 6px 8px 6px 24px;
  height: 24px;
  display: flex;
  align-items: center;
}

.hn-filter__item:hover {
  background-color: #ccc;
}

.hn-filter__selected-item {
  background-image: url(https://cukcukcomnew.misacdn.net/QLNH/resources/Image/table-circle-blue16.png);
  background-repeat: no-repeat;
  background-position: 4px 4px;
}
</style>
