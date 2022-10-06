var Resource = Resource || {};

Resource.DateType = {
    Day: "Day",
    Month: "Month",
    Year: "Year"
}

Resource.ErrorMes = {
    requireError: "Không được bỏ trống trường này.",
    notFound_Error: "Bản ghi đã bị xóa hoặc không tồn tại trong hệ thống.",
    generate_Error: "Đã có lỗi xảy ra. Vui lòng liên hệ Misa để được hỗ trợ.",
    e002: "Mã nguyên vật liệu đã tồn tại trong hệ thống.",
    e003: "Mã nguyên vật liệu không được bỏ trống.",
    e004: "Tên nguyên vật liệu không được bỏ trống.",
    e005: "Mã kho đã tồn tại trong hệ thống.",
    e006: "Mã kho không được để trống.",
    e007: "Tên kho không được để trống.",
    e008: "Đơn vị tính đã tồn tại trong hệ thống.",
    e009: "Tên đơn vị tính không được để trống.",
    e010: "Mã đơn vị tính không được để trống."
}

Resource.Table = {
    Material : "Material",
    Unit : "Unit",
    Stock : "Stock"
}

export default Resource;