var Enum = Enum || {};

// enum định dạng kiểu form
Enum.FormMode = {
    Add: 1,         // thêm mới
    Edit: 2,        // sửa
    Delete: 3,      // xóa
    Replication: 4, // nhân bản
    Reset: 5        // nạp mới
}

//  enum định dạng kiểu lưu
Enum.SaveMode = {
    Save: 1,    // cất
    SaveAdd: 2  // cất và thêm
}

// enum định dạng phép tính
Enum.Calculation = {
    Multiplication: 1,  // Phép nhân
    Division: 2,        // Phép chia
}

export default Enum;