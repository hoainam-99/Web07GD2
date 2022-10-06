<template>
  <div class="content">
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
        <button class="btn btn-refresh cursor-pointer">
          <i class="fa-solid fa-arrows-rotate"></i>
          <span>Nạp</span>
        </button>
      </div>
      <div class="content-table">
        <table border="1">
          <thead class="hn-thead">
            <tr>
              <th class="mw-200px">
                <div class="hn-th">
                  <div class="hn-th__title">Mã nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getMaterialCodeFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-300px">
                <div class="hn-th">
                  <div class="hn-th__title">Tên nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getMaterialNameFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-180px">
                <div class="hn-th">
                  <div class="hn-th__title">Tính chất</div>
                  <BaseFilter
                    @getFilter="getFeatureFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-180px">
                <div class="hn-th">
                  <div class="hn-th__title">ĐVT tính</div>
                  <BaseFilter
                    @getFilter="getUnitFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-180px">
                <div class="hn-th">
                  <div class="hn-th__title">Nhóm nguyên vật liệu</div>
                  <BaseFilter
                    @getFilter="getCategoryFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-300px">
                <div class="hn-th">
                  <div class="hn-th__title">Ghi chú</div>
                  <BaseFilter
                    @getFilter="getDescriptionFilter"
                    :filterType="'string'"
                  />
                </div>
              </th>
              <th class="mw-180px">
                <div class="hn-th">
                  <div class="hn-th__title">Theo dõi</div>
                  <div class="hn-th__filter">
                    <BaseSelectbox
                      :selectData="followSelectData"
                      @getFilter="getStatusFilter"
                    />
                  </div>
                </div>
              </th>
            </tr>
          </thead>
          <tbody class="hn-tbody">
            <tr
              class="hn-tr"
              v-for="material in materials"
              :key="material.materialID"
              @click="selectMaterial(material.materialID)"
              @dblclick="formDetailOnClick(2)"
              :class="{ 'hn-selected-tr': material.materialID == param.id }"
            >
              <td>{{ material.materialCode }}</td>
              <td>{{ material.materialName }}</td>
              <td>{{ material.feature }}</td>
              <td>{{ material.unitName }}</td>
              <td>{{ material.categoryName }}</td>
              <td>{{ material.description }}</td>
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
      <BaseLoading :top="28" v-show="isShowLoading"/>
      <div class="content-navigation">
        <BasePagination
          :totalCount="totalCount"
          @changePagination="changePagination"
          @refresh="refresh"
        />
      </div>
    </div>
  </div>
  <ThePopup v-if="isShowFormPopup" :param="param" @closeForm="closeForm" />
  <NotificationPopup
    v-if="isShowNotificationPopup"
    :param="notificationPopupParam"
    :deleteItem="selectedMaterial"
    @returnConfirmPopup="returnConfirmPopup"
    @closeNoticePopup="closeNoticePopup"
  />
</template>

<script>
import Enum from "@/js/Enum.js";
import Axios from "@/js/Axios";
import debounce from "lodash.debounce";
import { useToast } from "vue-toastification";
import ThePopup from "@/components/bases/pop-up/ThePopup.vue";
import BaseSelectbox from "@/components/bases/selectbox/BaseSelectbox.vue";
import BaseFilter from "../../bases/filter/BaseFilter.vue";
import BasePagination from "@/components/bases/pagination/BasePagination.vue";
import NotificationPopup from "@/components/bases/pop-up/NotificationPopup.vue";
import BaseLoading from "@/components/bases/BaseLoading.vue";
export default {
  components: {
    ThePopup,
    BaseSelectbox,
    BaseFilter,
    BasePagination,
    NotificationPopup,
    BaseLoading
},
  data() {
    return {
      isShowLoading: true,
      toast: useToast(),
      notificationPopupParam: "",
      selectedMaterial: {},
      param: {
        method: "",
        id: "",
      },
      totalCount: 0,
      materials: [],
      materialCodeFilter: "",
      materialNameFilter: "",
      unitFilter: "",
      featureFilter: "",
      categoryFilter: "",
      descriptionFilter: "",
      statusFilter: "",
      pagination: {},
      isShowFormPopup: false,
      isShowNotificationPopup: false,
      followSelectData: [
        {
          data: "Không",
          value: 2,
          isChecked: true,
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
     * Hàm đóng bảng thông báo
     * @param {Boolean} e giá trị trả về
     * Author: LHNAM (05/10/2022)
     */
    closeNoticePopup(e){
      if(this.isShowNotificationPopup){
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
          this.toast.success("Xóa bản ghi thành công.", {
            timeout: 2000,
            hideProgressBar: false,
          });
          this.isShowNotificationPopup = false;
          this.refresh(true);
        })
        .catch(() => {
          this.notificationPopupParam = "error";
        })
        .finally(() => {
          this.loading = false;
        });
    },

    /**
     * Hàm chọn nguyên vật liệu
     * @param {Guid} id Id của nguyên vật liệu được chọn
     * Author: LHNAM (01/10/2022)
     */
    selectMaterial(id) {
      try {
        if (id) {
          this.param.id = id;

          this.selectedMaterial = this.materials.find((item) => {
            return (item.id = id);
          });
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
      Axios.CallAxios(
        Axios.Methods.Get,
        `${Axios.Url.FilterMaterial}?materialCode=${this.materialCodeFilter}&materialName=${this.materialNameFilter}&feature=${this.featureFilter}&unit=${this.unitFilter}&category=${this.categoryFilter}&description=${this.descriptionFilter}&status=${this.statusFilter}&pageSize=${this.pagination.pageSize}&pageNum=${this.pagination.pageNum}`
      )
        .then((res) => {
          this.materials = res.data.data;
          this.totalCount = res.data.totalCount;
          this.isShowLoading = false;
        })
        .catch(() => {
          this.notificationPopupParam = "error";
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
      if (e){
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
     * @param {Guid} id Id của đối tượng cần gọi (có thể có hoặc không)
     * Author: LHNAM (28/09/2022)
     */
    formDetailOnClick(formMode) {
      try {
        switch (formMode) {
          case Enum.FormMode.Add:
            this.param.method = formMode;
            this.isShowFormPopup = true;
            break;
          case Enum.FormMode.Replication:
          case Enum.FormMode.Edit:
            if (this.param.id) {
              this.param.method = formMode;
              this.isShowFormPopup = true;
            }
            break;
          case Enum.FormMode.Delete:
            if (this.param.id) {
              this.notificationPopupParam = "deleteConfirm";
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
      this.getMaterial();
    }, 500);
  },
};
</script>

<style>
@import url(./content.css);
</style>
