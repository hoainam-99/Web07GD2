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
                <input
                  type="text"
                  class="w100per"
                  v-model="material.materialName"
                />
              </div>
              <div class="form-group">
                <label for="" title="Đơn vị tính">ĐVT <span>(*)</span></label>
                <BaseCombobox
                  :url="units"
                  :propValue="'unitID'"
                  :propText="'unitName'"
                  :inputValue="material.unitID"
                />
              </div>
              <div class="form-group">
                <label for="">Hạn sử dụng</label>
                <input type="text" class="w50per" style="margin-right: 4px" />
                <BaseSelectbox :selectData="timeSelectData" />
              </div>
            </div>
            <div class="form-col w50per" style="margin-left: 40px">
              <div class="form-group">
                <label for="">Mã <span>(*)</span></label>
                <input
                  type="text"
                  class="w100per"
                  v-model="material.materialCode"
                />
              </div>
              <div class="form-group">
                <label for="">Kho ngầm định</label>
                <BaseCombobox
                  :url="stocks"
                  :propValue="'stockID'"
                  :propText="'stockName'"
                  :inputValue="material.stockID"
                />
              </div>
              <div class="form-group">
                <label for="">SL tối thiểu</label>
                <input
                  type="text"
                  class="w100per"
                  v-model="material.materialName"
                />
              </div>
            </div>
          </div>
          <div class="form-row">
            <label for="">Mô tả</label>
            <textarea
              rows="3"
              class="w100per"
              v-model="material.description"
            ></textarea>
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
                <tr v-for="(item, index) in material.materialUnit" :key="index">
                  <th>{{ index + 1}}</th>
                  <td>{{ item.unitName }}</td>
                  <td>{{ item.conversionRate }}</td>
                  <td>{{ item.calculation }}</td>
                  <td>1 {{ item.unitName }} = {{ item.conversionRate }} * {{ material.unitName }}</td>
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
import Axios from "@/js/Axios.js";
import BaseSelectbox from "../selectbox/BaseSelectbox.vue";
import BaseCombobox from "../combobox/BaseCombobox.vue";
import Enum from "@/js/Enum";

export default {
  components: { BaseSelectbox, BaseCombobox },
  props: ["param"],
  data() {
    return {
      // thông tin bản ghi nguyên vật liệu
      material: {},

      // ID của nguyên vật liệu được chọn
      materialID: "",

      // url đơn vị tính
      units: Axios.Url.Unit,

      // url kho
      stocks: Axios.Url.Stock,

      // Mảng chứa danh sách thời hạn sử dụng
      timeSelectData: [
        { data: "Ngày", value: "Date", isChecked: true },
        { data: "Tháng", value: "Month", isChecked: false },
        { data: "Năm", value: "Year", isChecked: false },
      ],
    };
  },
  methods: {
    /**
     * Hàm lấy mã nguyên vật liệu mới
     * Author: LHNAM (01/10/2022)
     */
    getNewMaterialCode() {
      Axios.CallAxios(Axios.Methods.Get, Axios.Url.NewMaterialCode)
        .then((res) => {
          this.material.materialCode = res.data;
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => {
          this.loading = false;
        });
    },
    /**
     * Hàm lấy thông tin bản ghi nguyên vật liệu
     * Author: LHNAM (01/10/2022)
     */
    getMaterialDetail() {
      Axios.CallAxios(
        Axios.Methods.Get,
        `${Axios.Url.Material}/${this.param.id}`
      )
        .then((res) => {
          this.material = res.data;
          console.log(res.data);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => {
          this.loading = false;
        });
    },

    /**
     * Hàm lấy sự kiện bấm nút đóng form pop-up
     * Author: LHNAM (28/09/2022)
     */
    closeFormOnClick() {
      try {
        this.$emit("closeForm", false);
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm triển khai pop-up sau khi được mở
     * Author: LHNAM (01/10/2022)
     */
    async setupPopup() {
      try {
        if (this.param) {
          switch (this.param.method) {
            case Enum.FormMode.Add:
              this.getNewMaterialCode();
              break;
            case Enum.FormMode.Edit:
              this.getMaterialDetail();
              break;
            case Enum.FormMode.Replication:
              await this.getMaterialDetail();
              await this.getNewMaterialCode();
              break;
          }
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
  created() {
    this.setupPopup();
  },
};
</script>

<style>
@import url(./popup.css);
</style>
