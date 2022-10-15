<template>
  <div class="popup-container add-popup">
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left">
            {{ formatForm[this.param].header }}
          </div>
          <div class="form-header__right">
            <i class="fa-solid fa-circle-xmark" @click="closeFormOnClick"></i>
          </div>
        </div>
        <div class="form-content">
          <div class="form-col w100per">
            <div tabindex="0" @focus="shiftTabKeyOnPress"></div>
            <div class="form-group" v-if="formatForm[this.param].codeLabel">
              <label for=""
                >{{ formatForm[this.param].codeLabel }} <span>(*)</span></label
              >
              <input
                type="text"
                class="w100per"
                v-model="data.code"
                ref="code"
              />
            </div>
            <div class="form-group" v-if="formatForm[this.param].nameLabel">
              <label for=""
                >{{ formatForm[this.param].nameLabel }} <span>(*)</span></label
              >
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
          <div class="form-footer__left">
            <button class="btn help-btn">
              <i class="fa-regular fa-circle-question"></i>
              <span>Giúp</span>
            </button>
          </div>
          <div class="form-footer__right">
            <button class="btn save-btn" @click="saveDataOnClick">
              <span>Cất</span>
            </button>
            <button
              class="btn cancel-btn"
              @click="closeFormOnClick"
              ref="cancel"
            >
              <span>Hủy bỏ</span>
            </button>
            <div tabindex="0" @focus="tabKeyOnPress"></div>
          </div>
        </div>
      </div>
    </div>
    <NotificationPopup
      v-if="isShowNotificationPopup"
      :param="notificationPopupParam"
      :errorMsg="errorMsg"
      @closeForm="closeNoticePopup"
    />
  </div>
</template>

<script>
import Resource from "@/js/Resource";
import Axios from "@/js/Axios";
import NotificationPopup from "./NotificationPopup.vue";
import CommonFn from "@/js/Common";
import { useToast } from "vue-toastification";
export default {
  props: {
    param: String,
  },
  data() {
    return {
      // Biến chứa toast message
      toast: useToast(),

      // Mảng chứa lỗi
      errorMsg: [],

      // Biến đầu vào của popup thông báo
      notificationPopupParam: "",

      // Biến hiển thị pop-up
      isShowNotificationPopup: false,

      // Biến định dạng thêm mới
      formatForm: {
        Stock: {
          header: "Thêm kho ngầm định",
          codeLabel: "Mã kho",
          nameLabel: "Tên kho",
        },
        Unit: {
          header: "Thêm đơn vị tính",
          nameLabel: "Tên đơn vị",
        },
      },

      // Data lấy từ input
      data: { code: "", name: "" },
    };
  },
  methods: {
    /**
     * Hàm để focus quay lại khi nhấn phím tab
     * Author: LHNAM (14/09/2022)
     */
    tabKeyOnPress() {
      try {
        if (this.$refs.code) {
          this.$refs.code.focus();
        } else if (this.$refs.name) {
          this.$refs.name.focus();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm để focus quay lại khi nhấn phím shiftTab
     * Author: LHNAM (14/09/2022)
     */
    shiftTabKeyOnPress() {
      try {
        if (this.$refs.cancel) {
          this.$refs.cancel.focus();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm trả về đóng bảng thông báo
     * Author: LHNAM (05/10/2022)
     */
    closeNoticePopup(e) {
      if (this.isShowNotificationPopup) {
        this.isShowNotificationPopup = e;
      }
    },
    /**
     * Hàm trả về emit để reset lại form
     * Author: LHNAM (05/10/2022)
     */
    refreshData(id) {
      this.$emit(Resource.Emit.RefreshData, this.param, id);
    },
    /**
     * Hàm đẩy data lên api
     * Author: LHNAM (05/10/2022)
     */
    saveData(postData) {
      Axios.CallAxios(Axios.Methods.Post, Axios.Url[this.param], postData)
        .then((res) => {
          this.toast.success(Resource.Notice[`Create${this.param}Success`], {
            timeout: 2000,
            hideProgressBar: false,
          });
          this.refreshData(res.data);
          this.closeFormOnClick();
        })
        .catch((e) => {
          this.errorMsg = CommonFn.getError(e.response);
          this.notificationPopupParam = "error";
          this.isShowNotificationPopup = true;
        })
        .finally(() => {
          this.loading = false;
        });
    },
    /**
     * Hàm validate các trường trước khi đẩy lên api
     * Author: LHNAM (05/10/2022)
     */
    validate() {
      let isValid = true;
      this.errorMsg = [];
      if (this.param == "Stock" && !this.data.code) {
        if (this.$refs["code"]) {
          this.$refs["code"].classList.add("red-border");
          this.$refs["code"].setAttribute(
            "title",
            Resource.ErrorMes.requireError
          );
          this.errorMsg.push(Resource.ErrorMes.e006);
          isValid = false;
        } else {
          this.$refs["code"].classList.remove("red_border");
          this.$refs["code"].removeAttribute("title");
        }
      }
      if (!this.data.name) {
        if (this.$refs["name"]) {
          this.$refs["name"].classList.add("red-border");
          this.$refs["name"].setAttribute(
            "title",
            Resource.ErrorMes.requireError
          );
          if (this.param == "Stock") {
            this.errorMsg.push(Resource.ErrorMes.e007);
          } else {
            this.errorMsg.push(Resource.ErrorMes.e009);
          }
          isValid = false;
        } else {
          this.$refs["name"].classList.remove("red_border");
          this.$refs["name"].removeAttribute("title");
        }
      }
      return isValid;
    },
    /**
     * Hàm event click lưu thông tin
     * Author: LHNAM (05/10/2022)
     */
    saveDataOnClick() {
      try {
        let isValid = this.validate(),
          postData;
        switch (this.param) {
          case "Unit":
            postData = {
              unitName: this.data.name,
            };
            break;
          case "Stock":
            postData = {
              stockCode: this.data.code,
              stockName: this.data.name,
            };
        }
        if (isValid) {
          this.saveData(postData);
        } else {
          this.notificationPopupParam = "error";
          this.isShowNotificationPopup = true;
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm event click đóng form
     * Author: LHNAM (05/10/2022)
     */
    closeFormOnClick() {
      try {
        this.$emit(Resource.Emit.CloseForm, false);
      } catch (error) {
        console.error(error);
      }
    },
  },
  mounted() {
    if (this.$refs["code"]) {
      this.$refs["code"].focus();
    } else if (this.$refs["name"]) {
      this.$refs["name"].focus();
    }
  },
  components: { NotificationPopup },
};
</script>

<style scoped>
@import url(./popup.css);
.add-popup {
  z-index: 15;
}

.popup-main {
  min-width: 400px;
}
</style>
