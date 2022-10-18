<template>
  <div
    class="content"
    @keydown.prevent.down="selectedIndexPlus"
    @keydown.prevent.up="selectedIndexMinus"
  >
    <!-- @keydown.prevent.enter="formDetailOnClick(2)" -->
    <div class="content-header">
      <div class="content-header__left">Nguyên vật liệu</div>
      <div class="content-header__right">
        <button class="btn btn-feedback cursor-pointer">
          <i class="fa-solid fa-bullhorn"></i>
          <span>Phản hồi</span>
        </button>
      </div>
    </div>
    <div class="content-body">
      <div class="content-toolbar">
        <button
          class="btn btn-add cursor-pointer"
          @click="formDetailOnClick(1)"
          :disabled="isShowLoading"
        >
          <i class="fa-solid fa-file-circle-plus"></i>
          <span>Thêm</span>
        </button>
        <button
          class="btn btn-replication cursor-pointer"
          @click="formDetailOnClick(4)"
          :disabled="isShowLoading"
        >
          <i class="fa-regular fa-copy"></i>
          <span>Nhân bản</span>
        </button>
        <button
          class="btn btn-edit cursor-pointer"
          @click="formDetailOnClick(2)"
          :disabled="isShowLoading"
        >
          <i class="fa-regular fa-pen-to-square"></i>
          <span>Sửa</span>
        </button>
        <button
          class="btn btn-delete cursor-pointer"
          @click="formDetailOnClick(3)"
          :disabled="isShowLoading"
        >
          <i class="fa-solid fa-x"></i>
          <span>Xóa</span>
        </button>
        <button class="btn btn-refresh cursor-pointer" @click="refreshFilter">
          <i class="fa-solid fa-arrows-rotate"></i>
          <span>Nạp</span>
        </button>
      </div>
      <div class="content-table">
        <table border="1">
          <thead class="hn-thead">
            <tr>
              <th style="min-width: 150px">
                <div class="hn-th">
                  <div class="hn-th__title">Mã nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getMaterialCodeFilter"
                    :filterType="'string'"
                    ref="materialCodeFilter"
                  />
                </div>
              </th>
              <th style="min-width: 225px">
                <div class="hn-th">
                  <div class="hn-th__title">Tên nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getMaterialNameFilter"
                    :filterType="'string'"
                    ref="materialNameFilter"
                  />
                </div>
              </th>
              <th style="min-width: 150px">
                <div class="hn-th">
                  <div class="hn-th__title">Tính chất</div>
                  <BaseFilter
                    @getFilter="getFeatureFilter"
                    :filterType="'string'"
                    ref="featureFilter"
                  />
                </div>
              </th>
              <th style="min-width: 140px">
                <div class="hn-th">
                  <div class="hn-th__title" title="Đơn vị tính">ĐVT tính</div>
                  <BaseFilter
                    @getFilter="getUnitFilter"
                    :filterType="'string'"
                    ref="unitFilter"
                  />
                </div>
              </th>
              <th style="min-width: 150px">
                <div class="hn-th">
                  <div class="hn-th__title">Nhóm nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getCategoryFilter"
                    :filterType="'string'"
                    ref="categoryFilter"
                  />
                </div>
              </th>
              <th style="min-width: 250px">
                <div class="hn-th">
                  <div class="hn-th__title">Ghi chú</div>
                  <BaseFilter
                    @getFilter="getDescriptionFilter"
                    :filterType="'string'"
                    ref="descriptionFilter"
                  />
                </div>
              </th>
              <th style="min-width: 125px">
                <div class="hn-th">
                  <div class="hn-th__title">Theo dõi</div>
                  <div class="hn-th__filter">
                    <BaseSelectbox
                      :selectData="followSelectData"
                      :inputValue="2"
                      @getFilter="getStatusFilter"
                      ref="followFilter"
                    />
                  </div>
                </div>
              </th>
            </tr>
          </thead>
          <tbody class="hn-tbody">
            <tr
              class="hn-tr"
              v-for="(material, index) in materials"
              :key="material.materialID"
              @click="selectMaterial(index)"
              @dblclick="formDetailOnClick(2)"
              :class="{ 'hn-selected-tr': selectedItemIndex == index }"
            >
              <td>{{ material.materialCode }}</td>
              <td class="hide-td">{{ material.materialName }}</td>
              <td>{{ material.feature }}</td>
              <td>{{ material.unitName }}</td>
              <td>{{ material.categoryName }}</td>
              <td class="hide-td" :title="material.description">
                {{ material.description }}
              </td>
              <td>
                <div class="useCheck">
                  <input
                    type="checkbox"
                    :checked="material.status == 1"
                    name=""
                    id=""
                  />
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <BaseLoading :top="28" v-show="isShowLoading" />
      <div class="content-navigation">
        <BasePagination
          :totalCount="totalCount"
          @changePagination="changePagination"
          @refresh="refresh"
          ref="pagination"
        />
      </div>
    </div>
  </div>
  <ThePopup
    v-if="isShowFormPopup"
    :param="param"
    @closeForm="closeForm"
    @returnResult="refresh"
  />
  <NotificationPopup
    v-if="isShowNotificationPopup"
    :param="notificationPopupParam"
    :deleteItem="selectedMaterial"
    :errorMsg="errorMsg"
    @returnConfirmPopup="returnConfirmPopup"
    @closeForm="closeNoticePopup"
  />
