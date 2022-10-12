var Resource = Resource || {};

Resource.DateType = {
    Day: "Day",
    Month: "Month",
    Year: "Year"
}

Resource.Notice = {
    CreateMaterialSuccess: "Thêm mới nguyên vật liệu thành công.",  
    UpdateMaterialSuccess: "Sửa thông tin nguyên vật liệu thành công.",
    CreateUnitSuccess: "Thêm mới đơn vị tính thành công.",
    CreateStockSuccess: "Thêm mới kho thành công.",
    DeleteSuccess: "Xóa bản ghi thành công."
}

Resource.ErrorMes = {
    requireError: "Không được bỏ trống trường này.",
    notFound_Error: "Bản ghi đã bị xóa hoặc không tồn tại trong hệ thống.",
    generate_Error: "Đã có lỗi xảy ra. Vui lòng liên hệ Misa để được hỗ trợ.",
    numberFormat_Error: "Số không đúng định dạng.",
    conversionUnit_Unit_Diffrence: "Đơn vị chuyển đổi phải khác đơn vị tính.", 
    e002: "Mã nguyên vật liệu đã tồn tại trong hệ thống.",
    e003: "Mã nguyên vật liệu không được bỏ trống.",
    e004: "Tên nguyên vật liệu không được bỏ trống.",
    e005: "Mã kho đã tồn tại trong hệ thống.",
    e006: "Mã kho không được để trống.",
    e007: "Tên kho không được để trống.",
    e008: "Đơn vị tính đã tồn tại trong hệ thống.",
    e009: "Tên đơn vị tính không được để trống.",
    e010: "Mã đơn vị tính không được để trống.",
    e011: "Đơn vị chuyển đổi không được giống với đơn vị tính.",
    e012: "Số lượng tồn tối thiểu không đúng định dạng.",
    e013: "Đơn vị chuyển dổi không được để trống."
}

Resource.Table = {
    Material : "Material",
    Unit : "Unit",
    Stock : "Stock"
}

Resource.KeyTable = {
    MaterialID : "materialID",
    MaterialCode: "materialCode",
    MaterialName: "materialName",
    ExpiryDate: "expiryDate",
    UnitID: "unitID",
    UnitName: "unitName",
    CategoryID: "categoryID",
    CategoryCode: "categoryCode",
    CategoryName: "categoryName",
    StockID: "stockID",
    StockCode: "stockCode",
    StockName: "stockName", 
    Feature: "feature",
    InventoryNumber: "inventoryNumber",
    Description: "description",
    Status: "status",
    Calculation: "calculation",
    ConversionRate: "conversionRate",
    EmptyGuid: "00000000-0000-0000-0000-000000000000",
}

Resource.NotificationPopupParam = {
    DeleteConfirm: "deleteConfirm",
    SaveConfirm: "saveConfirm",
    Error: "error"
}

Resource.Emit = {
    ReturnResult: "returnResult",
    CloseForm: "closeForm",
    AddBtnOnClick: "addBtnOnClick",
    GetValue: "getValue",
    RefreshData: "refreshData",
    Refresh: "refresh",
    ReturnConfirmPopup: "returnConfirmPopup",
    ChangePagination: "changePagination",
    ReturnValue: "returnValue",
    GetFilter: "getFilter"
}

export default Resource;