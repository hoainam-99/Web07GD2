<template>
  <div
    class="popup-container"
    @keydown.exact.ctrl.s.prevent="saveDataOnClick(1)"
    @keydown.exact.ctrl.shift.s.prevent="saveDataOnClick(2)"
    @keydown.prevent.esc="xBtnOnClick"
    @keydown.prevent.f1="helpBtnOnClick"
    >
    <!-- @keydown.prevent.ctrl.b="closeFormOnClick" -->
    <div class="popup-main">
      <div class="form">
        <div class="form-header">
          <div class="form-header__left" v-if="formMode == 1 || formMode == 4">Thêm nguyên vật liệu</div>
          <div class="form-header__left" v-if="formMode == 2">Sửa nguyên vật liệu</div>
          <div class="form-header__right">
            <i class="fa-solid fa-circle-xmark" @click="xBtnOnClick"></i>
          </div>
        </div>
        <div class="form-content">
          <div class="form-row">
            <div class="form-col w50per">
              <div class="form-group">
                <div tabindex="0" @focus="shiftTabKeyOnPress"></div>
                <label for="">Tên <span>(*)</span></label>
                <input
                  type="text"
                  class="w100per"
                  v-model="material.materialName"
                  @change="getMaterialCode"
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
                  :isRefresh="isRefreshUnit"
                  @getValue="getUnitID"
                  @addBtnOnClick="showAddUnitPopup"
                  ref="unitID"
                />
              </div>
              <div class="form-group">
                <label for="">Hạn sử dụng</label>
                <input
                  type="text"
                  pattern="[0-9]"
                  maxlength="10"
                  class="w50per"
                  style="margin-right: 4px; text-align: right"
                  v-model="material.expiryDate.dateValue"
                  @keypress="isNumber($event)"
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
                  :isRefresh="isRefreshStock"
                  @getValue="getStockID"
                  @addBtnOnClick="showAddStockPopup"
                />
              </div>
              <div class="form-group">
                <label for="" title="Số lượng tối thiểu">SL tối thiểu</label>
                <BaseInput
                  style="width: 100%"
                  :fieldName="'inventoryNumber'"
                  type="float"
                  textRight
                  lengthAfterComma="2"
                  v-model:value="material.inventoryNumber"
                  @onInput="getData"
                  ref="inventoryNumber"
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
                  <th style="width: 30px">STT</th>
                  <th style="width: 200px">Đơn vị chuyển đổi</th>
                  <th style="width: 200px">Tỷ lệ chuyển đổi</th>
                  <th style="width: 80px">Phép tính</th>
                  <th>Mô tả</th>
                </tr>
              </thead>
              <tbody>
                <TheMaterialUnit
                  v-for="(item, index) in material.materialUnit"
                  :key="item.unitID"
                  :materialUnit="item"
                  :itemIndex="index"
                  :materialUnitName="material.unitName"
                  v-model:selectedMaterialUnit="selectedMaterialUnit"
                  :class="{'hn-selected-tr': selectedMaterialUnit == index}"
                  @returnValue="setMaterialUnitValue"
                  :ref="`materialUnit${index}`"
                />
              </tbody>
            </table>
          </div>
          <div class="form-table__toolbar">
            <button class="btn btn-add" @click="addMaterialUnitOnClick">
              <i class="fa-solid fa-file-circle-plus"></i>
              <span>Thêm dòng</span>
            </button>
            <button
              class="btn btn-delete"
              @click="removeMaterialUnitOnClick"
              :disabled="material.materialUnit.length == 0"
            >
              <i class="fa-solid fa-x"></i>
              <span>Xóa dòng</span>
            </button>
          </div>
        </div>
        <div class="form-footer">
          <div class="form-footer__left">
            <button
              class="btn help-btn"
              @click="helpBtnOnClick"
              title="Giúp (F1)"
            >
              <i class="fa-regular fa-circle-question"></i>
              <span>Giúp</span>
            </button>
          </div>
          <div class="form-footer__right">
            <button
              class="btn save-btn"
              @click="saveDataOnClick(1)"
              :disabled="isDisabled"
              title="Cất (Ctrl + S)"
            >
              <span>Cất</span>
            </button>
            <button
              class="btn save-add-btn"
              @click="saveDataOnClick(2)"
              :disabled="isDisabled"
              title="Cất và Thêm (Ctrl + Shift + S)"
            >
              <span>Cất & Thêm</span>
            </button>
            <button
              class="btn cancel-btn"
              @click="closeFormOnClick"
              ref="cancel"
              title="Hủy bỏ (Ctrl + B)"
            >
              <span>Hủy bỏ</span>
            </button>
            <div tabindex="0" @focus="tabKeyOnPress"></div>
          </div>
        </div>
        <BaseLoading v-show="isShowLoading" />
      </div>
    </div>
  </div>
  <TheAddPopup
    v-if="isShowAddPopup"
    :param="addPopupParam"
    @closeForm="closeAddForm"
    @refreshData="refreshCombobox"
  />
  <NotificationPopup
    v-if="isShowNotificationPopup"
    :param="notificationPopupParam"
    :errorMsg="errorMsg"
    @returnConfirmPopup="returnConfirmPopup"
    @closeForm="closeNoticePopup"
  />
