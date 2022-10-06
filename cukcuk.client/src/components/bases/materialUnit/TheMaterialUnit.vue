<template>
  <tr v-show="item.method != 3">
    <th>{{ itemIndex + 1 }}</th>
    <td>
      <BaseCombobox
        :url="units"
        :propValue="'unitID'"
        :propText="'unitName'"
        :inputValue="item.unitID"
        @getValue="getUnitID"
      />
    </td>
    <td>
      <input type="text" v-model="item.conversionRate" />
    </td>
    <td>
      <BaseSelectbox
        :selectData="calculationSelectData"
        :inputValue="item.calculation"
        @getFilter="getCalculation"
      />
    </td>
    <th>
      <div v-if="item.conversionRate">
        1 {{ materialUnitName }} = {{ item.conversionRate }}
        {{ $filters.formatCalculation(item.calculation)}}
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
export default {
  name: "MaterialUnit",
  components: { BaseSelectbox, BaseCombobox },
  props: ["materialUnit", "materialUnitName", "itemIndex"],
  watch: {
    item: {
      handler(){
        if(this.returnValue){
          this.returnValue();
        }
      },
      deep: true
    }
  },
  data() {
    return {
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
     * Hàm trả giá trị của item về cho component cha
     * Author: LHNAM (03/10/2022)
     */
    returnValue(){
      if(this.item){
        if(this.item.conversionRate){
          this.item.conversionRate = parseFloat(this.item.conversionRate);
        }
        this.$emit("returnValue", this.item, this.itemIndex + 1);
      }
    },

    /**
     * Hàm lấy giá trị trả về của calculation
     * @param {Enum} value giá trị của calculation
     * Author: LHNAM (03/10/2022)
     */
    getCalculation(value){
      this.item.calculation = value;
    },

    /**
     * Hàm lấy giá trị trả về của unitID trong combobox
     * @param {Guid} value giá trị của unitID
     * Author: LHNAM (03/10/2022)
     */
    getUnitID(value, text){
      this.item.unitID = value;
      this.unitName = text;
    }
  },
  created() {
  },
};
</script>

<style scoped>
  td{
    padding: 0;
  }
td input{
  border: none!important;
  outline: none;
}
</style>
