<template>
  <div class="popup-container">
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left">{{ formMode[this.param].header }}</div>
          <div class="form-header__right">
            <i class="fa-solid fa-circle-xmark" @click="closeNoticePopup"></i>
          </div>
        </div>
        <div class="form-content">
          <div class="form-confirm" v-if="this.param != 'error'">
            {{ formMode[this.param].content }}
          </div>
          <div class="form-confirm" v-if="this.param == 'error'">
            <div
              v-for="(item, index) in formMode['error'].content"
              :key="index"
            >
              <span v-if="item.error">{{ item.error }}</span>
              <span v-else>{{ item }}</span>
            </div>
          </div>
        </div>
        <!-- footer khi là pop-up xác nhận xóa -->
        <div class="form-footer" v-if="param == 'deleteConfirm'">
          <div class="form-footer__left">
            <div tabindex="0" @focus="shiftTabKeyOnPress"></div>
            <button class="btn cancel-btn" @click="closeNoticePopup" ref="bt1">
              <span>Hủy bỏ</span>
            </button>
          </div>
          <div class="form-footer__right">
            <button
              class="btn submit-btn"
              @click="returnConfirmPopupOnClick(true)"
              :disabled="isDisabled"
              ref="bt2"
            >
              Đồng ý
            </button>
            <div tabindex="0" @focus="tabKeyOnPress"></div>
          </div>
        </div>

        <!-- footer khi là pop-up thông báo lỗi -->
        <div class="form-footer" v-if="param == 'error'">
          <div class="form-footer__left">
            <div tabindex="0" @focus="shiftTabKeyOnPress"></div>
            <button class="btn help-btn" ref="bt1">
              <span>Giúp</span>
            </button>
          </div>
          <div class="form-footer__right">
            <button class="btn submit-btn" @click="closeNoticePopup" ref="bt2">
              Đồng ý
            </button>
            <div tabindex="0" @focus="tabKeyOnPress"></div>
          </div>
        </div>

        <!-- footer khi là pop-up xác nhận lưu -->
        <div class="form-footer" v-if="param == 'saveConfirm'">
          <div class="form-footer__left">
            <div tabindex="0" @focus="shiftTabKeyOnPress"></div>
            <button class="btn cancel-btn" @click="closeNoticePopup" ref="bt1">
              <span>Hủy</span>
            </button>
          </div>
          <div class="form-footer__right">
            <button
              class="btn cancel-btn"
              @click="returnConfirmPopupOnClick(false)"
              :disabled="isDisabled"
            >
              <span>Không</span>
            </button>
            <button
              class="btn save-btn"
              @click="returnConfirmPopupOnClick(true)"
              :disabled="isDisabled"
              ref="bt2"
            >
              <span>Có</span>
            </button>
            <div tabindex="0" @focus="tabKeyOnPress"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Resource from "@/js/Resource";
export default {
  props: {
    param: String,
    deleteItem: Object,
    errorMsg: Array,
  },
  watch: {
    errorMsg() {
      if (this.setData) {
        this.setData();
      }
    },
  },
  data() {
    return {
      // biến dùng để vô hiệu hóa button
      isDisabled: false,

      // Object chứa các biến thể của pop-up
      formMode: {
        // biến thể xác nhận xóa
        deleteConfirm: {
          header: "Thông báo",
          content: "",
        },
        // biến thể xác nhận lưu
        saveConfirm: {
          header: "Thông báo",
          content: "Dữ liệu đã bị thay đổi, bạn có muốn cất không?",
        },
        // biến thể thông báo lỗi
        error: {
          header: "Lỗi!",
          content: [],
        },
      },
    };
  },
  methods: {
    /**
     * Hàm để focus quay lại khi nhấn phím shiftTab
     * Author: LHNAM (14/09/2022)
     */
    shiftTabKeyOnPress() {
      try {
        if (this.$refs.bt2) {
          this.$refs.bt2.focus();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm để focus quay lại khi nhấn phím tab
     * Author: LHNAM (14/09/2022)
     */
    tabKeyOnPress() {
      try {
        if (this.$refs.bt1) {
          this.$refs.bt1.focus();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm sự kiện đóng bảng thông báo
     * Author: LHNAM (05/10/2022)
     */
    closeNoticePopup() {
      try {
        this.$emit(Resource.Emit.CloseForm, false);
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xét thông báo confirm xóa
     * Author: LHNAM (05/10/2022)
     */
    setData() {
      if (this.deleteItem) {
        this.formMode.deleteConfirm.content = `Bạn có chắc chắn muốn xóa nguyên vật liệu { ${this.deleteItem.materialCode} - ${this.deleteItem.materialName} } này không?`;
      }

      if (this.errorMsg) {
        this.formMode.error.content = this.errorMsg;
      }
    },

    /**
     * Hàm trả về giá trị confirm của người dùng cho component cha
     * @param {boolean} e giá trị người dùng confirm
     * Author: LHNAM (04/10/2022)
     */
    returnConfirmPopupOnClick(e) {
      try {
        this.isDisabled = true;
        this.$emit(Resource.Emit.ReturnConfirmPopup, e);
      } catch (error) {
        console.error(error);
      }
    },
  },
  created() {
    // chạy hàm khi khởi động pop-up
    this.setData();
  },
  mounted() {
    // focus vào button
    if (this.$refs["bt1"]) {
      this.$refs["bt1"].focus();
    }
  },
};
</script>

<style scoped>
@import url(./popup.css);
.popup-container {
  z-index: 13;
}
.popup-main {
  width: 400px;
}
</style>