</template>

<script>
import Axios from "@/js/Axios.js";
import { useToast } from "vue-toastification";
import BaseSelectbox from "../selectbox/BaseSelectbox.vue";
import BaseCombobox from "../combobox/BaseCombobox.vue";
import debounce from "lodash.debounce";
import Enum from "@/js/Enum";
import CommonFn from "@/js/Common";
import Resource from "@/js/Resource";
import TheMaterialUnit from "../materialUnit/TheMaterialUnit.vue";
import TheAddPopup from "./TheAddPopup.vue";
import NotificationPopup from "./NotificationPopup.vue";
import BaseLoading from "../BaseLoading.vue";
import BaseInput from "../BaseNumberInput/BaseInput.vue";

export default {
  components: {
    BaseSelectbox,
    BaseCombobox,
    TheMaterialUnit,
    TheAddPopup,
    NotificationPopup,
    BaseLoading,
    BaseInput,
  },
  props: ["param"],
  emits: ["closeForm", "returnResult"],
  watch: {
    material: {
      handler() {
        this.isChange = true;
      },
      deep: true,
    },
  },
  data() {
    return {
      // biến chứa index materialUnit được chọn
      selectedMaterialUnit: null,

      // Mảng chứa đơn vị chuyển đổi bị xóa
      deleteMaterialUnitArr: [],

      // biến định dạng kiểu form
      formMode: this.param.method,

      // biến để vô hiệu hóa nút cất, cất và thêm
      isDisabled: false,

      // Mảng chứa lỗi
      errorMsg: [],

      // Biến hiển thị loading
      isShowLoading: false,

      // Biến chứa toast message
      toast: useToast(),

      // Biến biểu thỉ dữ liệu đã bị thay đổi
      isChange: true,

      // Biến đầu vào của pop-up thông báo
      notificationPopupParam: "",

      // Biến hiển thị pop-up thông báo
      isShowNotificationPopup: false,

      // Biến refresh combobox kho
      isRefreshStock: false,

      // Biến refresh combobox đơn vị tính
      isRefreshUnit: false,

      // Đầu vào của form thêm
      addPopupParam: "",

      // Biến hiển thị form thêm đơn vị và kho
      isShowAddPopup: false,

      // Biến validate trường unit
      isUnitRequired: false,

      // thông tin bản ghi nguyên vật liệu
      material: {
        materialName: "",
        expiryDate: {
          dateType: Resource.DateType.Day,
          dateValue: 0,
        },
        materialUnit: [],
        status: 2,
      },

      // ID của nguyên vật liệu được chọn
      materialID: "",

      // url đơn vị tính
      units: Axios.Url.Unit,

      // url kho
      stocks: Axios.Url.Stock,

      // Mảng chứa danh sách thời hạn sử dụng
      timeSelectData: [
        { data: "Ngày", value: Resource.DateType.Day, isChecked: false },
        { data: "Tháng", value: Resource.DateType.Month, isChecked: false },
        { data: "Năm", value: Resource.DateType.Year, isChecked: false },
      ],
    };
  },
  methods: {
    /**
     * Hàm lọc phím bấm số cho input
     * @param {*} evt event nhận vào khi nhận sự kiện
     * Author: LHNAM (13/10/2022)
     */
    isNumber(evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        evt.preventDefault();
      } else {
        return true;
      }
    },

    /**
     * Hàm bấm nút giúp đỡ
     * Author: LHNAM (13/10/2022)
     */
    helpBtnOnClick() {
      try {
        window.open(
          "https://help.cukcuk.com/vi/1060300_them_NVL.htm",
          "_blank"
        );
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm set value số cho số lượng tối thiểu
     * @param {Object} data Object chứa nội dung trả về
     * Author: LHNAM (12/10/2022)
     */
    getData(data) {
      // check null
      if (this.material && data) {
        // Khi gọi hàm component sẽ trả về
        this.material[data.fieldName] = data.val;
      }
    },
    /**
     * Hàm để focus quay lại khi nhấn phím tab
     * Author: LHNAM (14/09/2022)
     */
    tabKeyOnPress() {
      try {
        if (this.$refs.materialName) {
          this.$refs.materialName.focus();
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
     * Hàm lấy mã nguyên vật liệu
     * Author: LHNAM (08/10/2022)
     */
    getMaterialCode() {
      try {
        if (this.formMode != Enum.FormMode.Edit) {
          this.debouncedGetCode();
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm gọi đến khi click nút x trên form
     * Author: LHNAM (05/10/2022)
     */
    xBtnOnClick() {
      try {
        if (this.isChange && !this.isShowNotificationPopup) {
          this.notificationPopupParam =
            Resource.NotificationPopupParam.SaveConfirm;
          this.isShowNotificationPopup = true;
        } else {
          this.closeFormOnClick();
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
        this.isDisabled = false;

        if (this.errorMsg) {
          if (this.$refs[this.errorMsg[0].key]) {
            if (this.$refs[this.errorMsg[0].key].componentFocus) {
              this.$refs[this.errorMsg[0].key].componentFocus();
            } else if (this.$refs[this.errorMsg[0].key][0]) {
              if (this.$refs[this.errorMsg[0].key][0].componentFocus) {
                this.$refs[this.errorMsg[0].key][0].componentFocus();
              }
            } else {
              this.$refs[this.errorMsg[0].key].focus();
            }
          }
        }
      }
    },

    /**
     * Hàm trả về confirm của người dùng
     * @param {Boolean} e kết quả trả về
     * Author: LHNAM (05/10/2022)
     */
    returnConfirmPopup(e) {
      if (e) {
        this.isShowNotificationPopup = false;
        this.saveDataOnClick(Enum.SaveMode.Save);
      } else {
        this.closeFormOnClick();
      }
    },
    /**
     * Hàm refresh combobox unit
     * Author: LHNAM (05/10/2022)
     */
    refreshUnit() {
      if (!this.isRefreshUnit) {
        this.isRefreshUnit = true;
      }
    },

    /**
     * Hàm refresh combobox stock
     * Author: LHNAM (05/10/2022)
     */
    refreshStock() {
      if (!this.isRefreshStock) {
        this.isRefreshStock = true;
      }
    },

    /**
     * Hàm nhận emit refresh từ component TheAddPopup
     * @param {String} param biến chứa tên combobox cần refresh
     * Author: LHNAM (05/10/2022)
     */
    refreshCombobox(param, id) {
      if (
        this[`refresh${param}`] &&
        typeof this[`refresh${param}`] == "function"
      ) {
        this[`refresh${param}`]();
      }

      if(id){
        this.material[`${param.toLowerCase()}ID`] = id;
      }
    },

    /**
     * Hàm đóng form thêm đơn vị tính và kho
     * @param {Boolean} e Giá trị trả về từ emit
     * Author: LHNAM (05/10/2022)
     */
    closeAddForm(e) {
      if (this.isShowAddPopup) {
        this.isShowAddPopup = e;
      }
    },

    /**
     * Hàm mở form thêm đơn vị tính
     * Author: LHNAM (05/10/2022)
     */
    showAddUnitPopup(e) {
      if (!this.isShowAddPopup && e) {
        this.addPopupParam = Resource.Table.Unit;
        this.isShowAddPopup = e;
      }

      if (this.isRefreshUnit) {
        this.isRefreshUnit = false;
      }
    },

    /**
     * Hàm mở form thêm kho ngầm định
     * Author: LHNAM (05/10/2022)
     */
    showAddStockPopup(e) {
      if (!this.isShowAddPopup && e) {
        this.addPopupParam = Resource.Table.Stock;
        this.isShowAddPopup = e;
      }

      if (this.isRefreshStock) {
        this.isRefreshStock = false;
      }
    },

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
      this.material.stockID = value;

      this.material.stockName = text;
    },

    /**
     * Hàm lấy giá trị trả về của selectbox chọn hạn sử dụng
     * @param {Enum} value giá trị trả về
     * Author: LHNAM (03/10/2022)
     */
    getExpiryDate(value) {
      if (this.material.expiryDate.dateType) {
        this.material.expiryDate.dateType = value;
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
    saveMaterial(data, saveMode) {
      if (this.formMode) {
        switch (this.formMode) {
          case Enum.FormMode.Add:
          case Enum.FormMode.Replication:
            Axios.CallAxios(Axios.Methods.Post, Axios.Url.Material, data)
              .then(() => {
                this.toast.success(Resource.Notice.CreateMaterialSuccess, {
                  timeout: 2000,
                  hideProgressBar: false,
                });
              })
              .then(() => {
                this.saveModeActive(saveMode);
              })
              .then(()=>{
                this.isChange = false;
              })
              .catch((e) => {
                this.isDisabled = false;
                this.errorMsg = CommonFn.getError(e.response);
                this.notificationPopupParam =
                  Resource.NotificationPopupParam.Error;
                this.isShowNotificationPopup = true;
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
              .then(() => {
                this.toast.success(Resource.Notice.UpdateMaterialSuccess, {
                  timeout: 2000,
                  hideProgressBar: false,
                });
              })
              .then(() => {
                this.saveModeActive(saveMode);
              })
              .then(()=>{
                this.isChange = false;
              })
              .catch((e) => {
                this.isDisabled = false;
                this.errorMsg = CommonFn.getError(e.response);
                this.notificationPopupParam =
                  Resource.NotificationPopupParam.Error;
                this.isShowNotificationPopup = true;
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
    validate(data) {
      let isValid = true;
      this.errorMsg = [];

      // validate tên nguyên vật liệu bỏ trống
      if (!data.materialName) {
        if (this.$refs[Resource.KeyTable.MaterialName]) {
          this.$refs[Resource.KeyTable.MaterialName].classList.add(
            "red-border"
          );
          this.$refs[Resource.KeyTable.MaterialName].setAttribute(
            "title",
            Resource.ErrorMes.requireError
          );
          this.errorMsg.push({
            key: Resource.KeyTable.MaterialName,
            error: Resource.ErrorMes.e004,
          });
          isValid = false;
        }
      } else {
        this.$refs[Resource.KeyTable.MaterialName].classList.remove(
          "red-border"
        );
        this.$refs[Resource.KeyTable.MaterialName].removeAttribute("title");
      }

      if (!data.materialCode) {
        if (this.$refs[Resource.KeyTable.MaterialCode]) {
          this.$refs[Resource.KeyTable.MaterialCode].classList.add(
            "red-border"
          );
          this.$refs[Resource.KeyTable.MaterialCode].setAttribute(
            "title",
            Resource.ErrorMes.requireError
          );
          this.errorMsg.push({
            key: Resource.KeyTable.MaterialCode,
            error: Resource.ErrorMes.e003,
          });
          isValid = false;
        }
      } else {
        this.$refs[Resource.KeyTable.MaterialCode].classList.remove(
          "red-border"
        );
        this.$refs[Resource.KeyTable.MaterialCode].removeAttribute("title");
      }

      if (!data.unitID) {
        this.isUnitRequired = true;
        this.errorMsg.push({
          key: Resource.KeyTable.UnitID,
          error: Resource.ErrorMes.e010,
        });
        isValid = false;
      } else {
        this.isUnitRequired = false;
      }

      if (data.materialUnit) {
        this.material.materialUnit.forEach((item, index) => {
          if (item.unitID == data.unitID) {
            this.errorMsg.push({
              key: `materialUnit${index}`,
              error: Resource.ErrorMes.conversionUnit_Unit_Diffrence,
            });

            if (this.$refs[`materialUnit${index}`]) {
              this.$refs[`materialUnit${index}`][0].isError = true;
            }

            isValid = false;
          } else if (item.unitID == Resource.KeyTable.EmptyGuid) {
            this.errorMsg.push({
              key: `materialUnit${index}`,
              error: Resource.ErrorMes.e013,
            });

            if (this.$refs[`materialUnit${index}`]) {
              this.$refs[`materialUnit${index}`][0].isError = true;
            }

            isValid = false;
          } else {
            if (this.$refs[`materialUnit${index}`]) {
              this.$refs[`materialUnit${index}`][0].isError = false;
            }
          }
        });
      }
      return isValid;
    },

    /**
     * Hàm xác nhận trạng thái lưu dữ liệu
     * Author: LHNAM (03/10/2022)
     */
    saveModeActive(saveMode) {
      this.$emit(Resource.Emit.ReturnResult, true);
      // kiểm tra kiểu lưu được chuyển vào
      switch (saveMode) {
        // trường hợp bấm nút cất
        case Enum.SaveMode.Save:
          if (
            this.closeFormOnClick &&
            typeof this.closeFormOnClick == "function"
          ) {
            this.closeFormOnClick();
          }
          break;
        // trường hợp bấm nút cất và thêm
        case Enum.SaveMode.SaveAdd:
          this.material = {
            expiryDate: {
              dateType: Resource.DateType.Date,
              dateValue: 0,
            },
            materialUnit: [],
            status: 2
          };
          // hủy vô hiệu hóa nút cất, cất và thêm
          this.isDisabled = false;

          // focus lại vào input nhập tên nguyên vật liệu
          if (this.$refs[Resource.KeyTable.MaterialName]) {
            this.$refs[Resource.KeyTable.MaterialName].focus();
          }

          this.deleteMaterialUnitArr = [];

          this.formMode = Enum.FormMode.Add;
          break;
        }
    },

    /**
     * Hàm event bấm nút cất, cất và thêm để lưu data
     * Author: LHNAM (03/10/2022)
     */
    saveDataOnClick(saveMode) {
      try {
        let isValid = true,
          data = {};
        // vô hiệu hóa nút cất, cất và thêm
        this.isDisabled = true;

        // gán dữ liệu của nguyên vật liệu
        data = Object.assign({}, this.material);

        // format lại giá trị ngày tháng năm
        data.expiryDate = CommonFn.formatOutputExpiryDate(
          this.material.expiryDate
        );

        // format lại dữ liệu kiểu số cho trường số lượng tối thiểu
        data.inventoryNumber = CommonFn.formatNumber(data.inventoryNumber);

        if(this.deleteMaterialUnitArr.length > 0){
          this.deleteMaterialUnitArr.forEach(item=>{
            data.materialUnit.push(item);
          });
        }
        // format lại kiểu dữ liệu cho các trường trong mảng đơn vị chuyển đổi
        data.materialUnit.map(item=>{
          item.conversionRate = CommonFn.formatNumber(item.conversionRate);
        });

        // xóa trường ID nguyên vật liệu
        delete data.materialID;

        if (this.validate && typeof this.validate == "function") {
          isValid = this.validate(data);
        }

        if (isValid) {
          this.saveMaterial(data, saveMode);
        } else {
          this.notificationPopupParam = Resource.NotificationPopupParam.Error;
          this.isShowNotificationPopup = true;
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
          if(this.selectedMaterialUnit != null || this.selectedMaterialUnit != undefined){
            if (
              this.material.materialUnit[this.selectedMaterialUnit].method != Enum.FormMode.Add
            ) {
              this.material.materialUnit[this.selectedMaterialUnit].method =
                Enum.FormMode.Delete;
            } else {
              this.material.materialUnit.splice(this.selectedMaterialUnit, 1);
            }

            this.material.materialUnit.filter((item, index)=>{
              if(this.deleteMaterialUnitArr && item.method == Enum.FormMode.Delete){
                this.deleteMaterialUnitArr.push(item);
                this.material.materialUnit.splice(index, 1);
              }
            });
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
          this.material.materialUnit.push({
            unitID: Resource.KeyTable.EmptyGuid,
            conversionRate: 1,
            calculation: Enum.Calculation.Multiplication,
            method: Enum.FormMode.Add,
          });

          this.$nextTick(() => {
            let index = this.material.materialUnit.length - 1;
            if (this.$refs[`materialUnit${index}`]) {
              this.$refs[`materialUnit${index}`][0].componentFocus();
            }
          });
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm lấy mã nguyên vật liệu mới
     * Author: LHNAM (01/10/2022)
     */
    getNewMaterialCode(code) {
      Axios.CallAxios(
        Axios.Methods.Get,
        `${Axios.Url.NewMaterialCode}/?code=${code}`
      )
        .then((res) => {
          this.material.materialCode = res.data;
          this.isShowLoading = false;
        })
        .catch((e) => {
          this.errorMsg = CommonFn.getError(e.response);
          this.notificationPopupParam = Resource.NotificationPopupParam.Error;
          this.isShowNotificationPopup = true;
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
        .then(() => {
          if (this.param.method == Enum.FormMode.Replication) {
            this.getMaterialCode();
            this.material.materialUnit.map(item=>{
              if(item.method){
                item.method = Enum.FormMode.Add;
              }
            })
          }
          this.isShowLoading = false;
          this.isChange = false;
        })
        .catch((e) => {
          this.errorMsg = CommonFn.getError(e.response);
          this.notificationPopupParam = Resource.NotificationPopupParam.Error;
          this.isShowNotificationPopup = true;
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
        this.$emit(Resource.Emit.CloseForm, false);
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
        if (this.formMode) {
          switch (this.formMode) {
            case Enum.FormMode.Add:
              this.isChange = false;
              break;
            case Enum.FormMode.Edit:
            case Enum.FormMode.Replication:
              // Lấy dữ liệu của bản ghi được chọn
              this.isShowLoading = true;
              this.getMaterialDetail();
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

    this.debouncedGetCode = debounce(() => {
      let code = CommonFn.formatCode(this.material.materialName);
      this.getNewMaterialCode(code);
    }, 200);
  },
  mounted() {
    if (this.$refs[Resource.KeyTable.MaterialName]) {
      this.$refs[Resource.KeyTable.MaterialName].focus();
    }
  },
};
</script>

<style>
@import url(./popup.css);
</style>
