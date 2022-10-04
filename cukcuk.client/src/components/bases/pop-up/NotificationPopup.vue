<template>
  <div class="popup-container">
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left">Thông báo</div>
          <div class="form-header__right">
            <i class="fa-solid fa-up-right-and-down-left-from-center"></i>
            <i class="fa-solid fa-circle-xmark" @click="returnConfirmPopupOnClick(false)"></i>
          </div>
        </div>
        <div class="form-content">
            <div class="form-confirm">
                Bạn có chắc chắn muốn xóa nguyên vật liệu này không?
            </div>
        </div>
        <div class="form-footer">
            <button class="btn cancel-btn" @click="returnConfirmPopupOnClick(false)">Hủy bỏ</button>
            <button class="btn submit-btn" @click="returnConfirmPopupOnClick(true)">Đồng ý</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
    data(){return {}},
    methods: {
        /**
         * Hàm trả về giá trị confirm của người dùng cho component cha
         * @param {boolean} e giá trị người dùng confirm
         * Author: LHNAM (04/10/2022)
         */
        returnConfirmPopupOnClick(e){
            try {
                this.$emit('returnConfirmPopup', e);
            } catch (error) {
                console.error(error);
            }
        }
    }
};
</script>

<style></style>