</template>

<script>
import Enum from "@/js/Enum.js";
import CommonFn from "@/js/Common";
import Axios from "@/js/Axios";
import debounce from "lodash.debounce";
import { useToast } from "vue-toastification";
import ThePopup from "@/components/bases/pop-up/ThePopup.vue";
import BaseSelectbox from "@/components/bases/selectbox/BaseSelectbox.vue";
import BaseFilter from "../../bases/filter/BaseFilter.vue";
import BasePagination from "@/components/bases/pagination/BasePagination.vue";
import NotificationPopup from "@/components/bases/pop-up/NotificationPopup.vue";
import BaseLoading from "@/components/bases/BaseLoading.vue";
import Resource from "@/js/Resource";
export default {
  components: {
    ThePopup,
    BaseSelectbox,
    BaseFilter,
    BasePagination,
    NotificationPopup,
    BaseLoading,
  },
  data() {
    return {
      // Mảng chứa lỗi
      errorMsg: [],

      // Index của bản ghi được chọn
      selectedItemIndex: 0,

      // hiển thị loading
      isShowLoading: true,

      // biến sử dụng toast
      toast: useToast(),

      // biến chứa đối tượng được chọn
      selectedMaterial: {},

      // biến tổng số bản ghi thỏa mãn điều kiện lọc
      totalCount: 0,

      // mảng chứa bản ghi phù hợp điều kiện lọc và phân trang
      materials: [],

      // các biến chứa điều kiện lọc
      materialCodeFilter: "",
      materialNameFilter: "",
      unitFilter: "",
      featureFilter: "",
      categoryFilter: "",
      descriptionFilter: "",
      statusFilter: "",

      // biến chứa điều kiện phân trang
      pagination: {},

      // biến hiển thị pop-up form
      isShowFormPopup: false,

      // param truyền vào pop-up form
      param: {
        method: "",
        id: "",
      },

      // biến hiển thị pop-up thông báo
      isShowNotificationPopup: false,

      // param truyền vào pop-up thông báo
      notificationPopupParam: "",

      followSelectData: [
        {
          data: "Không",
          value: 2,
          isChecked: false,
        },
        {
          data: "Có",
          value: 1,
          isChecked: false,
        },
      ],
    };
  },
  watch: {
    selectedItemIndex(newValue) {
      if (newValue < 0) {
        this.selectedItemIndex = 0;
      }

      if (newValue > this.materials.length - 1) {
        this.selectedItemIndex = this.materials.length - 1;
      }

      this.setMaterialIDForParam();
    },
    pagination: {
      handler() {
        this.debouncedGetData();
      },
      deep: true,
    },
    materialCodeFilter() {
      this.debouncedGetData();
    },
    materialNameFilter() {
      this.debouncedGetData();
    },
    unitFilter() {
      this.debouncedGetData();
    },
    featureFilter() {
      this.debouncedGetData();
    },
    categoryFilter() {
      this.debouncedGetData();
    },
    descriptionFilter() {
      this.debouncedGetData();
    },
    statusFilter() {
      this.debouncedGetData();
    },
  },
  methods: {
    /**
     * Hàm gọi đến khi bấm nút nạp
     * Author: LHNAM (07/10/2022)
     */
    refreshFilter() {
      try {
        debugger
        // refresh lại bộ lọc
        if (this.$refs) {
          Object.keys(this.$refs).forEach((item) => {
            if (this.$refs[item].refreshValue) {
              this.$refs[item].refreshValue();
            }
            if (item == Resource.KeyTable.Pagination) {
              if (this.$refs[Resource.KeyTable.Pagination].resetPagination) {
                this.$refs[Resource.KeyTable.Pagination].resetPagination();
              }
            }
          });
        }

        // xét bản ghi được chọn là bản ghi đầu tiên
        this.selectedItemIndex = 0;

        // Lấy dữ liệu
        this.getMaterial();
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thay đổi bản ghi được chọn khi bấm nút lên
     * Author: LHNAM (07/10/2022)
     */
    selectedIndexMinus() {
      try {
        this.selectedItemIndex--;
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thay đổi bản ghi được chọn khi bấm nút xuống
     * Author: LHNAM (07/10/2022)
     */
    selectedIndexPlus() {
      try {
        this.selectedItemIndex++;
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm đóng bảng thông báo
     * @param {Boolean} e giá trị trả về
     * Author: LHNAM (05/10/2022)
     */
    closeNoticePopup(e) {
      if (this.isShowNotificationPopup) {
        this.isShowNotificationPopup = e;
      }
    },

    /**
     * Hàm call api để xóa một nguyên vật liệu
     * Author: LHNAM (04/10/2022)
     */
    deleteMaterial() {
      Axios.CallAxios(
        Axios.Methods.Delete,
        `${Axios.Url.Material}/${this.param.id}`
      )
        .then(() => {
          this.toast.success(Resource.Notice.DeleteSuccess, {
            timeout: 2000,
            hideProgressBar: false,
          });

          // tắt thông báo pop-up
          this.isShowNotificationPopup = false;

          this.refresh(true);
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
     * Hàm set giá trị cho biến param
     * Author: LHNAM (07/10/2022)
     */
    setMaterialIDForParam() {
      if (this.materials[this.selectedItemIndex]) {
        this.param.id = this.materials[this.selectedItemIndex].materialID;

        this.selectedMaterial = this.materials[this.selectedItemIndex];
      }
    },

    /**
     * Hàm chọn nguyên vật liệu
     * @param {Guid} id Id của nguyên vật liệu được chọn
     * Author: LHNAM (01/10/2022)
     */
    selectMaterial(index) {
      try {
        if (
          this.selectedItemIndex != null ||
          this.selectedItemIndex != undefined
        ) {
          if (index != null || index != undefined) {
            this.selectedItemIndex = index;
          }

          this.setMaterialIDForParam();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm refresh bảng
     * @param {boolean} isRefresh truyền về giá trị refresh
     * Author: LHNAM (01/10/2022)
     */
    refresh(isRefresh) {
      if (isRefresh) {
        // xét lại giá trị cho item được chọn về đầu
        this.selectedItemIndex = 0;

        // lấy dữ liệu mới về
        this.getMaterial();
      }
    },
    /**
     * Hàm lấy dữ liệu phân trang từ component con
     * @param {Object} pagination dữ liệu phân trang
     * Author: LHNAM (30/09/2022)
     */
    changePagination(pagination) {
      if (pagination) {
        this.pagination = pagination;
      }
    },

    /**
     * Hàm gọi api lấy dữ liệu binding lên bảng
     * Author: LHNAM (30/09/2022)
     */
    getMaterial() {
      this.isShowLoading = true;
      Axios.CallAxios(
        Axios.Methods.Get,
        `${Axios.Url.FilterMaterial}?materialCode=${this.materialCodeFilter}&materialName=${this.materialNameFilter}&feature=${this.featureFilter}&unit=${this.unitFilter}&category=${this.categoryFilter}&description=${this.descriptionFilter}&status=${this.statusFilter}&pageSize=${this.pagination.pageSize}&pageNum=${this.pagination.pageNum}`
      )
        .then((res) => {
          this.materials = res.data.data;
          this.totalCount = res.data.totalCount;
          setTimeout(() => {
            this.isShowLoading = false;
          }, 500);
        })
        .then(() => {
          this.setMaterialIDForParam();
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
     * Hàm lấy giá trị filter cho cột Status
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getStatusFilter(filterValue) {
      if (filterValue) {
        this.statusFilter = filterValue;
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột Description
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getDescriptionFilter(filterValue) {
      if (filterValue) {
        this.descriptionFilter = filterValue;
      } else {
        this.descriptionFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột Category
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getCategoryFilter(filterValue) {
      if (filterValue) {
        this.categoryFilter = filterValue;
      } else {
        this.categoryFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột Unit
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getUnitFilter(filterValue) {
      if (filterValue) {
        this.unitFilter = filterValue;
      } else {
        this.unitFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột Feature
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getFeatureFilter(filterValue) {
      if (filterValue) {
        this.featureFilter = filterValue;
      } else {
        this.featureFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột Material Name
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getMaterialNameFilter(filterValue) {
      if (filterValue) {
        this.materialNameFilter = filterValue;
      } else {
        this.materialNameFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị filter cho cột MaterialCode
     * @param {string} filterValue chuỗi filter
     * Author: LHNAM (30/09/2022)
     */
    getMaterialCodeFilter(filterValue) {
      if (filterValue) {
        this.materialCodeFilter = filterValue;
      } else {
        this.materialCodeFilter = "";
      }
    },

    /**
     * Hàm lấy giá trị confirm của người dùng
     * @param {boolean} e giá trị trả về từ emit của notification popup
     * Author: LHNAM (04/10/2022)
     */
    returnConfirmPopup(e) {
      if (e) {
        this.deleteMaterial();
      }
    },

    /**
     * Hàm đóng form pop-up
     * @param {Boolean} e giá trị trả về từ emit của form
     * Author: LHNAM (28/09/2022)
     */
    closeForm(e) {
      if (e != null && e != undefined && this.isShowFormPopup) {
        this.isShowFormPopup = e;
      }
    },

    /**
     * Hàm click các phương thức liên quan đến form pop-up
     * @param {enum} formMode Phương thức cần gọi
     * Author: LHNAM (28/09/2022)
     */
    formDetailOnClick(formMode) {
      try {
        // xét giá trị cho biến param để truyền vào component pop-up
        switch (formMode) {
          // pop-up để thêm mới
          case Enum.FormMode.Add:
            this.param.method = formMode;
            this.isShowFormPopup = true;
            break;
          // pop-up để sửa và nhân bản
          case Enum.FormMode.Replication:
          case Enum.FormMode.Edit:
            if (this.param.id) {
              this.param.method = formMode;
              this.isShowFormPopup = true;
            }
            break;
          // nếu là thao tác xóa thì sẽ mở pop-up confirm
          case Enum.FormMode.Delete:
            if (this.param.id) {
              this.notificationPopupParam =
                Resource.NotificationPopupParam.DeleteConfirm;
              this.isShowNotificationPopup = true;
            }
            break;
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
  created() {
    this.debouncedGetData = debounce(() => {
      if (this.pagination) {
        if (this.pagination.pageNum != "") {
          this.getMaterial();
        }
      }
    }, 500);
  },
};
</script>

<style>
@import url(./content.css);
</style>
