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
                  ref="materialName"
                />
              </div>
              <div class="form-group">
                <label for="" title="Đơn vị tính">ĐVT <span>(*)</span></label>
                <BaseCombobox
                  :url="units"
                  :propValue="'unitID'"
                  :propText="'unitName'"
                  :inputValue="material.unitID"
                  :isRequire="isUnitRequired"
                  @getValue="getUnitID"
                  @addBtnOnClick="showAddUnitPopup"
                />
              </div>
              <div class="form-group">
                <label for="">Hạn sử dụng</label>
                <input
                  type="text"
                  pattern="[0-9]"
                  class="w50per"
                  style="margin-right: 4px"
                  v-model="material.expiryDate.dateValue"
                />
                <BaseSelectbox
                  :selectData="timeSelectData"
                  :inputValue="material.expiryDate.dateType"
                  @getFilter="getExpiryDate"
                />
              </div>
            </div>
            <div class="form-col w50per" style="margin-left: 40px">
              <div class="form-group">
                <label for="">Mã <span>(*)</span></label>
                <input
                  type="text"
                  class="w100per"
                  v-model="material.materialCode"
                  ref="materialCode"
                />
              </div>
              <div class="form-group">
                <label for="">Kho ngầm định</label>
                <BaseCombobox
                  :url="stocks"
                  :propValue="'stockID'"
                  :propText="'stockName'"
                  :inputValue="material.stockID"
                  @getValue="getStockID"
                  @addBtnOnClick="showAddStockPopup"
                />
              </div>
              <div class="form-group">
                <label for="">SL tối thiểu</label>
                <input
                  type="text"
                  class="w100per"
                  v-model="material.inventoryNumber"
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
                <TheMaterialUnit
                  v-for="(item, index) in material.materialUnit"
                  :key="index"
                  :materialUnit="item"
                  :itemIndex="index"
                  :materialUnitName="material.unitName"
                  @returnValue="setMaterialUnitValue"
                />
              </tbody>
            </table>
          </div>
          <div class="form-table__toolbar">
            <button class="btn btn-add" @click="addMaterialUnitOnClick">
              <i class="fa-solid fa-file-circle-plus"></i>
              <span>Thêm dòng</span>
            </button>
            <button class="btn btn-delete" @click="removeMaterialUnitOnClick">
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
            <button class="btn save-btn" @click="saveDataOnClick(1)">
              <i class="fa-solid fa-floppy-disk"></i>
              <span>Cất</span>
            </button>
            <button class="btn save-add-btn" @click="saveDataOnClick(2)">
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
import CommonFn from "@/js/Common";
import Resource from "@/js/Resource";
import TheMaterialUnit from "../materialUnit/TheMaterialUnit.vue";

