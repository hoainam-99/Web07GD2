var Resource = Resource || {};

Resource.DateType = {
    Day: "Day",
    Month: "Month",
    Year: "Year"
}

Resource.Notice = {
    CreateSuccess: "Thêm mới nguyên vật liệu thành công.",  
    UpdateSuccess: "Sửa thông tin nguyên vật liệu thành công.",

}

Resource.ErrorMes = {
    requireError: "Không được bỏ trống trường này.",
    notFound_Error: "Bản ghi đã bị xóa hoặc không tồn tại trong hệ thống.",
    generate_Error: "Đã có lỗi xảy ra. Vui lòng liên hệ Misa để được hỗ trợ.",
    numberFormat_Error: "Số không đúng định dạng.",
    e002: "Mã nguyên vật liệu đã tồn tại trong hệ thống.",
    e003: "Mã nguyên vật liệu không được bỏ trống.",
    e004: "Tên nguyên vật liệu không được bỏ trống.",
    e005: "Mã kho đã tồn tại trong hệ thống.",
    e006: "Mã kho không được để trống.",
    e007: "Tên kho không được để trống.",
    e008: "Đơn vị tính đã tồn tại trong hệ thống.",
    e009: "Tên đơn vị tính không được để trống.",
    e010: "Mã đơn vị tính không được để trống.",
    e012: "Số lượng tồn tối thiểu không đúng định dạng.",
    e011: "Đơn vị chuyển đổi không được giống với đơn vị tính."
}

Resource.Table = {
    Material : "Material",
    Unit : "Unit",
    Stock : "Stock"
}

export default Resource;