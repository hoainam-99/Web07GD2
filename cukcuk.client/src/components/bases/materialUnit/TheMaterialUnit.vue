<template>
  <tr v-show="item.method != 3" class="sub-tr">
    <th class="sub-td">{{ itemIndex + 1 }}</th>
    <td class="sub-td">
      <BaseCombobox
        :url="units"
        :propValue="'unitID'"
        :propText="'unitName'"
        :inputValue="item.unitID"
        :isRequire="isError"
        @getValue="getUnitID"
        ref="unitCombobox"
      />
    </td>
    <td class="sub-td">
      <BaseInput
        style="width: 100%"
        :fieldName="'conversionRate'"
        type="float"
        textRight
        lengthAfterComma="2"
        v-model:value="item.conversionRate"
        @onInput="getData"
      />
    </td>
    <td class="sub-td">
      <BaseSelectbox
        :selectData="calculationSelectData"
        :inputValue="item.calculation"
        @getFilter="getCalculation"
      />
    </td>
    <th class="sub-td">
      <div v-if="item.conversionRate">
        1 {{ materialUnitName }} = {{ item.conversionRate }}
        {{ $filters.formatCalculation(item.calculation) }}
        {{ unitName }}
      </div>
    </th>
  </tr>
</template>

<script>
import Axios from "@/js/Axios";
import Enum from "@/js/Enum";
import BaseSelectbox from "../selectbox/BaseSelectbox.vue";
import BaseCombobox from "../combobox/BaseCombobox.vue";
import Resource from "@/js/Resource";
import CommonFn from "@/js/Common";
import BaseInput from "../BaseNumberInput/BaseInput.vue";
import debounce from "lodash.debounce";

export default {
  name: "MaterialUnit",
  components: { BaseSelectbox, BaseCombobox, BaseInput },
  props: ["materialUnit", "materialUnitName", "itemIndex"],
  watch: {
    item: {
      handler() {
        if (this.debouncedReturnValue) {
          this.debouncedReturnValue();
        }
      },
      deep: true,
    },
  },
  data() {
    return {
      // biến hiện lỗi
      isError: false,

      item: this.materialUnit,
      // url đơn vị tính
      units: Axios.Url.Unit,

      // Mảng chứa phép tính
      calculationSelectData: [
        { data: "*", value: Enum.Calculation.Multiplication, isChecked: false },
        { data: "/", value: Enum.Calculation.Division, isChecked: false },
      ],

      // tên đơn vị
      unitName: "",
    };
  },
  methods: {
    /**
     * Hàm set value số cho số lượng tối thiểu
     * @param {Object} data Object chứa nội dung trả về
     * Author: LHNAM (12/10/2022)
     */
    getData(data) {
      // check null
      if (this.item && data) {
        // Khi gọi hàm component sẽ trả về
        this.item[data.fieldName] = data.val;
      }
    },
    /**
     * Hàm focus conversion unit
     * Author: LHNAM (10/10/2022)
     */
    componentFocus() {
      if (this.$refs.unitCombobox && this.$refs.unitCombobox.componentFocus) {
        this.$refs.unitCombobox.componentFocus();
      }
    },

    /**
     * Hàm trả giá trị của item về cho component cha
     * Author: LHNAM (03/10/2022)
     */
    returnValue() {
      if (this.item) {
        this.$emit(Resource.Emit.ReturnValue, this.item, this.itemIndex + 1);
      }
    },

    /**
     * Hàm lấy giá trị trả về của calculation
     * @param {Enum} value giá trị của calculation
     * Author: LHNAM (03/10/2022)
     */
    getCalculation(value) {
      this.item.calculation = value;
    },

    /**
     * Hàm lấy giá trị trả về của unitID trong combobox
     * @param {Guid} value giá trị của unitID
     * Author: LHNAM (03/10/2022)
     */
    getUnitID(value, text) {
      this.item.unitID = value;
      this.unitName = text;
    },
  },
  created() {
    this.debouncedReturnValue = debounce(() => {
      if(this.returnValue){
        this.returnValue();
      }
    }, 1000);
  },
};
</script>

<style>
.input {
  width: 100%;
  padding: 2px 6px;
  border: none;
  outline: none;
}

.sub-tr {
  height: 24px;
}

.sub-td {
  padding: 0;
}

.sub-td .combobox {
  height: 100%;
}

.sub-td .input,
.sub-td .sb-input {
  height: 24px !important;
  padding: 2px 6px !important;
}
</style>