export default {
  components: { BaseSelectbox, BaseCombobox, TheMaterialUnit },
  props: ["param"],
  watch: {},
  data() {
    return {
      // Biến validate trường unit
      isUnitRequired: false,

      // thông tin bản ghi nguyên vật liệu
      material: {
        expiryDate: {
          dateType: Resource.DateType.Date,
          dateValue: 0,
        },
        materialUnit: [],
      },

      // ID của nguyên vật liệu được chọn
      materialID: "",

      // url đơn vị tính
      units: Axios.Url.Unit,

      // url kho
      stocks: Axios.Url.Stock,

      // Mảng chứa danh sách thời hạn sử dụng
      timeSelectData: [
        { data: "Ngày", value: Resource.DateType.Date, isChecked: false },
        { data: "Tháng", value: Resource.DateType.Month, isChecked: false },
        { data: "Năm", value: Resource.DateType.Year, isChecked: false },
      ],

      // Object đơn vị chuyển đổi
      materialUnit: {
        unitID: "",
        conversionRate: "",
        calculation: Enum.Calculation.Multiplication,
        method: Enum.FormMode.Add,
      },
    };
  },
  methods: {
    showAddUnitPopup(){},
    showAddStockPopup(){},
    
    /**
     * Hàm lấy dữ liệu trả về từ combobox chọn đơn vị
     * @param {Guid} value ID của đơn vị
     * @param {String} text Tên đơn vị
     */
    getUnitID(value, text) {
      this.material.unitID = value;
      this.material.unitName = text;
    },

    /**
     * Hàm lấy dữ liệu trả về từ combobox chọn kho
     * @param {Guid} value ID của kho
     * @param {String} text Tên kho
     */
    getStockID(value, text) {
      if (this.material.stockID) {
        this.material.stockID = value;
      }

      if (this.material.stockName) {
        this.material.stockName = text;
      }
    },

    /**
     * Hàm lấy giá trị trả về của selectbox chọn hạn sử dụng
     * @param {Enum} value giá trị trả về
     * Author: LHNAM (03/10/2022)
     */
    getExpiryDate(value) {
      if (this.material.expiryDate.DateType) {
        this.material.expiryDate.DateType = value;
      }
    },
    /**
     * Hàm trả về giá trị của materialUnit
     * @param {materialUnit} value giá trị trả về của materialUnit
     * @param {int} index vị trí của materialUnit trong mảng
     * Author: LHNAM (03/10/2022)
     */
    setMaterialUnitValue(value, index) {
      if (value && index) {
        if (this.material.materialUnit) {
          this.material.materialUnit[index - 1] = value;
        }
      }
    },
    /**
     * Hàm gọi api để lưu dữ liệu
     * Author: LHNAM (03/10/2022)
     */
    saveMaterial(data) {
      if (this.param) {
        switch (this.param.method) {
          case Enum.FormMode.Add:
          case Enum.FormMode.Replication:
            Axios.CallAxios(Axios.Methods.Post, Axios.Url.Material, data)
              .then((res) => {
                console.log(res);
              })
              .catch((e) => {
                console.error(e);
              })
              .finally(() => {
                this.loading = false;
              });
            break;
          case Enum.FormMode.Edit:
            Axios.CallAxios(
              Axios.Methods.Put,
              `${Axios.Url.Material}/${this.param.id}`,
              data
            )
              .then((res) => {
                console.log(res);
              })
              .catch((e) => {
                console.error(e);
              })
              .finally(() => {
                this.loading = false;
              });
            break;
        }
      }
    },

    /**
     * Hàm validate dữ liệu
     * Author: LHNAM (03/10/2022)
     */
    validate() {
      let isValid = true;
      if (!this.material.materialCode) {
        if (this.$refs["materialCode"]) {
          this.$refs["materialCode"].classList.add("red-border");
          this.$refs["materialCode"].setAttribute(
            "title",
            Resource.ValidateMes.requireError
          );
          isValid = false;
        } else {
          this.$refs["materialCode"].classList.remove("red_border");
          this.$refs["materialCode"].removeAttribute("title");
        }
      }

      if (!this.material.materialName) {
        if (this.$refs["materialName"]) {
          this.$refs["materialName"].classList.add("red-border");
          this.$refs["materialName"].setAttribute(
            "title",
            Resource.ValidateMes.requireError
          );
          isValid = false;
        } else {
          this.$refs["materialName"].classList.remove("red_border");
          this.$refs["materialName"].removeAttribute("title");
        }
      }

      if (!this.material.unitID) {
        this.isUnitRequired = true;
        isValid = false;
      } else {
        this.isUnitRequired = false;
      }
      return isValid;
    },

    /**
     * Hàm event bấm nút cất, cất và thêm để lưu data
     * Author: LHNAM (03/10/2022)
     */
    saveDataOnClick(saveMode) {
      try {
        let isValid = true,
          data = {};
        debugger;
        if (this.validate && typeof this.validate == "function") {
          isValid = this.validate();
        }

        if (isValid) {
          data = Object.assign({}, this.material);
          data.expiryDate = CommonFn.formatOutputExpiryDate(data.expiryDate);
          delete data.materialID;

          this.saveMaterial(data);

          switch (saveMode) {
            case Enum.SaveMode.Save:
              if (
                this.closeFormOnClick &&
                typeof this.closeFormOnClick == "function"
              ) {
                this.closeFormOnClick();
              }
              break;
            case Enum.SaveMode.SaveAdd:
              this.material = {
                expiryDate: {
                  dateType: Resource.DateType.Date,
                  dateValue: 0,
                },
                materialUnit: [],
              };
              this.getNewMaterialCode();
              if (this.$refs["materialName"]) {
                this.$refs["materialName"].focus();
              }
              break;
          }
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xóa dòng mới nhất trong mảng materialUnit
     * Author: LHNAM (03/10/2022)
     */
    removeMaterialUnitOnClick() {
      try {
        if (this.material.materialUnit) {
          let length = this.material.materialUnit.length;

          if (
            this.material.materialUnit[length - 1].method == Enum.FormMode.Edit
          ) {
            this.material.materialUnit[length - 1].method =
              Enum.FormMode.Delete;
          } else {
            this.material.materialUnit.pop();
          }
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thêm dòng mới vào mảng materialUnit
     * Author: LHNAM (02/10/2022)
     */
    addMaterialUnitOnClick() {
      try {
        if (this.material.materialUnit) {
          this.material.materialUnit.push(this.materialUnit);
        }
      } catch (error) {
        console.error(error);
      }
    },

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
          res.data.expiryDate = CommonFn.formatInputExpiryDate(
            res.data.expiryDate
          );
          this.material = res.data;
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
    setupPopup() {
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
              this.getMaterialDetail();
              this.getNewMaterialCode();
              break;
          }
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
  created() {
    // khởi tạo pop-up
    this.setupPopup();
  },
  mounted() {
    if (this.$refs["materialName"]) {
      this.$refs["materialName"].focus();
    }
  },
};
</script>

<style>
@import url(./popup.css);
</style>
