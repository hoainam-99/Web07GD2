<template>
  <div class="popup-container">
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left">Thêm nguyên vật liệu</div>
          <div class="form-header__right">
            <i class="fa-solid fa-up-right-and-down-left-from-center"></i>
            <i class="fa-solid fa-circle-xmark" @click="closeFormOnClick"></i>
          </div>
        </div>
        <div class="form-content">
          <div class="form-row">
            <div class="form-col w50per">
              <div class="form-group">
                <label for="">Tên <span>(*)</span></label>
                <input type="text" class="w100per" />
              </div>
              <div class="form-group">
                <label for="" title="Đơn vị tính">ĐVT <span>(*)</span></label>
                <BaseSelectbox  :isAddBtnExists="true"/>
              </div>
              <div class="form-group">
                <label for="">Hạn sử dụng</label>
                <input type="text" class="w50per" style="margin-right: 4px;" />
                <BaseSelectbox/>
              </div>
            </div>
            <div class="form-col w50per" style="margin-left: 40px">
              <div class="form-group">
                <label for="">Mã <span>(*)</span></label>
                <input type="text" class="w100per" />
              </div>
              <div class="form-group">
                <label for="">Kho ngầm định</label>
                <BaseSelectbox :isAddBtnExists="true"/>
              </div>
              <div class="form-group">
                <label for="">SL tối thiểu</label>
                <input type="text" class="w100per" />
              </div>
            </div>
          </div>
          <div class="form-row">
            <label for="">Mô tả</label>
            <textarea rows="3" class="w100per"></textarea>
          </div>
          <div class="form-menu">
            <div class="form-menu__item">Đơn vị chuyển đổi</div>
          </div>
          <div class="form-table">
            <table border="1">
              <thead>
                <tr>
                  <th>STT</th>
                  <th>Đơn vị chuyển đổi</th>
                  <th>Tỷ lệ chuyển đổi</th>
                  <th>Phép tính</th>
                  <th>Mô tả</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <th>1</th>
                  <td>Thùng</td>
                  <td>24</td>
                  <td>*</td>
                  <td>1 Thùng = 24 * chai</td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="form-table__toolbar">
            <button class="btn btn-add">
                <i class="fa-solid fa-file-circle-plus"></i>
              <span>Thêm dòng</span>
            </button>
            <button class="btn btn-delete">
              <i class="fa-solid fa-x"></i>
              <span>Xóa dòng</span>
            </button>
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
            <button class="btn save-btn">
              <i class="fa-solid fa-floppy-disk"></i>
              <span>Cất</span>
            </button>
            <button class="btn save-add-btn">
              <i class="fa-solid fa-floppy-disk"></i>
              <span>Cất & Thêm</span>
            </button>
            <button class="btn cancel-btn" @click="closeFormOnClick">
              <i class="fa-solid fa-ban"></i>
              <span>Hủy bỏ</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BaseSelectbox from '../selectbox/BaseSelectbox.vue';

export default {
    props: [],
    data() {
        return {};
    },
    methods: {
        /**
           * Hàm lấy sự kiện bấm nút đóng form pop-up
           * Author: LHNAM (28/09/2022)
           */
        closeFormOnClick() {
            try {
                this.$emit("closeForm", false);
            }
            catch (error) {
                console.error(error);
            }
        }
    },
    components: { BaseSelectbox }
};
</script>

<style>
@import url(./popup.css);
</style>
