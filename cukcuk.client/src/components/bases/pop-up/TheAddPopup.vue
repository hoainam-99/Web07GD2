<template>
  <div class="popup-container">
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left">{{ formatForm[this.param].header }}</div>
          <div class="form-header__right">
            <i class="fa-solid fa-up-right-and-down-left-from-center"></i>
            <i
              class="fa-solid fa-circle-xmark"
              @click="returnConfirmPopupOnClick(false)"
            ></i>
          </div>
        </div>
        <div class="form-content">
          <div class="form-col w100per">
            <div class="form-group">
              <label for="">{{ formatForm[this.param].codeLabel }} <span>(*)</span></label>
              <input
                type="text"
                class="w100per"
                v-model="data.code"
                ref="code"
              />
            </div>
            <div class="form-group">
              <label for="">{{ formatForm[this.param].nameLabel }} <span>(*)</span></label>
              <input
                type="text"
                class="w100per"
                v-model="data.name"
                ref="name"
              />
            </div>
          </div>
        </div>
        <div class="form-footer">
          <button
            class="btn cancel-btn"
            @click="closeForm"
          >
            Hủy bỏ
          </button>
          <button
            class="btn submit-btn"
            @click="addBtnOnClick"
          >
            Đồng ý
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    param: String,
  },
  data() {
    return {
      formatForm: {
        Stock: {
          header: "Thêm kho ngầm định",
          codeLabel: "Mã kho",
          nameLabel: "Tên kho",
        },
        Unit: {
          header: "Thêm đơn vị tính",
          codeLabel: "Mã đơn vị",
          nameLabel: "Tên đơn vị",
        },
      },
      data: { code: "", name: "" },
    };
  },
  methods: {
    /**
     * Hàm trả về giá trị confirm của người dùng cho component cha
     * @param {boolean} e giá trị người dùng confirm
     * Author: LHNAM (04/10/2022)
     */
    returnConfirmPopupOnClick(e) {
      try {
        this.$emit("returnConfirmPopup", e);
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>

<style></style>
