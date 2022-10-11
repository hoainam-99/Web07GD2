<template>
  <div class="navigation-left">
    <div class="prev-page">
      <i
        class="fa-solid fa-angles-left cursor-pointer"
        @click="firstPageOnClick"
      ></i>
      <i
        class="fa-solid fa-chevron-left cursor-pointer"
        @click="prevPageOnClick"
      ></i>
    </div>
    <div class="select-page">
      <span>Trang</span>
      <input type="text" pattern="[0-9]" v-model="pagination.pageNum" />
      <span>trên {{ totalPage }}</span>
    </div>
    <div class="next-page">
      <i
        class="fa-solid fa-chevron-right cursor-pointer"
        @click="nextPageOnClick"
      ></i>
      <i
        class="fa-solid fa-angles-right cursor-pointer"
        @click="finalPageOnClick"
      ></i>
    </div>
    <div class="refresh-page cursor-pointer" @click="refresh">
      <i class="fa-solid fa-rotate-right"></i>
    </div>
    <div class="page-sizing">
      <input type="text" pattern="[0-9]" disabled :value="pagination.pageSize" />
      <div class="page-size-dropdown cursor-pointer" @click="isShowPageSizeList = !isShowPageSizeList">
        <i class="fa-solid fa-caret-down"></i>
      </div>
      <div class="pageSizeList" v-show="isShowPageSizeList" v-clickoutside="hidePageSizeList">
        <div class="pageSizeItem" v-for="item in pageSizeList" :key="item" @click="changePageSize(item)">{{item}}</div>
      </div>
    </div>
  </div>
  <div class="navigation-right">
    <p>
      Hiển thị {{ (pagination.pageNum - 1) * pagination.pageSize + 1 }} -
      {{ pagination.pageNum * pagination.pageSize }} trên {{ totalCount }} kết quả
    </p>
  </div>
</template>

<script>
export default {
  props: ["totalCount"],
  emits: ["refresh", "changePagination"],
  data() {
    return {
        // Biến phân trang
      pagination: {
        pageNum: 1,
        pageSize: 50,
      },
      // Biến chứa các kích thước trang
      pageSizeList: [25, 50, 100],

      // biến chứa tổng số trang
      totalPage: 0,

      // biến hiển thị select chọn pageSize
      isShowPageSizeList: false,
    };
  },
  watch: {
    pagination: {
      handler(newValue) {
        if(newValue.pageNum < 1){
            this.pagination.pageNum = 1;
        }

        if(newValue.pageNum > this.totalPage){
            this.pagination.pageNum = this.totalPage;
        }
        this.changePage();        
      },
      deep: true,
    },
    totalCount() {
        // xét số trang về 1
      this.pagination.pageNum = 1;

      // tính lại tổng số trang
      this.setNavigation();
    },
  },
  methods: {
    /**
     * Hàm ẩn bảng chọn item
     * Author: LHNAM (10/10/2022)
     */
    hidePageSizeList(){
      this.isShowPageSizeList = false;
    },

    /**
     * Hàm refresh lại phân trang
     * Author: LHNAM (30/09/2022)
     */
    refresh(){
      try {
        this.$emit('refresh', true);
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thay đổi giá trị số bản ghi trên 1 trang
     * @param {int} pageSize Số bản ghi trên 1 trang
     * Author: LHNAM (30/09/2022)
     */
    changePageSize(pageSize){
      try {
        if(pageSize){
          this.pagination.pageSize = pageSize;
          this.isShowPageSizeList = false;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm trả về giá trị cho component cha khi thay đổi số trang và kích thước trang 
     * Author: LHNAM (30/09/2022)
     */
    changePage(){
        if(this.pagination){
            this.$emit('changePagination', this.pagination);
            this.setNavigation();
        }
    },

    /**
     * Hàm sự kiện click sang trang tiếp theo
     * Author: LHNAM (30/09/2022)
     */
    nextPageOnClick() {
      if (this.pagination.pageNum) {
        this.pagination.pageNum++;
      }
    },

    /**
     * Hàm sự kiện click sang trang cuối 
     * Author: LHNAM (30/09/2022)
     */
    finalPageOnClick() {
      if (this.pagination.pageNum) {
        this.pagination.pageNum = this.totalPage;
      }
    },

    /**
     * Hàm sự kiện click về trang trước 
     * Author: LHNAM (30/09/2022)
     */
    prevPageOnClick() {
      if (this.pagination.pageNum) {
        this.pagination.pageNum--;
      }
    },

    /**
     * Hàm sự kiện click về trang đầu
     * Author: LHNAM (30/09/2022)
     */
    firstPageOnClick() {
      if (this.pagination.pageNum) {
        this.pagination.pageNum = 1;
      }
    },

    /**
     * Hàm tính tổng số trang dựa theo tổng số bản ghi
     * Author: LHNAM (30/09/2022)
     */
    setNavigation() {
      if (this.pagination.pageSize && this.totalCount) {
        this.totalPage = parseInt(this.totalCount / this.pagination.pageSize);

        if (this.totalCount % this.pagination.pageSize > 0) {
          this.totalPage++;
        }
      }
    },
  },
  created(){
    // trả về giá trị phân trang cho component cha
    this.changePage();
  },
};
</script>

<style scoped>
.navigation-left {
  height: 100%;
  padding: 3px 0;
  display: flex;
  justify-content: center;
  align-items: center;
  color: #9a9a9a;
}

.navigation-left span {
  color: #000;
}

.navigation-left input {
  width: 38px;
  padding: 2px 4px;
  border: 1px solid #ccc;
  outline: none;
}

.navigation-left .fa-solid {
  margin: 0 8px;
}

.select-page {
  height: 100%;
  border-left: 2px solid #ccc;
  border-right: 2px solid #ccc;
  display: flex;
  align-items: center;
}

.select-page span {
  margin: 0 12px;
}

.refresh-page {
  height: 100%;
  border-left: 2px solid #ccc;
  border-right: 2px solid #ccc;
  padding: 0 6px;
  margin: 0 6px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.page-sizing {
  display: flex;
  margin-left: 6px;
  position: relative;
}

.page-size-dropdown {
  width: 100%;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid #ccc;
}

.pageSizeList{
  position: absolute;
  bottom: 24px;
  left: 0;
  background-color: #fff;
  border: 1px solid #ccc;
  width: 100%;
}

.pageSizeItem{
  width: 100%;
  height: 24px;
  padding: 4px 8px;
  color: #000;
  cursor: pointer;
}

.pageSizeItem:hover{
  background-color: #ccc;
}

.navigation-right {
  display: flex;
  align-items: center;
}
</style>
